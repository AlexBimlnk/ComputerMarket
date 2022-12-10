using System.ComponentModel.DataAnnotations;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Модель представления связи.
/// </summary>
public sealed class LinkViewModel
{
    /// <summary xml:lang = "ru">
    /// Внутренний идентификатор продукта.
    /// </summary>
    [Required(ErrorMessage = "Не указан внутренний идентификатор продукта.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long InternalId { get; set; }

    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    [Required(ErrorMessage = "Не указан внутренний идентификатор поставщика.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Внешний идентификатор продукта.
    /// </summary>
    [Required(ErrorMessage = "Не указан внутренний идентификатор продукта.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long ExternalId { get; set; }
}
