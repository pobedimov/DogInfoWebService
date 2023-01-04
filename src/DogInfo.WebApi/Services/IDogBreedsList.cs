namespace DogInfo.WebApi.Services;

/// <summary>
/// Интерфейс загрузки списка пород собак.
/// </summary>
public interface IDogBreedsList
{
    /// <summary>
    /// Представляет метод для получения пород собак.
    /// </summary>
    /// <returns>Список пород собак.</returns>
    Task<List<string>> GetAllBreeds();
}