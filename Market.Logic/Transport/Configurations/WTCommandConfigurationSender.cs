using General.Transport;

namespace Market.Logic.Transport.Configurations;

/// <summary xml:lang = "ru">
/// Конфигурация отправителя в сервис WT.
/// </summary>
public sealed class WTCommandConfigurationSender : ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public string Destination { get; set; } = null!;
}
