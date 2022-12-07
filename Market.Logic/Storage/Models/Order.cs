namespace Market.Logic.Storage.Models;
public class Order
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int StateId { get; set; }

    public DateTime Date { get; set; }

    public virtual User User { get; set; } = default!;

    public virtual ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
}
