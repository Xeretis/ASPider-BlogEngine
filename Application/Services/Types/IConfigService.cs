using Domain.Data;

namespace Application.Services.Types;

public interface IConfigService
{
    Task<Config> GetConfigAsync();
    Task SetConfigAsync(Config config);
}