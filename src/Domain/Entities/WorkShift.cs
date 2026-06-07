using Domain.Common;

namespace Domain.Entities;

public class WorkShift : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public User User { get; set; }
    
    protected WorkShift(){}

    private WorkShift(Guid userId, DateTime startTime, DateTime endTime)
    {
        UserId = userId;
        StartTime = startTime;
        EndTime = endTime;
    }
    
    #region WorhShiftFactorySummary
    /// <summary>
    /// Factory method that creates a new instance of a <see cref="WorkShift"/>, validating user identity and chronological integrity.
    /// </summary>
    /// <param name="userId">The unique identifier of the user assigned to this shift. Cannot be an empty GUID.</param>
    /// <param name="startTime">The date and time when the work shift begins.</param>
    /// <param name="endTime">The date and time when the work shift ends. Must be strictly after the start time.</param>
    /// <returns>A fully initialized and valid <see cref="WorkShift"/> instance.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when:
    /// <list type="bullet">
    /// <item><description>The <paramref name="userId"/> is equal to <see cref="Guid.Empty"/>.</description></item>
    /// <item><description>The <paramref name="startTime"/> is greater than or equal to the <paramref name="endTime"/>.</description></item>
    /// </list>
    /// </exception>
    #endregion
    public static WorkShift Create(Guid userId, DateTime startTime, DateTime endTime)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User id cannot be empty");

        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");
        
        return new WorkShift(userId, startTime, endTime);
    }
}