﻿using General.Transport;

namespace Import.Logic.Transport.Configuration;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя внутренних продуктов.
/// </summary>
public sealed class InternalProductSenderConfiguration : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = default!;
}
