namespace Market.Logic.Models.Abstractions;

public interface IFilterValue
{
    public string Value { get; }

    public int Count { get; }

    public bool Selected { get; set; }
}
