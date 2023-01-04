namespace DogInfo.WebApi.Services;

/// <summary>
/// Представляет интерфейс для загрузки картинок.
/// </summary>
public interface IDogDownloadImage
{
    /// <summary>
    /// Представляет метод для загрузки изображений.
    /// </summary>
    /// <param name="listBreed">Список содержащий породы собак.</param>
    /// <param name="imageCount">Количество загружаемых изображений.</param>
    /// <returns></returns>
    Task<Dictionary<string, List<string>>> SaveImageAsync(IEnumerable<string> listBreed, int imageCount);
}