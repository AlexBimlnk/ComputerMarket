using Import.Logic.Commands;

namespace Import.Logic.Abstractions.Commands;

/// <summary xml:lang = "ru">
/// Описывает фабрику команд.
/// </summary>
public interface ICommandFactory
{
    public ICommand Create(CommandParameters parameters);
}