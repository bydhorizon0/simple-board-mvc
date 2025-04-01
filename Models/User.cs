namespace SimpleFreeBoard.Models;

public class User
{
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}