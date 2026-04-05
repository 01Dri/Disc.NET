# Disc.NET

**Disc.NET** is an experimental project focused on building a .NET 8+ library for interacting with the Discord API. The goal is to provide a modular alternative with strong performance and a modern developer experience.

The project is still in **active development** and explores decoupled architectures to handle the complexity of the Discord Gateway and interactions.

---

## How does the project work?

The Disc.NET architecture is built around three main pillars:

### Event Dispatcher & Handlers
The core of the system is the `EventDispatcher`. It receives raw Gateway payloads and routes them to specific **Handlers**.
- Each Handler is responsible for a specific event type, such as `InteractionCreate` or `MessageCreate`.
- This keeps processing logic isolated and makes the system easier to extend without cluttering the main connection flow.

### Commands via Attributes
Instead of large `if/else` blocks to validate commands, Disc.NET uses **Attributes** to map command classes automatically:
- Decorate your class with `[SlashCommand]` or `[PrefixCommand]`.
- The registration system scans the classes and binds execution to the correct trigger through Reflection.

### Service Container (DI)
Disc.NET includes an internal `DiscNetContainer` to manage dependencies. This gives commands native access to configuration services, API clients, and databases.

---

## Installation via NuGet

Install the main package:

```bash
dotnet add package Disc.NET
```

If you need to install the modules separately:

```bash
dotnet add package Disc.NET.Client.SDK
dotnet add package Disc.NET.Commands
```

---

## Usage Example (Experimental)

```csharp
// 1. Define the command using attributes
[SlashCommand("ping", InteractionType.ApplicationCommand, "Checks latency")]
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

// 2. Initialize AppBuilder
var app = new AppBuilder()
    .AddConfiguration(new AppConfiguration("TOKEN") { ApplicationId = 123 })
    .Build();

await app.RunAsync();
```

### Example with Message Builders

```csharp
using Disc.NET.Client.SDK.Messages.Components.Buttons;
using Disc.NET.Commands;
using Disc.NET.Commands.MessageBuilders;

[SlashCommand("ping", InteractionType.ApplicationCommand, "Checks latency")]
public class PingCommand : ISlashCommand
{
    public async Task RunAsync(InteractionContext context, CancellationToken ct = default)
    {
        var message = new MessageBuilder()
            .WithContent("Hello World!")
            .WithEmbed(embed => embed
                .SetTitle("Pong!")
                .SetDescription("Example message built with builders.")
                .SetColor(0x5865F2)
                .AddField("Status", "Online", true))
            .WithActionRow(
                new ActionRowBuilder()
                    .AddButton("Refresh", "ping_refresh", ButtonStyle.Primary)
                    .AddLinkButton("Repository", "https://github.com/dridev/Disc.NET"))
            .Build();

        await context.Response.SendMessageAsync(message, ct);
    }
}
```

---

## Current Modules
- **Disc.NET.Client.SDK**: Discord REST API abstraction.
- **Disc.NET.Commands**: Command execution engine and contexts.
- **Disc.NET.Shared**: Serialization utilities and system extensions.
- **Disc.NET.Components**: Builders for buttons, selects, and embeds.

---

> Warning: Since this is an experimental project, breaking API changes may happen at any time.
