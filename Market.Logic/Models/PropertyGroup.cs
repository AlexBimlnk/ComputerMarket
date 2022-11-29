namespace Market.Logic.Models;

public sealed class PropertyGroup
{
    public static PropertyGroup Default => new PropertyGroup();

    private const string NONE_GROUP_NAME = "None";

    public PropertyGroup(int id, string name)
    {
        Id = id; 
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
