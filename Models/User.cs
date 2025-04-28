namespace SimpleFreeBoard.Models;

public class User
{
    public long Seq { get; set; }
    public string Email { get; set; }
    public string Nickname { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}