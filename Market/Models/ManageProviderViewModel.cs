using System.ComponentModel.DataAnnotations;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Модель представления провайдера для управления политикой.
/// </summary>
public sealed class ManageProviderViewModel
{
    /// <summary xml:lang = "ru">
    /// Идентификатор провайдера.
    /// </summary>
    [Required]
    public long Key { get; set; }

    /// <summary xml:lang = "ru">
    /// Имя провайдера.
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Маржа провайдера.
    /// </summary>
    [Required(ErrorMessage = "Не указана маржа поставщика.")]
    [Range(1.0, 1000, ErrorMessage = "Маржа может быть в диапазоне от 1 до 1000.")]
    public decimal Margin { get; set; }
}
