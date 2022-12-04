namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
/// Группа свойств товара.
/// </summary>
public sealed class PropertyGroup
{
    /// <summary xml:lang = "ru">
    /// Дефолтное значение для группы свойств.
    /// </summary>
    public static PropertyGroup Default => new PropertyGroup();

    private const string NONE_GROUP_NAME = "None";

    /// <summary xml:lang = "ru">
    /// Создаёт экземпляр класса <see cref="PropertyGroup"/>.
    /// </summary>
    /// <param name="id" xml:lang = "ru">Идентификатор группы.</param>
    /// <param name="name" xml:lang = "ru">Название группы.</param>
    /// <exception cref="ArgumentException" xml:lang = "ru">Если <paramref name="name"/> - имеет неправильный формат.</exception>
    public PropertyGroup(ID id, string name)
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(PropertyGroup)} can't be null or white spaces or empty.");

        Name = name;
    }

    private PropertyGroup()
    {
        Id = new ID(-1);
        Name = NONE_GROUP_NAME;
    }

    /// <summary xml:lang = "ru">
    /// Идентифкатор группы.
    /// </summary>
    public ID Id { get; }

    /// <summary xml:lang = "ru">
    /// Название группы.
    /// </summary>
    public string Name { get; }
}
