namespace General.Logic.Executables;

/// <summary xml:lang = "ru">
/// Исполняемая сущность.
/// </summary>
/// <typeparam name="TResult" xml:lang = "ru">
/// Тип результата возвращаемого исполняемой сущностью. 
/// </typeparam>
public interface IExecutable<TResult> where TResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Асинхронно вызывает исполнение сущности.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}>"/>.
    /// </returns>
    public Task<TResult> ExecuteAsync();
}
