namespace Market.Logic.Storage.Models;
public class OrderItem
{
    public long OrderId { get; set; }

    public long ProviderId { get; set; }

    public long ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal PaidPrice { get; set; }
    
    public bool? ApprovedByProvider { get; set; } 

    public virtual Order Order { get; set; } = default!;

    public virtual Product Product { get; set; } = default!;
}
