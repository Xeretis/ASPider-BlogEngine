using System.Text.Json;
using Application.Services.Types;
using Domain.Data;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementations;

public class ConfigService : IConfigService
{
    private static readonly string ConfigPath =
        Path.Combine(Directory.GetCurrentDirectory(), "Resources", "config.json");

    private readonly ILogger _logger;

    private Config? _config;

    public ConfigService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger("Config");
    }

    public async Task<Config> GetConfigAsync()
    {
        if (_config == null)
        {
            _logger.LogInformation("Loading config from file");

            try
            {
                await using var stream = File.OpenRead(ConfigPath);
                _config = await JsonSerializer.DeserializeAsync<Config>(stream);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, "Failed to load config from file");

                _config = new Config();
                await using var stream = File.Create(ConfigPath);
                await JsonSerializer.SerializeAsync(stream, _config);
            }
        }

        return _config;
    }

    public async Task SetConfigAsync(Config config)
    {
        _config = config;

        await using var stream = File.Create(ConfigPath);
        await JsonSerializer.SerializeAsync(stream, _config);
    }
}