using General.Transport;

namespace Market.Logic.Transport.Configurations;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя в сервис импорта.
/// </summary>
public sealed class ImportCommandConfigurationSender : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = null!;
}
