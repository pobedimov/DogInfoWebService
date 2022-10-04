﻿using System.ComponentModel.DataAnnotations;

namespace DogInfoWebService.Model;

/// <summary>
/// Представляет класс модели запроса.
/// </summary>
public class RequestModel
{
    /// <summary>
    /// Представляет команду.
    /// </summary>
    [Required]
    [Display(Name = "command")]
    [RegularExpression(@"^run", ErrorMessage = "Invalid string value.")]
    public string Command { get; set; } = string.Empty;

    /// <summary>
    /// Представляет количество загружаемыз изображений.
    /// </summary>
    [Required]
    [Display(Name = "count")]
    [Range(0,int.MaxValue)]
    public int Count { get; set; }
}