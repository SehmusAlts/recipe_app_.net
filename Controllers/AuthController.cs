using Microsoft.AspNetCore.Mvc;
using RecipeApp.Web.Models.ViewModels;
using RecipeApp.Web.Services;

namespace RecipeApp.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.LoginAsync(model.Email, model.Password);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "Email veya şifre hatalı");
            return View(model);
        }

        TempData["SuccessMessage"] = "Başarıyla giriş yaptınız!";

        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Recipe");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.RegisterAsync(model);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyin.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Kayıt başarılı! Hoş geldiniz!";
        return RedirectToAction("Index", "Recipe");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        TempData["InfoMessage"] = "Başarıyla çıkış yaptınız.";
        return RedirectToAction("Index", "Home");
    }
}
