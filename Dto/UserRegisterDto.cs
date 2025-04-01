using System.ComponentModel.DataAnnotations;

namespace SimpleFreeBoard.Dto;

public class UserRegisterDto
{
    [EmailAddress(ErrorMessage = "이메일 패턴에 맞게 입력해주세요.")]
    [Required(ErrorMessage = "이메일은 필수 입력입니다.")]
    public string Email { get; set; }

    [MinLength(6, ErrorMessage = "닉네임은 최소 6자 이상입니다.")]
    [Required(ErrorMessage = "닉네임은 필수 입력입니다.")]
    public string Nickname { get; set; }

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{6,}$",
        ErrorMessage = "패스워드는 최소 6자 이상이며, 대문자, 소문자, 특수문자를 포함해야 합니다.")]
    [Required(ErrorMessage = "패스워드는 필수 입력입니다.")]
    public string Password { get; set; }
}