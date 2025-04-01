using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleFreeBoard.Dto;
using SimpleFreeBoard.Services;

namespace SimpleFreeBoard.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;

    public AccountController(ILogger<AccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Register() => View();
    
    [HttpPost]
    public async Task<IActionResult> Register([FromForm] UserRegisterDto dto)
    {
        if (ModelState.IsValid)
        {
            await _accountService.RegisterAsync(dto);
            return RedirectToAction("Login", "Account");
        }
        
        ModelState.AddModelError(string.Empty, "가입양식을 정확히 기입해주세요.");
        return View();
    }
    
    [HttpGet]
    public IActionResult Login() => View("Login");

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] UserLoginDto dto)
    {
        if (ModelState.IsValid && await _accountService.LoginAsync(dto))
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "로그인 실패");
        return View();
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}