using General.Transport;

namespace Market.Logic.Transport.Senders;

/// <summary xml:lang = "ru">
/// Описывает отпавителя запросов.
/// </summary>
/// <typeparam name="TConfiguration" xml:lang = "ru">
/// Конфигурация отправителя.
/// </typeparam>
/// <typeparam name="TData" xml:lang = "ru">
/// Тип отправляемых данных.
/// </typeparam>
/// <typeparam name="TResult">
/// Тип получаемых данных в ответ.
/// </typeparam>
public interface IQuerySender<TConfiguration, TData, TResult>
    where TConfiguration : ITransportSenderConfiguration
{
    /// <summary xml:lang = "ru">
    /// Отправляет данные.
    /// </summary>
    /// <param name="entity" xml:lang = "ru">
    /// Данные, которые нужно отправить.
    /// </param>
    /// <param name="token" xml:lang = "ru">
    /// Токен отмены операции.
    /// </param>
    /// <returns xml:lang = "ru">
    /// <see cref="Task{TResult}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Когда входные данные оказались <see langword="null"/>.
    /// </exception>
    /// <exception cref="OperationCanceledException" xml:lang = "ru">
    /// Когда операция была отменена.
    /// </exception>
    public Task<TResult> SendAsync(TData entity, CancellationToken token = default);
}
