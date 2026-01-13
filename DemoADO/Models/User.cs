namespace DemoADO.Models;

public class User
{
    public User(int id, string email, string? lastname, string? firstname)
    {
        Id = id;
        Email = email;
        Lastname = lastname;
        Firstname = firstname;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string? Lastname { get; set; }
    public string? Firstname { get; set; }

    public override string? ToString()
    {
        return $"Id: {Id}, email: {Email}, lastname: {Lastname}, firstname: {Firstname}";
    }
}
