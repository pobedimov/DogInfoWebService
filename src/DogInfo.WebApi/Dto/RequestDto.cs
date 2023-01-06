using System.ComponentModel.DataAnnotations;

namespace DogInfo.WebApi.Dto;

/// <summary>
/// Представляет класс модели запроса.
/// </summary>
public class RequestDto
{
    /// <summary>
    /// Представляет команду.
    /// </summary>
    [Required]
    [Display(Name = "command")]
    [RegularExpression(@"^run", ErrorMessage = "Invalid string value.")]
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// Представляет количество загружаемых изображений.
    /// </summary>
    [Required]
    [Display(Name = "count")]
    [Range(0,int.MaxValue)]
    public int Count { get; set; }
}
