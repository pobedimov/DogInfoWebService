using System.Text.Json;
using DogInfo.WebApi.Dto;
using Microsoft.Net.Http.Headers;

namespace DogInfo.WebApi.Services;

/// <summary>
/// Представляет клиента для загрузки породы собак.
/// </summary>
public class DogBreedsList : IDogBreedsList
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<DogBreedsList> _logger;

	/// <summary>
	/// Конструктор класса.
	/// </summary>
	/// <param name="httpClient">Клиент.</param>
	/// <param name="logger">Средство логированния.</param>
	public DogBreedsList(HttpClient httpClient, ILogger<DogBreedsList> logger)
	{
		_httpClient = httpClient;
		_httpClient.BaseAddress = new Uri("https://dog.ceo");
		_httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "DogBreeds");
		_logger = logger;
	}

	/// <summary>
	/// Полчение всех пород собак.
	/// </summary>
	/// <returns>Список пород собак.</returns>
	public async Task<List<string>> GetAllBreeds()
	{
		var httpResponse = await _httpClient.GetAsync("/api/breeds/list/all");

        // Выброс исключения, если код не 200-299.
        httpResponse.EnsureSuccessStatusCode();

		if (httpResponse.Content is null && httpResponse?.Content?.Headers?.ContentType?.MediaType != "application/json")
		{
            _logger.LogError("Некорректный формат HTTP ответа, данные не могут быть дессерилазованы.");
            return new List<string>();
        }

		try
		{
			var jsonDataDeserializeObject = await httpResponse.Content.ReadFromJsonAsync<BreedsDto>();

			// Проверка формата JSON файла
			if (jsonDataDeserializeObject is BreedsDto && jsonDataDeserializeObject.Message is not null)
			{
				return new List<string>(jsonDataDeserializeObject.Message.Keys);
			}
			else
			{
				_logger.LogError("Неверный формат JSON данных.");
				return new List<string>();
			}
		}
		catch(Exception ex) when (ex is NotSupportedException || ex is JsonException)
        {
            _logger.LogError("Ошибка дессериализации JSON данных.{error}", ex);
            throw;
		}
    }
}
