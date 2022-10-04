namespace Market.Models;

public class User
{
    public User(int id, string login, string password, UserType type)
    {
        Id = id;
        Login = login;
        Password = password;
        Type = type;
    }

    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public UserType Type { get; set; }

}