using Import.Logic.Commands;

namespace Import.Logic.Abstractions.Commands;

/// <summary xml:lang = "ru">
/// Описывает обработчика команд, получаемых по HTTP.
/// </summary>
public interface IAPICommandHandler
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
    /// <exception cref="ArgumentException" xml:lang = "ru">
    /// Когда <paramref name="request"/> имел неверный формат.
    /// </exception>
    public Task<CommandResult> HandleAsync(string request);
}
