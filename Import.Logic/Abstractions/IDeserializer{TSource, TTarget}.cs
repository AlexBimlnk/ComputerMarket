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
    public TTarget Deserialize(TSource source);
}
