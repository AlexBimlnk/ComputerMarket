using General.Logic.Executables;

namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает обработчика команд, получаемых по HTTP.
/// </summary>
public interface IAPIExecutableHandler<TResult> where TResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Обрабатывает запрос, содержащий команду.
    /// </summary>
    /// <param name="request" xml:lang = "ru">
    /// Запрос.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}"/>.
    /// </returns>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="request"/> имел неверный формат.
    /// </exception>
    /// <exception cref="OperationCanceledException" xml:lang = "ru">
    /// Если операция была отменена.
    /// </exception>
    public Task<TResult> HandleAsync(string request, CancellationToken token = default);
}
