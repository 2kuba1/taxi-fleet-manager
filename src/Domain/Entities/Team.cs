using Domain.Common;

namespace Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; private set; }
    public Guid OwnerId { get; private set; }
    public User Owner { get; set; }
    
    public Guid CarFleetId { get; private set; }
    public CarFleet CarFleet { get; set; }
    
    public List<User> Users { get; set; } = new();
    
    protected  Team(){}

    private Team(string name, Guid ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    #region TeamFactorySummary
        /// <summary>
        /// Factory method that creates a new city branch instance with a designated manager.
        /// </summary>
        /// <param name="name">The name of the branch. Cannot be null, empty, or whitespace.</param>
        /// <param name="ownerId">The unique identifier of the user who will manage this branch. Cannot be an empty GUID.</param>
        /// <returns>A fully initialized and valid <see cref="Team"/> entity.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item><description>The <paramref name="name"/> is null or consists only of white-space characters.</description></item>
        /// <item><description>The <paramref name="ownerId"/> is equal to <see cref="Guid.Empty"/>.</description></item>
        /// </list>
        /// </exception>
    #endregion
    public static Team Create(string name, Guid ownerId)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Team name cannot be empty");
        
        if(Guid.Empty == ownerId)
            throw new ArgumentException("Owner id cannot be empty");
        
        return new Team(name, ownerId);
    }

    #region AssignCarFleetSummaryRegion
        /// <summary>
        /// Assigns a specific vehicle fleet to this branch, binding the operational cars to the team.
        /// </summary>
        /// <param name="carFleet">The <see cref="CarFleet"/> instance to be assigned.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="carFleet"/> is null.</exception>
    #endregion
    public void AssignCarFleet(CarFleet carFleet)
    {
        CarFleet = carFleet ?? throw new ArgumentNullException(nameof(carFleet));
        CarFleetId = carFleet.Id;
    }
}