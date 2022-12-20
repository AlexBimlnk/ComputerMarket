using Market.Logic.Models.Abstractions;

namespace Market.Logic.Models;

/// <summary xml:lang = "ru">
///  Тип товара.
/// </summary>
public sealed record class ItemType : IURLImage
{
    /// <summary>
    /// Создаёт экземпляр типа <see cref="ItemType"/>.
    /// </summary>
    /// <param name="name" xml:lang = "ru">Название типа.</param>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="name"/> - пустое или <see langword="null"/> или содержит только пробелы.
    /// </exception>
    public ItemType(int id, string name, string? urlImage = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(name)} can't be empty or null or contains only white spaces");
        Name = name;

        if (id <= 0)
            throw new ArgumentOutOfRangeException("");

        Id = id;

        URL = urlImage;
    }

    /// <summary xml:lang = "ru">
    /// Название типа.
    /// </summary>
    public string Name { get; }

    /// <summary xml:lang = "ru">
    /// Индетификатор типа продукта.
    /// </summary>
    public int Id { get; }

    /// <inheritdoc/>
    public string? URL { get; }
}
