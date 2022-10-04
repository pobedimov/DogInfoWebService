using DogInfoWebService.Infrastructure;
using StackExchange.Redis;

namespace DogInfoWebService.Services;

/// <summary>
/// Представляет класс для работы с БД изображений.
/// </summary>
public class DogImageInfoDb : IDogImageInfoDb
{
    private readonly ILogger<DogImageInfoDb> _logger;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="logger">Средство логгирования.</param>
    public DogImageInfoDb(ILogger<DogImageInfoDb> logger)
    {
        _logger = logger;
    }


    /// <summary>
    /// Сохраняет информацию об изображениях в БД.
    /// </summary>
    /// <param name="dictionaryBreedImages">Словарь с информацией по изображениям и породам.</param>
    /// <exception cref="ArgumentNullException">В качетсве входного параметра передана пустая ссылка.</exception>
    public async Task SaveDogImageInfoDb(Dictionary<string, List<string>> dictionaryBreedImages)
    {
        if (dictionaryBreedImages is null)
        {
            _logger.LogError("В качестве параметра словаря передана пустая ссылка.");
            throw new ArgumentNullException(nameof(dictionaryBreedImages), "В качестве параметра словаря передана пустая ссылка.");
        }

        if (dictionaryBreedImages.Count > 0)
        {
            IDatabase redisStore = RedisStore.Redis;

            foreach (var itemKey in dictionaryBreedImages.Keys)
            {
                if (await redisStore.KeyExistsAsync(itemKey))
                {
                    await redisStore.KeyDeleteAsync(itemKey, CommandFlags.FireAndForget);
                }

                IEnumerable<string> linkList = dictionaryBreedImages[itemKey];
                
                foreach (var itemLink in linkList)
                {
                    await redisStore.ListRightPushAsync(itemKey, itemLink);
                }

                // Вывод в лог информации о загруженных объектах.
                var len = await redisStore.ListLengthAsync(itemKey);
                _logger.LogInformation($"{itemKey} - {len}");
            }
        }
        else 
        {
            _logger.LogError("Отсутствуют данные по изображениям для размещения в БД.");
        }
    }
}
