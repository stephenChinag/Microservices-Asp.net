


using Shared.Events;

namespace Shared.Interfaces
{

    public interface IEventBus
    {

        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
    public interface IIntegrationEventHandler<in TIntegrationEvent>
       where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }
}