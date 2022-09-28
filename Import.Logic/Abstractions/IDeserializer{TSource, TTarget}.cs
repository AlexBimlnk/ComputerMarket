namespace Import.Logic.Abstractions;

/// <summary xml:lang = "ru">
/// Описывает десериализатор.
/// </summary>
/// <typeparam name="TSource" xml:lang = "ru">
/// Тип исходной сущности.
/// </typeparam>
/// <typeparam name="TTarget" xml:lang = "ru">
/// Тип получаемой сущности.
/// </typeparam>
public interface IDeserializer<TSource, TTarget>
{
    /// <summary xml:lang = "ru">
    /// Десериализует исходный объект <paramref name="source"/> 
    /// в объект типа <typeparamref name="TTarget"/>.
    /// </summary>
    /// <param name="source" xml:lang = "ru">
    /// Сериализированный ресурс.
    /// </param>
    /// <returns xml:lang = "ru">
    /// Результат десериализации типа <typeparamref name="TTarget"/>.
    /// </returns>
    public TTarget Deserialize(TSource source);
}
