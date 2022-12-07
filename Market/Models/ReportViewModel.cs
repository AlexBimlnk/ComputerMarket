using System.ComponentModel.DataAnnotations;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Модель представления связи.
/// </summary>
public sealed class ReportViewModel
{
    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    [Required(ErrorMessage = "Не указан внутренний идентификатор поставщика.")]
    [Range(1, long.MaxValue, ErrorMessage = "Идентификатор не может быть отрицательным.")]
    public long ProviderId { get; set; }

    /// <summary xml:lang = "ru">
    /// Дата начала периода, по которому будет идти отчет.
    /// </summary>
    [Required(ErrorMessage = "Не указано начало периода, по которому будет идти отчет.")]
    public DateTime StartPeriod { get; set; }

    /// <summary xml:lang = "ru">
    /// Дата конца периода, по которому будет идти отчет.
    /// </summary>
    [Required(ErrorMessage = "Не указан конец периода, по которому будет идти отчет.")]
    public DateTime EndPeriod { get; set; }
}
