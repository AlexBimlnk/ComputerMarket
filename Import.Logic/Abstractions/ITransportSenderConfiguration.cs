namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает транспортную конфигурацию отправителя.
/// </summary>
public interface ITransportSenderConfiguration
{
    /// <summary xml:lang = "ru">
    /// Место назначения.
    /// </summary>
    public string Destination { get; }
}
