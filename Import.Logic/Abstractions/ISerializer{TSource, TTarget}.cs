namespace Import.Logic.Abstractions;

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
    public TTarget Serialize(TSource source);
}
