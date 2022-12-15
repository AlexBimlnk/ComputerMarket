using Market.Logic.Models;

namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Описывает строителя компьютера.
/// </summary>
public interface IComputerBuilder
{
    /// <summary>
    /// Добавляет компоненты в сборку.
    /// </summary>
    /// <param name="items">
    /// Словарь из компонентов, где ключом выступает <see cref="ItemType"/>,
    /// а значением <see cref="Item"/>.
    /// </param>
    public void AddRange(IReadOnlyDictionary<ItemType, Item> items);

    /// <summary>
    /// Добавляет компонент в сборку или заменяет аналогичный.
    /// </summary>
    /// <param name="item">
    /// Компонент, который нужно добавить в сборку.
    /// </param>
    public void AddOrReplace(Item item);

    /// <summary>
    /// Исключает из сборки компонент с выбранным типом.
    /// </summary>
    /// <param name="itemType">
    /// Тип компонента, который нужно исключить из сборки.
    /// </param>
    public void Remove(ItemType itemType);

    /// <summary>
    /// Собирает компьютер.
    /// </summary>
    /// <returns>
    /// Результат сборки типа <see cref="IComputerBuildResult"/>.
    /// </returns>
    public IComputerBuildResult Build();
}
