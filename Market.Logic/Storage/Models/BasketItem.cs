namespace Market.Logic.Storage.Models;
public class BasketItem
{
    public long UserId { get; set; }

    public long ProviderId { get; set; }

    public long ItemId { get; set; }

    public int Quantity { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual Provider Provider { get; set; } = default!;

    public virtual Item Item { get; set; } = default!;
}
