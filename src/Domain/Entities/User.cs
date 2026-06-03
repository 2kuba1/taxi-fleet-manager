using System;
using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public KilometerRate KilometerRate { get; private  set; }
    public ContractType ContractType { get; private set; }
    
    public Role Role { get; set; }
    public Guid RoleId { get; private set; }
    
    public Team Team  { get; set; }
    public Guid TeamId { get; private set; }

    protected User()
    {
        
    }

    private User(string email, string login, string password, string phoneNumber, string areaCode, string firstName,
        string lastName, float kilometerRate, ContractType contractType, Guid roleId, Guid teamId)
    {
        Email = email;
        Login = login;
        Password = password;
        PhoneNumber = PhoneNumber.Create(phoneNumber, areaCode);
        FirstName = firstName;
        LastName = lastName;
        ContractType = contractType;
        RoleId = roleId;
        TeamId = teamId;
        KilometerRate = KilometerRate.Create(kilometerRate);
    }

    public static User Create(string email, string login, string password, string phoneNumber, string areaCode,
        string firstName,
        string lastName, float kilometerRate, ContractType contractType, Guid roleId, Guid teamId)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Email is invalid");

        if (string.IsNullOrWhiteSpace(login))
            throw new ArgumentException("Login cannot be empty");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty");

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty");
        

        return new User(email, login, password, phoneNumber, areaCode, firstName, lastName, kilometerRate, contractType, roleId, teamId);
    }
}