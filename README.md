# Disc.NET

**Disc.NET** é um projeto experimental para a construção de uma biblioteca de interação com a API do Discord, utilizando .NET 8+. O objetivo é criar uma alternativa modular, focada em performance e facilidade de uso através de padrões modernos de desenvolvimento.

O projeto ainda está em **fase ativa de desenvolvimento** e explora arquiteturas desacopladas para lidar com a complexidade da Gateway e das interações do Discord.

---

## Como o projeto funciona?

A arquitetura do Disc.NET gira em torno de três pilares principais:

### Event Dispatcher & Handlers
O coração do sistema é o `EventDispatcher`. Ele recebe os payloads brutos da Gateway e os roteia para **Handlers** específicos. 
- Cada Handler é responsável por um tipo de evento (ex: `InteractionCreate`, `MessageCreate`).
- Isso permite que a lógica de processamento seja isolada e fácil de estender sem sujar o código principal da conexão.

### Comandos via Attributes
Chega de `if/else` gigantes para validar comandos. O Disc.NET utiliza **Attributes** para mapear classes de comando automaticamente:
- Basta decorar sua classe com `[SlashCommand]` ou `[PrefixCommand]`.
- O sistema de registro faz o *scan* das classes e vincula a execução ao trigger correto via Reflection.

### Service Container (DI)
Utilizamos um `DiscNetContainer` interno para gerenciar dependências. Isso garante que seus comandos tenham acesso fácil a serviços de configuração, clientes de API e bancos de dados de forma nativa.

---

## Instalação via NuGet

Instale o pacote principal:

```bash
dotnet add package Disc.NET
```

Se precisar instalar os módulos separadamente:

```bash
dotnet add package Disc.NET.Client.SDK
dotnet add package Disc.NET.Commands
```

---

## Exemplo de Uso (Experimental)

```csharp
// 1. Defina o comando usando atributos
[SlashCommand("ping", InteractionType.ApplicationCommand, "Testa a latência")]
public class PingCommand : ISlashCommand
{
    public async Task RunAsync(InteractionContext context, CancellationToken ct = default)
    {
       await context.Response.SendMessageAsync(new Message()
       {
           Content = "Hello World!",
       });
    }
}

// 2. Inicialize o AppBuilder
var app = new AppBuilder()
    .AddConfiguration(new AppConfiguration("TOKEN") { ApplicationId = 123 })
    .Build();

await app.RunAsync();
```

### Exemplo com Message Builders

```csharp
using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.MessageBuilders;

[SlashCommand("ping", InteractionType.ApplicationCommand, "Testa a latência")]
public class PingCommand : ISlashCommand
{
    public async Task RunAsync(InteractionContext context, CancellationToken ct = default)
    {
        var message = new MessageBuilder()
            .WithContent("Hello World!")
            .WithEmbed(embed => embed
                .SetTitle("Pong!")
                .SetDescription("Exemplo de mensagem usando builders.")
                .SetColor(0x5865F2)
                .AddField("Status", "Online", true))
            .WithActionRow(
                new ActionRowBuilder()
                    .AddButton("Atualizar", "ping_refresh", ButtonStyle.Primary)
                    .AddLinkButton("Repositório", "https://github.com/dridev/Disc.NET"))
            .Build();

        await context.Response.SendMessageAsync(message, ct);
    }
}
```

---

## 📦 Módulos Atuais
- **Disc.NET.Client.SDK**: Abstração da API REST.
- **Disc.NET.Commands**: Motor de execução de comandos e contextos.
- **Disc.NET.Shared**: Utilitários de serialização e extensões de sistema.
- **Disc.NET.Components**: Builders para botões, selects e embeds.

---

> ⚠️ **Aviso:** Por ser um projeto experimental, mudanças drásticas na API podem ocorrer a qualquer momento.
