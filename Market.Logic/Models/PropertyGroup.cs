namespace Market.Logic.Models;

public sealed class PropertyGroup
{
    public static PropertyGroup Default => new PropertyGroup();

    private const string NONE_GROUP_NAME = "None";

    public PropertyGroup(int id, string name)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException("Property group id can't have negative id");

        Id = id;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name of {nameof(PropertyGroup)} can't be null or white spaces or empty.");

        Name = name;
    }

    private PropertyGroup()
    {
        Id = -1;
        Name = NONE_GROUP_NAME;
    }

    public int Id { get; }
    public string Name { get; }
}
