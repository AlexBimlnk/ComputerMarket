﻿namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает интерфейс конвертера.
/// </summary>
/// <typeparam name="TSource" xml:lang = "ru">
/// Тип исходной сущности.
/// </typeparam>
/// <typeparam name="TTarget" xml:lang = "ru">
/// Тип сконвертируемой сущности.
/// </typeparam>
public interface IConverter<TSource, TTarget>
{
    /// <summary xml:lang = "ru">
    /// Конвертирует сущность из типа <typeparamref name="TSource"/> в <typeparamref name="TTarget"/>.
    /// </summary>
    /// <param name="source" xml:lang = "ru">
    /// Сущность, которую нужно сконвертировать.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Сконвертированная сущность типа <typeparamref name="TTarget"/>.
    /// </returns>
    public TTarget Convert(TSource source);
}
