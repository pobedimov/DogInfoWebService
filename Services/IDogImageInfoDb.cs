namespace DogInfoWebService.Services;

/// <summary>
/// Представляет интерфейс для сохранения информации об изображениях.
/// </summary>
public interface IDogImageInfoDb
{
    /// <summary>
    /// Сохраняет информацию об изображениях в БД.
    /// </summary>
    Task SaveDogImageInfoDb(Dictionary<string, List<string>> dictionaryBreedImages);
}