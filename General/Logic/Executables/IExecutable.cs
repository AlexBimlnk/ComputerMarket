namespace General.Logic.Executables;

public interface IExecutable<TResult> where TResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Асинхронно выполняет команду.
    /// </summary>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}>"/>.
    /// </returns>
    public Task<TResult> ExecuteAsync();
}
