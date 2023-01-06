using System.Text.Json;
using DogInfo.WebApi.Dto;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace DogInfo.WebApi.Services;

/// <summary>
/// Представляет класс, реализующий загрузку картинок.
/// </summary>
public class DogDownloadImage : IDogDownloadImage
{
    private readonly string PathToImageFiles;
    private readonly HttpClient _httpClient;
    private readonly ILogger<DogBreedsList> _logger;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="options">Данные настроек для картинок загруженные из конфигурации.</param>
    /// <param name="httpClient">Http клиент.</param>
    /// <param name="logger">Средство логгирования.</param>
    public DogDownloadImage(IOptions<ImageSettingsDto> options, HttpClient httpClient, ILogger<DogBreedsList> logger)
    {
        ImageSettingsDto appImageSettings = options.Value;
        PathToImageFiles = appImageSettings.PathToImageFiles;

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://dog.ceo");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "DogBreeds");
        _logger = logger;
    }


    /// <summary>
    /// Получает список URI изображений и запускает загрузку изображений.
    /// </summary>
    /// <param name="listBreed">Список пород собак, изображения которых необходимо загрузить.</param>
    /// <param name="imageCount">Количество изображений для загрузки.</param>
    /// <returns>Коллекция ссылок на изображения по породам собак.</returns>
    /// <exception cref="ArgumentNullException">В качестве входного параметра списка передана пустая ссылка.</exception>
    /// <exception cref="ArgumentOutOfRangeException">В качестве входного параметра списка передана пустая ссылка.</exception>
    public async Task<Dictionary<string, List<string>>> SaveImageAsync(IEnumerable<string> listBreed, int imageCount)
    {
        if (listBreed is null)
        {
            _logger.LogError("В качестве параметра списка передана пустая ссылка.");
            throw new ArgumentNullException(nameof(listBreed), "В качестве параметра списка передана пустая ссылка.");
        }
        if (imageCount <= 0)
        {
            _logger.LogError("Переданно некорректное количество изображений.");
            throw new ArgumentOutOfRangeException(nameof(imageCount), "Переданно некорректное количество изображений.");
        }

        Dictionary<string, List<string>> dictionaryBreedImages = new();

        foreach (var itemBreed in listBreed)
        {
            var httpResponse = await _httpClient.GetAsync($"/api/breed/{itemBreed}/images/random/{imageCount}");
            // Выброс исключения, если код не 200-299.
            httpResponse.EnsureSuccessStatusCode();

            dictionaryBreedImages[itemBreed] = new List<string>();

            if (httpResponse.Content is not null && httpResponse?.Content?.Headers?.ContentType?.MediaType == "application/json")
            {
                try
                {
                    var jsonDataDeserializeObject = await httpResponse.Content.ReadFromJsonAsync<ImagesDto>();

                    // Проверка формата JSON файла
                    if (jsonDataDeserializeObject is ImagesDto && jsonDataDeserializeObject.Message is not null)
                    {
                        foreach (var itemImageUri in jsonDataDeserializeObject.Message)
                        {
                            string fullPath = await DownloadImageAsync(PathToImageFiles, new Uri(itemImageUri));
                            dictionaryBreedImages[itemBreed].Add(new Uri(itemImageUri).GetLeftPart(UriPartial.Path));
                        }
                    }
                    else
                    {
                        _logger.LogError("Неверный формат JSON данных.");
                    }
                }
                catch (Exception ex) when (ex is NotSupportedException || ex is JsonException)
                {
                    _logger.LogError("Ошибка дессериализации JSON данных.{error}", ex);
                    throw;
                }
            }
            else
            {
                _logger.LogError("Некорректный формат HTTP ответа, данные не могут быть дессерилазованы.");
            }
        }

        return dictionaryBreedImages;
    }


    /// <summary>
    /// Скачивает и сохраняет изображение с указанного URI в указанную директорию.
    /// </summary>
    /// <param name="directoryPath">Путь к директории для сохранения изображений.</param>
    /// <param name="uri">Адрес изображения.</param>
    private async Task<string> DownloadImageAsync(string directoryPath, Uri uri)
    {
        if (string.IsNullOrEmpty(directoryPath))
        {
            _logger.LogError("Передан некорректный или пустой путь директории для сохранения изображений.");
            throw new ArgumentNullException(nameof(directoryPath), "Передан некорректный путь директории для сохранения изображений");
        }
        if (uri is null)
        {
            _logger.LogError("Передан некорректный или пустой адрес источника изображения.");
            throw new ArgumentNullException(nameof(uri), "Передан некорректный или пустой адрес источника изображения.");
        }

        try
        {
            using var httpClient = new HttpClient();

            string uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
            string fullFileName = Path.GetFileName(uriWithoutQuery);

            string fullPath = Path.Combine(directoryPath, fullFileName);
            Directory.CreateDirectory(directoryPath);

            var imageBytes = await httpClient.GetByteArrayAsync(uri);
            await File.WriteAllBytesAsync(fullPath, imageBytes);

            return fullPath;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при получении/сохранении изображения {error}", ex);
            throw;
        }
    }
}
