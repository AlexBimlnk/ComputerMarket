using Market.Logic.Models;

namespace Market.Logic.ComputerBuilder;

/// <summary>
/// Сборщик компьютера.
/// </summary>
public sealed class ComputerBuilder : IComputerBuilder
{
    private readonly Dictionary<ItemType, Item> _components = new();
    private readonly IComputerBuildRulesRepository _rulesRepository;
    private readonly Dictionary<ItemType, List<string>> _errors = new();

    /// <summary>
    /// Создает новый объект типа <see cref="ComputerBuilder"/>.
    /// </summary>
    /// <param name="rulesRepository">
    /// Хранилище правил.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Когда <paramref name="rulesRepository"/> оказался <see langword="null"/>.
    /// </exception>
    public ComputerBuilder(IComputerBuildRulesRepository rulesRepository)
    {
        _rulesRepository = rulesRepository ?? throw new ArgumentNullException(nameof(rulesRepository));
    }

    private void ApplyRules(
        Item item,
        IEnumerable<IComputerBuildRule> rules,
        IReadOnlyDictionary<ID, Item> otherItems)
    {
        foreach (var otherItem in otherItems)
        {
            foreach (var rule in rules.Where(x => x.CompareBy.ItemTypeID == otherItem.Key))
            {
                var itemProperty = item.Properties
                    .Single(x => x.Key == rule.ForType.PropertyID);

                var otherItemProperty = otherItem.Value.Properties
                    .Single(x => x.Key == rule.CompareBy.PropertyID);

                if (!rule.CompareFunction(itemProperty, otherItemProperty))
                {
                    if (_errors.ContainsKey(item.Type))
                        _errors[item.Type].Add(rule.Description);
                    else
                        _errors.Add(item.Type, new List<string>() { rule.Description });
                }
            }
        }
    }

    /// <inheritdoc/>
    public void AddOrReplace(Item item) => _components[item.Type] = item;

    /// <inheritdoc/>
    public void AddRange(IReadOnlyDictionary<ItemType, Item> items)
    {
        foreach (var pair in items)
            _components.Add(pair.Key, pair.Value);
    }

    /// <inheritdoc/>
    public void Remove(ItemType itemType) => _components.Remove(itemType);

    /// <inheritdoc/>
    public IComputerBuildResult Build()
    {
        _errors.Clear();

        var rules = _rulesRepository.GetRules();

        foreach (var component in _components)
        {
            var itemTypeId = new ID(component.Key.Id);

            var otherComponents = _components
                .Where(x => x.Key.Id != itemTypeId.Value)
                .ToDictionary(x => new ID(x.Key.Id), x => x.Value);

            var possibleRules = rules
                .Where(x => x.ForType.ItemTypeID == itemTypeId);

            ApplyRules(component.Value, possibleRules, otherComponents);
        }

        return new BuildResult(_errors);
    }

    private class BuildResult : IComputerBuildResult
    {
        public BuildResult(Dictionary<ItemType, List<string>> errors)
        {
            ErrorsByType = errors.ToDictionary(x => x.Key, x => (IReadOnlyCollection<string>)x.Value);
        }

        public bool IsSucces => !ErrorsByType.Any();

        public IReadOnlyDictionary<ItemType, IReadOnlyCollection<string>> ErrorsByType { get; set; }
    }
}
