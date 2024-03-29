﻿namespace Market.Logic.Storage.Models;

/// <summary xml:lang = "ru">
/// Транспортная модель пользователя, используемая хранилищем.
/// </summary>
public class User
{
    /// <summary xml:lang = "ru">
    /// Индетифкатор пользователя.
    /// </summary>
    public long Id { get; set; }

    /// <summary xml:lang = "ru">
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Адрес электронной почты пользователя.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Индетифкатор типа пользователя.
    /// </summary>
    public short UserTypeId { get; set; }

    /// <summary xml:lang = "ru">
    /// Тип пользователя.
    /// </summary>
    public virtual UserType UserType { get; set; } = null!;

    /// <summary xml:lang = "ru">
    /// Поставщик к которому относится пользователь.
    /// </summary>
    public virtual ProviderAgent? ProvidersAgent { get; set; }
}
