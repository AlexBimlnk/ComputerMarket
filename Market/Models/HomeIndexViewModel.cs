using Market.Logic.Models;

namespace Market.Models;

public class HomeIndexViewModel
{
    public IEnumerable<ItemType> Types { get; set; }

    public string? SearchString { get; set; }

    public UserType LoginUserType { get; set; }
}
