namespace Market.Models;

/// <summary xml:lang = "ru"
/// Модель поставщика.
/// </summary>
public class Provider
{
    /// <summary xml:lang = "ru">
    /// Идентификатор поставщика.
    /// </summary>
    public int Id { get; set; }

    /// <summary xml:lang = "ru">
    ///  Наименование поставщика.
    /// </summary>
    public string Name { get; set; }

    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public string Inn { get; set; }

    /// <summary xml:lang = "ru">
    /// Заданная маржа поставщика.
    /// </summary>
    public decimal Margin { get; set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; set; }
}
