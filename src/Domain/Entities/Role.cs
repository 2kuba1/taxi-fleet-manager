using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; private set; }
    
    protected Role(){}

    private Role(string name)
    {
        Name = name;
    }

    public static Role Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty");

        return new Role(name);
    }
}