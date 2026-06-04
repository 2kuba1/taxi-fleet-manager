using Domain.Common;

namespace Domain.Entities;

public class CarFleet : BaseEntity
{
    public Guid TeamId { get; set; }
    public Team Team { get; set; }

    public List<Car> Cars { get; set; } = new();
}