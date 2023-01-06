using System.Text.Json.Serialization;

namespace DogInfo.WebApi.Dto;

/// <summary>
/// Представляет сообщение содержащее информацию о породах собак.
/// </summary>
[Serializable]
public class BreedsDto
{
    /// <summary>
    /// Содержит словарь, описывающий породы собак.
    /// </summary>
    [JsonPropertyName("message")]
    public Dictionary<string, string[]>? Message { get; set; }

    /// <summary>
    /// Статус сообщения.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
}
