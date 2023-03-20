using ECommerceBE.Domain.Entities.Common;

namespace ECommerceBE.Domain.Entities
{
    public class CompletedOrder : BaseEntity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
