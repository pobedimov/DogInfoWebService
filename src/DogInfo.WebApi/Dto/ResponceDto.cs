using System.ComponentModel.DataAnnotations;

namespace DogInfo.WebApi.Dto;

/// <summary>
/// Представляет класс ответа.
/// </summary>
public class ResponceDto
{
    /// <summary>
    /// Представляет команду.
    /// </summary>
    [Required]
    [Display(Name = "status")]
    public string Status { get; set; } = string.Empty;
}
