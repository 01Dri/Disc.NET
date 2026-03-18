using Disc.NET.Handlers;
using Disc.NET.Shared.Configurations;
using Disc.NET.Shared.Enums;
using System.Reflection;
using System.Text.Json;

namespace Disc.NET.Dispatcher
{
    internal class EventDispatcher
    {
        private readonly List<IHandler> _handlers;

        private readonly AppConfiguration _appConfiguration;
        public EventDispatcher(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            _handlers = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IHandler).IsAssignableFrom(t)
                    && !t.IsInterface
                    && !t.IsAbstract)
                    .Select(t => (IHandler)Activator.CreateInstance(t, appConfiguration)!)
                    .ToList();
        }
        public async Task DispatchAsync(EventHandlerPayload payload)
        {
            var eventType = payload.EventType;
            var handlers = _handlers.Where(x => x.GetEventType() == eventType).ToList();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(payload, _appConfiguration);
            }
        }
    }

    internal class EventHandlerPayload(GatewayEvent eventType,MessageType messageType, JsonDocument data)
    {
        public GatewayEvent EventType { get; } = eventType;
        public JsonDocument Data { get; } = data;
        public MessageType MessageType { get; } = messageType;

    }
}
