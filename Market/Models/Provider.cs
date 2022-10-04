using System.Text.RegularExpressions;

namespace Market.Models;

/// <summary xml:lang = "ru">
/// Модель поставщика.
/// </summary>
public class Provider
{
    /// <summary>
    /// Создает экземпляр типа <see cref="Provider"/>.
    /// </summary>
    /// <param name="name">Название поставщика.</param>
    /// <param name="inn">Инн поставщика.</param>
    /// <param name="margin">Маржа поставщика.</param>
    /// <param name="bankAccount">Счёт поставщика.</param>
    /// <exception cref="ArgumentNullException">Если любой из параметров равен <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Если инн не соответсвует уставновленному формату или маржа имеет некоректное значение.</exception>
    public Provider(string name, string inn, decimal margin, string bankAccount)
    {
        var innPattern = @"^[0-9]{10}$";

        Name = name ?? throw new ArgumentNullException(nameof(name));

        if (!Regex.IsMatch(inn ?? throw new ArgumentNullException(nameof(inn)), innPattern))
            throw new ArgumentException("Given inn not match with INN format");
        INN = inn;

        if (margin < 1)
            throw new ArgumentException("Given margin has incorrect value");
        Margin = margin;

        BankAccount = bankAccount ?? throw new ArgumentNullException(nameof(bankAccount));
    }

    /// <summary xml:lang = "ru">
    ///  Название поставщика.
    /// </summary>
    public string Name { get; private set; }

    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public string INN { get; private set; }

    /// <summary xml:lang = "ru">
    /// Заданная маржа поставщика.
    /// </summary>
    public decimal Margin { get; private set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; private set; }
}
