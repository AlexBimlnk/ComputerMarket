using Market.Logic.Models;

namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Правило для сборки компьютера.
/// </summary>
public sealed class ComputerBuildRule : IComputerBuildRule
{
    /// <summary>
    /// Создает новый объект типа <see cref="ComputerBuildRule"/>.
    /// </summary>
    /// <param name="description">
    /// Описание правила.
    /// </param>
    /// <param name="forType">
    /// Компонента для которого создается правило. 
    /// </param>
    /// <param name="compareBy">
    /// Компонент к которому применяется правило.
    /// </param>
    /// <param name="func"> Функция, определяющая правило. </param>
    /// <exception cref="ArgumentNullException">
    /// Если любой аргумент оказался <see langword="null"/>
    /// </exception>
    public ComputerBuildRule(
        string description,
        (ID ItemTypeID, ID PropertyID) forType,
        (ID ItemTypeID, ID PropertyID) compareBy,
        Func<ItemProperty, ItemProperty, bool> func)
    {
        CompareFunction = func ?? throw new ArgumentNullException(nameof(func));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException(
                "Description can't be null, empty or has only whitespaces",
                nameof(description));
        Description = description;

        if (forType.ItemTypeID == compareBy.ItemTypeID)
            throw new ArgumentException("Item type id for type and for compare can't be same");

        ForType = forType;
        CompareBy = compareBy;
    }

    /// <inheritdoc/>
    public string Description { get; }

    /// <inheritdoc/>
    public (ID ItemTypeID, ID PropertyID) ForType { get; }

    /// <inheritdoc/>
    public (ID ItemTypeID, ID PropertyID) CompareBy { get; }

    /// <inheritdoc/>
    public Func<ItemProperty, ItemProperty, bool> CompareFunction { get; }
}
