using System.ComponentModel.DataAnnotations;

namespace SimpleFreeBoard.Dto;

public class UserLoginDto
{
    [EmailAddress(ErrorMessage = "이메일 패턴에 맞게 입력해주세요.")]
    [Required(ErrorMessage = "이메일은 필수 입력입니다.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "패스워드는 필수 입력입니다.")]
    public string Password { get; set; }
}