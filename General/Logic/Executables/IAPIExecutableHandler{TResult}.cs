using General.Logic.Executables;

namespace General.Logic.Commands;

/// <summary xml:lang = "ru">
/// Описывает обработчика исполняемых сущностей, получаемых по HTTP.
/// </summary>
/// <typeparam name="TResult" xml:lang = "ru">
/// Тип результата, который будет возвращён сущностью.
/// </typeparam>
public interface IAPIExecutableHandler<TResult> where TResult : IExecutableResult
{
    /// <summary xml:lang = "ru">
    /// Обрабатывает запрос, содержащий исполняемую сущность.
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
