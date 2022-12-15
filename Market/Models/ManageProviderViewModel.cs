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

    /// <summary>
    /// ИНН поставщика.
    /// </summary>
    [Required]
    public string INN { get; set; } = null!;

    /// <summary>
    /// Банковский счёт поставщика.
    /// </summary>
    [Required]
    public string BankAccount { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Маржа провайдера.
    /// </summary>
    [Required(ErrorMessage = "Не указана маржа поставщика.")]
    public string Margin { get; set; } = default!;

    /// <summary>
    /// Подтверждён ли провайдер.
    /// </summary>
    public bool IsAproved { get; set; }
}
