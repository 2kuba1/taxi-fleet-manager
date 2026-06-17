using Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class CdnService(IConfiguration configuration) : ICdnService
{
    public Task SaveImageAsync(Stream image, string fileName)
    {
        return Task.CompletedTask;
    }
}