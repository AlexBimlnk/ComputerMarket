using General.Transport;

namespace Market.Logic.Transport.Configurations;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя в сервис импорта.
/// </summary>
public class ImportCommandConfigurationSender : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = null!;
}
