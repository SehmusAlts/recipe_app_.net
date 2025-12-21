using Microsoft.AspNetCore.Mvc;
using RecipeApp.Web.Models;
using RecipeApp.Web.Models.ViewModels;
using RecipeApp.Web.Services;

namespace RecipeApp.Web.Controllers;

public class RecipeController : Controller
{
    private readonly IRecipeService _recipeService;
    private readonly IAuthService _authService;
    private readonly ILogger<RecipeController> _logger;

    public RecipeController(
        IRecipeService recipeService,
        IAuthService authService,
        ILogger<RecipeController> logger)
    {
        _recipeService = recipeService;
        _authService = authService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, RecipeCategory? category = null)
    {
        await _authService.GetTokenAsync(); // Load token if exists
        var recipes = await _recipeService.GetRecipesAsync(pageNumber, 12, category);

        ViewBag.CurrentCategory = category;
        ViewBag.Categories = Enum.GetValues<RecipeCategory>();

        return View(recipes ?? new PagedResultDto<RecipeDto>());
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        await _authService.GetTokenAsync();
        var recipe = await _recipeService.GetRecipeByIdAsync(id);

        if (recipe == null)
        {
            return NotFound();
        }

        return View(recipe);
    }

    [HttpGet]
    public async Task<IActionResult> Favorites(int pageNumber = 1)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Favorites") });
        }

        var favorites = await _recipeService.GetFavoriteRecipesAsync(pageNumber, 12);
        return View(favorites ?? new PagedResultDto<RecipeDto>());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Create") });
        }

        ViewBag.Categories = Enum.GetValues<RecipeCategory>();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRecipeViewModel model)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return RedirectToAction("Login", "Auth");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = Enum.GetValues<RecipeCategory>();
            return View(model);
        }

        // Token'ı tekrar al ve set et (garantiye al)
        var token = await _authService.GetTokenAsync();
        _logger.LogInformation("Create POST - Token from session: {TokenPreview}", 
            string.IsNullOrEmpty(token) ? "NULL" : $"Bearer {token.Substring(0, Math.Min(20, token.Length))}...");
        
        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Token bulunamadı, kullanıcı giriş yapmamış");
            return RedirectToAction("Login", "Auth");
        }
        
        _logger.LogInformation("Tarif oluşturuluyor, token mevcut");

        var ingredients = model.IngredientsText
            .Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

        var request = new CreateRecipeRequest
        {
            Name = model.Name,
            Description = model.Description,
            Ingredients = ingredients,
            Instructions = model.Instructions,
            Category = (int)model.Category,
            PreparationTimeMinutes = model.PreparationTimeMinutes,
            CookingTimeMinutes = model.CookingTimeMinutes,
            Servings = model.Servings,
            ImageUrl = model.ImageUrl
        };

        var result = await _recipeService.CreateRecipeAsync(request);

        if (result == null)
        {
            ModelState.AddModelError(string.Empty, "Tarif eklenirken bir hata oluştu");
            ViewBag.Categories = Enum.GetValues<RecipeCategory>();
            return View(model);
        }

        TempData["SuccessMessage"] = "Tarif başarıyla eklendi!";
        return RedirectToAction("Details", new { id = result.Id });
    }

    [HttpPost]
    public async Task<IActionResult> AddToFavorites(Guid id)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return Json(new { success = false, message = "Lütfen giriş yapın" });
        }

        // Token'ı API client'a set et
        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return Json(new { success = false, message = "Token bulunamadı" });
        }

        var result = await _recipeService.AddToFavoritesAsync(id);
        return Json(new { success = result, message = result ? "Favorilere eklendi" : "Hata oluştu" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromFavorites(Guid id)
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return Json(new { success = false, message = "Lütfen giriş yapın" });
        }

        // Token'ı API client'a set et
        var token = await _authService.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return Json(new { success = false, message = "Token bulunamadı" });
        }

        var result = await _recipeService.RemoveFromFavoritesAsync(id);
        return Json(new { success = result, message = result ? "Favorilerden çıkarıldı" : "Hata oluştu" });
    }

    [HttpPost]
    public async Task<IActionResult> SyncExternalRecipes()
    {
        if (!await _authService.IsAuthenticatedAsync())
        {
            return RedirectToAction("Login", "Auth");
        }

        var result = await _recipeService.SyncExternalRecipesAsync();

        if (result)
        {
            TempData["SuccessMessage"] = "Dış tarifler başarıyla yüklendi!";
        }
        else
        {
            TempData["ErrorMessage"] = "Dış tarifler yüklenirken hata oluştu";
        }

        return RedirectToAction("Index");
    }
}
