using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Logic.Models;

/// <summary>
/// Товар.
/// </summary>
public sealed class Item
{
    /// <summary>
    /// Создает экземпляр типа <see cref="Item"/>.
    /// </summary>
    /// <param name="type">Тип товара.</param>
    /// <param name="name">Название товара.</param>
    /// <param name="properties"></param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="properties"/> или <paramref name="type"/> - <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///  Если <paramref name="name"/> - состоит из пробелов, явялется пустой строкой или <see langword="null"/>.
    /// </exception>
    public Item(ItemType type, string name, IEnumerable<ItemProperty> properties)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(Item)} can't be null or white spaces or empty.");

        Name = name;
        Properties = properties ?? throw new ArgumentNullException(nameof(type));
    }

    /// <summary>
    /// Тип товара.
    /// </summary>
    public ItemType Type { get; }
    
    /// <summary>
    /// Название товара.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Характеристики товара.
    /// </summary>
    public IEnumerable<ItemProperty> Properties { get; }

}
