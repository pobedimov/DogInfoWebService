﻿using System.Text.Json.Serialization;

namespace DogInfoWebService.Model;

/// <summary>
/// Представляет класс, описывающий изображения собак.
/// </summary>
[Serializable]
public class JsonImages
{    
    /// <summary>
    /// Содержит URI изображений.
    /// </summary>
    [JsonPropertyName("message")]
    public string[]? Message { get; set; }

    /// <summary>
    /// Статус сообщения.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
