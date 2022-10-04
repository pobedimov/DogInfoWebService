using StackExchange.Redis;

namespace DogInfoWebService.Infrastructure;

/// <summary>
/// Представляет класс для подключения к БД Redis.
/// </summary>
public class RedisStore
{
    /// <summary>
    /// Создаёт инстанс подключения к БД.
    /// </summary>
    public static ConnectionMultiplexer Connection => LazyConnection.Value;

    /// <summary>
    /// Интерфейс для работы с БД.
    /// </summary>
    public static IDatabase Redis => Connection.GetDatabase();


    private static readonly Lazy<ConnectionMultiplexer> LazyConnection;


    static RedisStore()
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        if (string.IsNullOrEmpty(config["RedisConnectionString"]))
        {
            throw new ArgumentException("Не задана строка подключения в файле настроек.");
        }

        var configurationOptions = new ConfigurationOptions
        {
            EndPoints = { config["RedisConnectionString"] }
        };

        LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configurationOptions));
    }
}
