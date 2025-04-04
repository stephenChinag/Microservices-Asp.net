
using  Shared.Models;

namespace Shared.Events
{
    // Base integration event class
    public abstract class IntegrationEvent
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }

    // Order created event
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public int OrderId { get; private set; }
        public int UserId { get; private set; }
        public List<OrderItemDto> OrderItems { get; private set; }

        public OrderCreatedIntegrationEvent(int orderId, int userId, List<OrderItemDto> orderItems)
        {
            OrderId = orderId;
            UserId = userId;
            OrderItems = orderItems;
        }
    }

    // User created event
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }

        public UserCreatedIntegrationEvent(int userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}