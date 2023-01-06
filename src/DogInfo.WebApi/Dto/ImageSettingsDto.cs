namespace DogInfo.WebApi.Dto;

/// <summary>
/// Представляет класс для хранения настроек об изображениях загруженных из конфигурации.
/// </summary>
public class ImageSettingsDto
{
    /// <summary>
    /// Представляет путь к директории для сохранения файлов изображений.
    /// </summary>
    public string PathToImageFiles { get; set; } = Directory.GetCurrentDirectory();
}
