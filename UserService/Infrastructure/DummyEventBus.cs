using Shared.Events;
using Shared.Interfaces;

namespace UserService.Infrastructure
{
    public class DummyEventBus : IEventBus
    {
        public void Publish(IntegrationEvent @event)
        {
            // Dummy implementation that does nothing
            Console.WriteLine($"[DummyEventBus] Event published: {@event.GetType().Name}");
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            // Dummy implementation that does nothing
            Console.WriteLine($"[DummyEventBus] Subscribed to: {typeof(T).Name} with handler {typeof(TH).Name}");
        }
    }
}