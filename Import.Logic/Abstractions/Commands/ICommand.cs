using Import.Logic.Commands;

namespace Import.Logic.Abstractions.Commands;

/// <summary xml:lang = "ru">
/// Описывает команду для создания связей.
/// </summary>
public interface ICommand
{
    /// <summary xml:lang = "ru">
    /// Асинхронно выполняет команду.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}>"/>.
    /// </returns>
    public Task<CommandResult> ExecuteAsync();
}