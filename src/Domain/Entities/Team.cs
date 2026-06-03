using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities;

public class Team : BaseEntity
{
    public string Name { get; private set; }
    public Guid OwnerId { get; private set; }
    
    public User Owner { get; set; }
    public List<User> Users { get; set; } = new();
    
    protected  Team()
    {
    }

    private Team(string name, Guid ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public static Team Create(string name, Guid ownerId)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Team name cannot be empty");
        
        if(Guid.Empty == ownerId)
            throw new ArgumentException("Owner id cannot be empty");
        
        return new Team(name, ownerId);
    }
}