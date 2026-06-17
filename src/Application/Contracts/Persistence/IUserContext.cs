using Domain.Entities;

namespace Application.Contracts.Persistence;

public interface IUserContext
{
    public Guid? UserId { get; set; }
}