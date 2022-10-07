﻿using Import.Logic.Abstractions;

namespace Import.Logic.Transport.Configuration;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя внутренних продуктов.
/// </summary>
public class InternalProductSenderConfiguration : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = default!;
}
