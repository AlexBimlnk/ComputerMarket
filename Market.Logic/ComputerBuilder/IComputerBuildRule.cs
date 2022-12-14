using Market.Logic.Models;

namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Описывает правило при сборке компьютера.
/// </summary>
public interface IComputerBuildRule
{
    /// <summary>
    /// Описание правила.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Компонент для которого устанавливается правило.
    /// </summary>
    public (ID ItemTypeID, ID PropertyID) ForType { get; }

    /// <summary>
    /// Компонент к которому применяется правило.
    /// </summary>
    public (ID ItemTypeID, ID PropertyID) CompareBy { get; }

    /// <summary>
    /// Функция, определяющая правило сопоставления.
    /// </summary>
    public Func<ItemProperty, ItemProperty, bool> CompareFunction { get; }
}
