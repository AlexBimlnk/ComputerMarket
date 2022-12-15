using Market.Logic.ComputerBuilder;

namespace Market.Logic;

/// <summary>
/// Хранилище правил.
/// </summary>
public sealed class RulesRepository : IComputerBuildRulesRepository
{
    private readonly IReadOnlySet<IComputerBuildRule> _computerBuildRules = new HashSet<IComputerBuildRule>()
    {
        new ComputerBuildRule(
            "Сокет материнской платы должен совпадать с сокетом процессора.",
            (ItemTypeID: new(1), PropertyID: new(2)),
            (ItemTypeID: new(4), PropertyID: new(2)),
            (motherBoardSocket, cpuSocket) => motherBoardSocket.Value == cpuSocket.Value)
    };

    /// <inheritdoc/>
    public IReadOnlySet<IComputerBuildRule> GetRules() => _computerBuildRules;
}
