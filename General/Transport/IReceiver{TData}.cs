namespace General.Transport;

/// <summary xml:lang = "ru">
/// Описывает принимателя данных.
/// </summary>
/// <typeparam name="TData" xml:lang = "ru">
/// Тип данных, которые нужно принять.
/// </typeparam>
public interface IReceiver<TData>
{
    /// <summary xml:lang = "ru">
    /// Принимает данные.
    /// </summary>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}"/>.
    /// </returns>
    /// <exception cref="OperationCanceledException" xml:lang = "ru">
    /// Когда операция была отменена.
    /// </exception>
    public Task<TData> ReceiveAsync(CancellationToken token = default);
}
