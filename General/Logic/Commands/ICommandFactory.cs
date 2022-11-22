using General.Logic.Executables;
using General.Logic.Queries;

namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает фабрику команд.
/// </summary>
public interface ICommandFactory : IExecutableFactory<ICommand, CommandParametersBase, ICommandResult>
{
}