# ACK, NACK e Dead Letter Queue no RabbitMQ

Este documento explica os conceitos de confirmação de mensagem (ack/nack) e como eles se conectam com Dead Letter Queues (DLQ), usando como referência a implementação em `Messaging.Shared` (`RabbitMQConsumer.cs`).

## 1. O problema que o ACK resolve

Quando um consumer se conecta a uma fila e pede pra receber mensagens (`BasicConsumeAsync`), o RabbitMQ **entrega** a mensagem, mas por padrão continua guardando uma cópia dela até ter certeza de que ela foi processada com sucesso. Essa "cópia guardada" existe porque entre o broker entregar a mensagem e o consumer terminar de processá-la, várias coisas podem dar errado:

- o processo do consumer pode cair no meio do processamento;
- a conexão TCP pode cair;
- o handler pode lançar uma exceção;
- a máquina pode reiniciar.

Se o broker simplesmente apagasse a mensagem da fila no momento em que a entregou, qualquer uma dessas falhas faria a mensagem **desaparecer sem nunca ter sido processada de verdade** — perda silenciosa de dado.

O **ACK** (`acknowledgement`, confirmação) é o consumer avisando explicitamente ao broker: *"recebi essa mensagem e terminei de processá-la com sucesso, pode apagar ela da fila"*. Só depois desse aviso o RabbitMQ remove a mensagem definitivamente.

## 2. `autoAck` vs `autoAck: false` (manual ack)

O RabbitMQ.Client permite dois modos:

- **`autoAck: true`** — o broker considera a mensagem entregue = processada, no instante em que ela é enviada pela rede ao consumer. Mais simples, porém **perigoso**: se o handler falhar depois de receber a mensagem, ela já foi removida da fila e é perdida pra sempre.
- **`autoAck: false`** (o que o projeto usa) — o broker mantém a mensagem "em voo" (unacked) até o consumer confirmar manualmente. Se a conexão do consumer cair antes de confirmar, o RabbitMQ **redevolve a mensagem pra fila automaticamente** (redelivery), pra ser entregue a outro consumer (ou ao mesmo, depois de reconectar).

No código do projeto:

```csharp
// RabbitMQConsumer.cs
await _channel.BasicConsumeAsync(route.Queue, autoAck: false, consumer: consumer);
```

`autoAck: false` é a escolha certa para qualquer processamento que tenha efeito importante (gravar no banco, chamar outro serviço, etc.) — é o que garante a semântica **"at least once"** (a mensagem só some da fila depois de confirmada, então na pior hipótese ela é processada mais de uma vez, nunca zero vezes).

## 3. `BasicAckAsync` — confirmando sucesso

```csharp
await handle(data);
await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
```

- `DeliveryTag` é um identificador numérico que o broker atribui a cada entrega, único dentro do canal (`IChannel`). É assim que o `ack` sabe **qual** mensagem está sendo confirmada.
- `multiple: false` significa "confirme só essa mensagem". Se fosse `true`, confirmaria essa e **todas as anteriores ainda não confirmadas** no mesmo canal — útil pra confirmar em lote, mas não é o caso aqui, já que cada mensagem é tratada isoladamente.

Depois do `ack`, a mensagem é removida da fila definitivamente. Se o handler nunca chegar a essa linha (porque lançou exceção antes), o ack nunca acontece — e é aí que entra o nack.

## 4. `BasicNackAsync` — confirmando falha

```csharp
catch (Exception ex)
{
    logger.LogError(ex, "Failed to handle message from queue {Queue}", route.Queue);
    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
}
```

`nack` é o oposto do `ack`: o consumer está dizendo *"recebi essa mensagem, mas não consegui processá-la"*. O parâmetro `requeue` decide o que o broker faz com ela em seguida:

- **`requeue: true`** — o broker coloca a mensagem de volta na fila pra ser entregue novamente (a qualquer consumer disponível, podendo ser o mesmo). É "tentar de novo".
- **`requeue: false`** — o broker **descarta** a mensagem da fila original. Se não houver nada mais configurado, ela é perdida de vez. Mas se a fila tiver uma **Dead Letter Exchange (DLX)** configurada, em vez de descartar, o broker roteia a mensagem pra lá.

Isso já aparece no projeto pra JSON inválido:

```csharp
if (data is null)
{
    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: false);
    return;
}
```
Faz sentido: se a mensagem nunca vai conseguir ser desserializada, tentar de novo (`requeue: true`) não resolve nada — ela vai falhar do mesmo jeito pra sempre. Por isso `requeue: false` aqui.

