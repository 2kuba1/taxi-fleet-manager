using Domain.Common;

namespace Domain.Entities;

public class CarFleet : BaseEntity
{
    public Guid TeamId { get; private set; }
    public Team Team { get; private set; }

    public List<Car> Cars { get; set; } = new();
    
    protected CarFleet(){}

    private CarFleet(Guid teamId)
    {
        TeamId = teamId;
    }

    public static CarFleet Create(Guid teamId)
    {
        if (teamId == Guid.Empty)
            throw new ArgumentException("Team id cannot be empty");
        
        return new CarFleet(teamId);
    }
}