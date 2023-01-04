using DogInfo.WebApi.Services;

namespace DogInfo.WebApi.Tests;

/// <summary>
/// Класс для тестирования сервиса загрузки изображений.
/// </summary>
public class DogDownloadImageTest
{
    /// <summary>
    /// 
    /// </summary>
    [Fact]
    public async Task ThrowExeptionsIfIncorrectInnerParameters()
    {
        IDogDownloadImage DogDownloadImage = new DogDownloadImage(null, null, null);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await DogDownloadImage.SaveImageAsync(null, 8);
        });
    }
}