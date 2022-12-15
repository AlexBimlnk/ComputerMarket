namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Описывает хранилище правил для сборки компьютера.
/// </summary>
public interface IComputerBuildRulesRepository
{
    /// <summary>
    /// Возвращает множество всех правил для сборки компьютера.
    /// </summary>
    /// <returns>
    /// Множество правил типа <see cref="IReadOnlySet{T}"/>.
    /// </returns>
    public IReadOnlySet<IComputerBuildRule> GetRules();
}
