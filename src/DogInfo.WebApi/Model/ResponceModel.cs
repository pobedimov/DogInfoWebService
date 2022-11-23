using System.ComponentModel.DataAnnotations;

namespace DogInfoWebService.Model;

/// <summary>
/// Представляет класс ответа.
/// </summary>
public class ResponceModel
{
    /// <summary>
    /// Представляет команду.
    /// </summary>
    [Required]
    [Display(Name = "status")]
    public string Status { get; set; } = string.Empty;
}
