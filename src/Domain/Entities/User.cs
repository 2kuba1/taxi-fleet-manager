using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string Login { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public KilometerRate KilometerRate { get; private  set; }
    public ContractType ContractType { get; private set; }

    public Guid RoleId { get; private set; }
    public Role Role { get; set; }
    
    public Guid? TeamId { get; private set; }
    public Team Team  { get; set; }

    public List<ShiftReport> ShiftReports { get; set; } = new();
    public List<WorkShift> WorkShifts { get; set; } = new();

    protected User(){}

    private User(string email, string login, string phoneNumber, string areaCode, string firstName,
        string lastName, float kilometerRate, ContractType contractType, Guid roleId, Guid teamId)
    {
        Email = email;
        Login = login;
        PhoneNumber = PhoneNumber.Create(phoneNumber, areaCode);
        FirstName = firstName;
        LastName = lastName;
        ContractType = contractType;
        RoleId = roleId;
        TeamId = teamId;
        KilometerRate = KilometerRate.Create(kilometerRate);
    }

    #region UserFactorySummaryRegion
        /// <summary>
        /// Factory method that creates a new instance of a <see cref="User"/> (driver or administrator) ensuring all business invariants are met.
        /// </summary>
        /// <param name="email">The user's email address used for communication and identification. Cannot be empty and must contain the '@' character.</param>
        /// <param name="login">The unique login credential for the user. Cannot be empty and must be between 3 and 8 characters long.</param>
        /// <param name="phoneNumber">The 9-digit phone number of the user.</param>
        /// <param name="areaCode">The country calling code (e.g., "48").</param>
        /// <param name="firstName">The first name of the user. Cannot be empty.</param>
        /// <param name="lastName">The last name of the user. Cannot be empty.</param>
        /// <param name="kilometerRate">The individual financial rate applied per kilometer driven.</param>
        /// <param name="contractType">The type of employment contract (B2B, Mandate, Employment).</param>
        /// <param name="roleId">The identifier of the assigned system role (e.g., Driver, Admin).</param>
        /// <param name="teamId">The identifier of the city branch/team (e.g., Cieszyn, Bielsko). Can be null for global system administrators.</param>
        /// <returns>A fully initialized, business-valid <see cref="User"/> entity.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item><description>The <paramref name="email"/> is null, empty, or does not contain an '@' character.</description></item>
        /// <item><description>The <paramref name="login"/> is null, empty, or its length is outside the 3-8 character range.</description></item>
        /// <item><description>The underlying phone number validation within <see cref="PhoneNumber"/> fails due to an invalid format.</description></item>
        /// </list>
        /// </exception>
    #endregion
    public static User Create(string email, string login, string phoneNumber, string areaCode,
        string firstName,
        string lastName, float kilometerRate, ContractType contractType, Guid roleId, Guid teamId)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Email is invalid");

        if (string.IsNullOrWhiteSpace(login) || login.Length < 3 || login.Length > 8)
            throw new ArgumentException("Login cannot be empty");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty");
        

        return new User(email, login, phoneNumber, areaCode, firstName, lastName, kilometerRate, contractType, roleId, teamId);
    }
}