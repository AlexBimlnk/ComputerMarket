namespace Market.Models;

public class Provider
{
    public Provider(int id, string name, string inn, decimal margin, string bankAccount)
    {
        Id = id;
        Name = name;
        Inn = inn;
        Margin = margin;
        BankAccount = bankAccount;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Inn { get; set; }
    public decimal Margin { get; set; }
    public string BankAccount { get; set; }
}
