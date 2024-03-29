﻿namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель поставщика, используемая хранилищем.
/// </summary>
public class Provider
{
    /// <summary xml:lang = "ru">
    /// Индетификатор поставщика.
    /// </summary>
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Название поставщика.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Маржа закрепленная за поставщиком.
    /// </summary>
    public decimal Margin { get; set; }

    /// <summary xml:lang = "ru">
    /// Банковский счёт поставщика.
    /// </summary>
    public string BankAccount { get; set; } = null!;

    /// <summary xml:lang="ru">
    /// Подтвержден ли провайдер.
    /// </summary>
    public bool IsAproved { get; set; } = false;

    /// <summary xml:lang = "ru">
    /// Инн поставщика.
    /// </summary>
    public string Inn { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Продукты предоставляемые поставщиком.
    /// </summary>
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

    /// <summary xml:lang = "ru">
    /// Представители поставщика.
    /// </summary>
    public virtual ICollection<ProviderAgent> ProvidersAgents { get; set; } = new HashSet<ProviderAgent>();
}
