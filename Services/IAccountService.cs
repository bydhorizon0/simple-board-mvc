using SimpleFreeBoard.Dto;

namespace SimpleFreeBoard.Services;

public interface IAccountService
{
    public Task RegisterAsync(UserRegisterDto dto);
    public Task<bool> LoginAsync(UserLoginDto dto);
}