O problema está no outro `catch`: **toda falha de handler cai em `requeue: true`, sem limite**. Se o handler falha por um motivo permanente (bug, dado que sempre quebra alguma regra, dependência externa fora do ar por horas), a mensagem é reentregue → falha de novo → reentregue → falha de novo... **para sempre**, gastando CPU e enchendo o log, sem que ninguém saiba que aquilo virou uma mensagem "presa".

## 5. Dead Letter Exchange / Dead Letter Queue (DLX/DLQ)

Uma **Dead Letter Exchange** é só uma exchange comum do RabbitMQ, sem nada de especial na sua criação — o que a torna "dead letter" é o fato de outra fila apontar pra ela através de um argumento na hora da declaração:

```csharp
await _channel.QueueDeclareAsync(
    route.Queue,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: new Dictionary<string, object?>
    {
        ["x-dead-letter-exchange"] = $"{route.Exchange}.dlx"
    });
```

A partir daí, o RabbitMQ redireciona automaticamente pra essa exchange qualquer mensagem da fila principal que seja:

1. **rejeitada** com `nack`/`reject` e `requeue: false`;
2. **expirada** por TTL (`x-message-ttl`), se configurado;
3. **descartada** por a fila ter estourado um limite (`x-max-length`), se configurado.

A "Dead Letter Queue" (DLQ) é simplesmente a fila que você vincula (bind) a essa DLX pra receber essas mensagens rejeitadas — não é um recurso especial do RabbitMQ, é uma fila normal usada com esse propósito.

```
┌─────────────┐   publish   ┌──────────────┐   routing key   ┌────────────────┐
│  Publisher   │ ──────────▶ │   Exchange   │ ───────────────▶ │  Fila principal │
└─────────────┘             └──────────────┘                  └────────┬────────┘
                                                                          │
                                                          consumer falha  │ nack(requeue:false)
                                                          repetidamente   ▼
                                                                 ┌─────────────────┐
                                                                 │   DLX (exchange)  │
                                                                 └────────┬──────────┘
                                                                          │ bind
                                                                          ▼
                                                                 ┌─────────────────┐
                                                                 │  DLQ (fila)       │
                                                                 │  mensagens presas │
                                                                 │  p/ inspeção      │
                                                                 └─────────────────┘
```

### Por que isso importa

Sem DLQ, uma mensagem "envenenada" (poison message) ou fica em loop infinito de reentrega (`requeue: true` sempre) ou é perdida silenciosamente (`requeue: false` sem DLX configurada). Com DLQ:

- a mensagem problemática sai da fila principal (parando o loop e liberando o consumer pra processar as próximas mensagens saudáveis);
- ela não é perdida — fica visível e inspecionável na fila de dead-letter (dá pra ver pelo RabbitMQ Management UI, por exemplo);
- alguém (ou algum processo automático) pode depois investigar o que houve, corrigir o bug ou os dados, e reprocessar manualmente a mensagem, republicando-a na fila original.

### O que falta implementar no projeto

Hoje `RabbitMQConsumer.Consume<T>` não declara nenhuma DLX/DLQ, e o `catch` sempre faz `requeue: true` sem limite de tentativas — ou seja, o cenário de "falha permanente = loop eterno" descrito acima é real no código atual. A sugestão discutida foi:

1. Declarar, junto com a fila principal, uma DLX (`{exchange}.dlx`) e uma DLQ (`{queue}.dlq`), e apontar a fila principal pra essa DLX via `x-dead-letter-exchange`.
2. Trocar o `nack(requeue: true)` incondicional por uma lógica com **limite de tentativas** — usando o header `x-death` (que o próprio RabbitMQ preenche automaticamente toda vez que uma mensagem passa por uma DLX, com a contagem de quantas vezes isso já aconteceu) pra decidir: tenta de novo enquanto estiver abaixo do limite, e faz `nack(requeue: false)` definitivo (indo pra DLQ) quando estourar.

## 6. Resumo rápido

| Ação | Significado | Efeito na fila |
|---|---|---|
| `BasicAckAsync` | "processei com sucesso" | mensagem removida definitivamente |
| `BasicNackAsync(requeue: true)` | "falhou, tenta de novo" | mensagem volta pra mesma fila |
| `BasicNackAsync(requeue: false)`, sem DLX | "falhou, desiste" | mensagem descartada e perdida |
| `BasicNackAsync(requeue: false)`, com DLX configurada | "falhou, desiste" | mensagem roteada pra DLQ, preservada |
