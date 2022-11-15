namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает команду.
/// </summary>
public interface ICommand
{
    /// <summary xml:lang = "ru">
    /// Асинхронно выполняет команду.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}>"/>.
    /// </returns>
    public Task<ICommandResult> ExecuteAsync();
}