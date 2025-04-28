using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MySql.Data.MySqlClient;
using SimpleFreeBoard.Dto;
using SimpleFreeBoard.Models;
using SimpleFreeBoard.Repositories;
using SimpleFreeBoard.Services.Security;

namespace SimpleFreeBoard.Services;

public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AccountRepository _accountRepository;
    private readonly PasswordHasher _passwordHasher;

    public AccountService(IHttpContextAccessor httpContextAccessor, AccountRepository accountRepository,
        PasswordHasher passwordHasher)
    {
        _httpContextAccessor = httpContextAccessor;
        _accountRepository = accountRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterAsync(UserRegisterDto dto)
    {
        try
        {
            await _accountRepository.InsertUserAsync(dto);
        }
        catch (MySqlException ex) when (ex.Number == 1602)
        {
            throw new InvalidOperationException("이미 사용중인 이메일입니다.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("회원가입 중 에러가 발생했습니다.", ex);
        }
    }

    public async Task<bool> LoginAsync(UserLoginDto dto)
    {
        User? dbUser = await _accountRepository.FindUserByEmailAsync(dto.Email);
        if (dbUser == null)
            return false;

        if (!_passwordHasher.VerifyPassword(dbUser.Password, dto.Password))
            return false; 

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, dbUser.Email),
            new Claim(ClaimTypes.Name, dbUser.Nickname),
            new Claim(ClaimTypes.Role, dbUser.Role.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return true;
    }
}