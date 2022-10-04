using DogInfoWebService.Services;
using DogInfoWebService.Model;
using Microsoft.AspNetCore.Mvc;

namespace DogInfoWebService.Controllers
{
    /// <summary>
    /// Контроллер сервиса загрузки пород собак и изображений.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DogController : ControllerBase
    {
        private readonly IDogBreedsList _dogBreedsList;
        private readonly IDogDownloadImage _dogDownloadImage;
        private readonly IDogImageInfoDb _dogImageInfoDb;
        private readonly ILogger<DogController> _logger;


        /// <summary>
        /// Конструктор класса.
        /// </summary>
        public DogController(IDogBreedsList dogBreedsList, IDogDownloadImage dogDownloadImage, IDogImageInfoDb dogImageInfoDb, ILogger<DogController> logger)
        {
            _dogBreedsList = dogBreedsList;
            _dogDownloadImage = dogDownloadImage;
            _dogImageInfoDb = dogImageInfoDb;
            _logger = logger;
        }

        /// <summary>
        /// Get запрос списка пород.
        /// </summary>
        /// <returns>Список пород собак.</returns>
        [HttpGet("getdogbreed")]
        public async Task<ActionResult> Index()
        {
            return Ok(await _dogBreedsList.GetAllBreeds());
        }

        /// <summary>
        /// Обработка POST запроса с командой.
        /// </summary>
        /// <param name="model">Модель.</param>
        [HttpPost("command")]
        public async Task<ActionResult> DownloadDogsInfoAsync([FromBody] RequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(
                    new ValidationProblemDetails(ModelState));
            }

            try
            {
                // Получение списка пород собак.
                _logger.LogInformation("Загрузка списка пород собак.");
                List<string> breedsList = await _dogBreedsList.GetAllBreeds();

                Dictionary<string, List<string>> dictionaryBreedImages;

                if (breedsList.Count <= 0)
                {
                    _logger.LogError("Пустой список пород собак или ошибка загрузки.");
                    return StatusCode(500, new ResponceModel() { Status = "error" });
                }

                // Сохранение изображений.
                _logger.LogInformation("Загрузка изображений.");
                dictionaryBreedImages = await _dogDownloadImage.SaveImageAsync(breedsList, model.Count);

                if (dictionaryBreedImages.Count <= 0)
                {
                    _logger.LogError("Пустой список загруженных изображений. Не были загружены изображения.");
                    return StatusCode(500, new ResponceModel() { Status = "error" });
                }

                // Сохранение информации об изображениях в БД Redis.
                _logger.LogInformation("Сохранение информации об изображениях в БД Redis");
                await _dogImageInfoDb.SaveDogImageInfoDb(dictionaryBreedImages);

                return Ok(new ResponceModel() { Status = "ok" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Ошибка при выполнении работы сервиса - {error}", ex);
                return StatusCode(500, new ResponceModel() { Status = "error" });
            }
        }
    }
}
