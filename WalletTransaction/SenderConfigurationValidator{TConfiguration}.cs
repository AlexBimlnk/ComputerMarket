using General.Transport;

using Microsoft.Extensions.Options;

namespace WalletTransaction;

/// <summary xml:lang = "ru">
/// Валидатор конфигурации отправителя.
/// </summary>
/// <typeparam name="TConfiguration" xml:lang = "ru">
/// Тип конфигурации отправителя.
/// </typeparam>
public sealed class SenderConfigurationValidator<TConfiguration> : IValidateOptions<TConfiguration>
    where TConfiguration : class, ITransportSenderConfiguration
{
    /// <inheritdoc/>
    public ValidateOptionsResult Validate(string name, TConfiguration options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("Options can't be null.");

        if (string.IsNullOrWhiteSpace(options.Destination))
            return ValidateOptionsResult.Fail("Destination can't be null, empty or has only whitespaces");

        return ValidateOptionsResult.Success;
    }
}
