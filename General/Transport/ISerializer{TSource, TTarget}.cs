namespace General.Transport;

/// <summary xml:lang = "ru">
/// Описывает сериализатор.
/// </summary>
/// <typeparam name="TSource" xml:lang = "ru">
/// Тип исходной сущности.
/// </typeparam>
/// <typeparam name="TTarget" xml:lang = "ru">
/// Тип получаемой сущности.
/// </typeparam>
public interface ISerializer<TSource, TTarget>
{
    /// <summary xml:lang = "ru">
    /// Сериализует объект из типа <typeparamref name="TSource"/> в <typeparamref name="TTarget"/>.
    /// </summary>
    /// <param name="source" xml:lang = "ru">
    /// Сериализуемый ресурс.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Сериализованный ресурс типа <typeparamref name="TTarget"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException" xml:lang = "ru">
    /// Если <paramref name="source"/> был <see langword="null"/>.
    /// </exception>
    public TTarget Serialize(TSource source);
}
