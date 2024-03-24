namespace Domain.KonsiCredit;

public class User
{
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }

    public User()
    {
        
    }

    public string Username { get; set; }
    
    public string Password { get; set; }
}