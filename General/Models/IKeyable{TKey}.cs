﻿namespace General.Models;

/// <summary xml:lang = "ru">
/// Описывает сущность способную иметь ключ.
/// </summary>
/// <typeparam name="TKey" xml:lang = "ru">
/// Тип ключа сущности.
/// </typeparam>
public interface IKeyable<TKey>
{
    /// <summary xml:lang = "ru">
    /// Ключ сущности.
    /// </summary>
    public TKey Key { get; }
}
