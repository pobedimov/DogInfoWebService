namespace DogInfo.WebApi.Settings;

/// <summary>
/// Представляет класс для хранения настроек об изображениях загруженных из конфигурации.
/// </summary>
public class AppImageSettings
{
    /// <summary>
    /// Представляет путь к директории для сохранения файлов изображений.
    /// </summary>
    public string PathToImageFiles { get; set; } = Directory.GetCurrentDirectory();
}
