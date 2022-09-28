using Import.Logic.Abstractions.Commands;

namespace Import.Logic.Commands;

/// <summary xml:lang = "ru">
/// Фабрика команд.
/// </summary>
public sealed class CommandFactory : ICommandFactory
{
    private readonly Func<SetLinkCommandParameters, ICommand> _setLinkCommandFactory;

    /// <summary xml:lang = "ru">
    /// Создает новый экземпляр типа <see cref="CommandFactory"/>.
    /// </summary>
    /// <param name="setLinkCommandFactory" xml:lang = "ru">
    /// Делегат, создающий на основе <see cref="CommandID"/> и <see cref="SetLinkCommandParameters"/>
    /// команду типа <see cref="ICommand"/>.
    /// </param>
    public CommandFactory(Func<SetLinkCommandParameters, ICommand> setLinkCommandFactory)
    {
        _setLinkCommandFactory = setLinkCommandFactory;
    }

    /// <inheritdoc/>
    public ICommand Create(CommandParametersBase parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        return parameters switch
        {
            SetLinkCommandParameters setLinkCommandParameters =>
                _setLinkCommandFactory(setLinkCommandParameters),
            _ => throw new ArgumentException(
                $"The command parameters type is unknown {parameters.GetType().Name}",
                nameof(parameters))
        };
    }
}
