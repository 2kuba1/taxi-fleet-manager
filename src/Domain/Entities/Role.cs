using Domain.Common;

namespace Domain.Entities;

public class Role : BaseEntity
{
    public required string Name { get; set; }
}