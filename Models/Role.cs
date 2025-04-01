using System.ComponentModel;

namespace SimpleFreeBoard.Models;

public enum Role
{
    [Description("ADMIN")]
    ADMIN,
    [Description("MANAGER")]
    MANAGER,
    [Description("USER")]
    USER,
}