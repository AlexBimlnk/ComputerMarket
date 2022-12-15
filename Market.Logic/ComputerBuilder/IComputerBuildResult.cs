using Market.Logic.Models;

namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Описывает результат сборки компьютера.
/// </summary>
public interface IComputerBuildResult
{
    /// <summary>
    /// Флаг определяющий успешно ли завершилась сборка.
    /// </summary>
    public bool IsSucces { get; }

    /// <summary>
    /// Ошибки, привязанные к типу, которые привели к провалу сборки.
    /// </summary>
    public IReadOnlyDictionary<ItemType, IReadOnlyCollection<string>> ErrorsByType { get; }
}
