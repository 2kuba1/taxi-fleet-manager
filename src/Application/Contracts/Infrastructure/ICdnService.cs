namespace Application.Contracts.Infrastructure;

public interface ICdnService
{
    Task SaveImageAsync(Stream image, string fileName);
}