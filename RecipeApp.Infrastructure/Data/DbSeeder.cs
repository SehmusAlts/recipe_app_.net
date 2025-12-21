using Microsoft.EntityFrameworkCore;
using RecipeApp.Domain.Entities;
using RecipeApp.Domain.Enums;
using RecipeApp.Infrastructure.Data;
using System.Text.Json;

namespace RecipeApp.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        // Demo user oluştur
        var demoUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "demo@recipeapp.com");
        if (demoUser == null)
        {
            demoUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "demo@recipeapp.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test123!"),
                FirstName = "Demo",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(demoUser);
            await context.SaveChangesAsync();
        }

        // Eğer tarifler varsa, seed etme
        if (await context.Recipes.AnyAsync())
        {
            return;
        }

        var recipes = new List<Recipe>
        {
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Mercimek Çorbası",
                Description = "Geleneksel Türk mutfağının vazgeçilmez çorbası.",
                Ingredients = JsonSerializer.Serialize(new[] { "1 su bardağı kırmızı mercimek", "1 adet soğan", "1 adet havuç", "1 yemek kaşığı salça", "1 tatlı kaşığı tuz", "1 tatlı kaşığı karabiber", "Sıvı yağ" }),
                Instructions = "Mercimekleri yıkayın. Soğan ve havucu küp doğrayın. Tencereye yağ koyup sebzeleri kavurun. Salçayı ekleyin. Mercimekleri ve suyu ekleyip pişirin. Blenderdan geçirin.",
                Category = RecipeCategory.AnaYemek,
                PreparationTimeMinutes = 10,
                CookingTimeMinutes = 30,
                Servings = 4,
                ImageUrl = "https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Sütlaç",
                Description = "Fırında pişmiş kremalı pirinç tatlısı.",
                Ingredients = JsonSerializer.Serialize(new[] { "1 su bardağı pirinç", "1 litre süt", "1 su bardağı şeker", "1 paket vanilya", "2 yemek kaşığı nişasta", "Tarçın" }),
                Instructions = "Pirinci haşlayın. Süt, şeker ve nişastayı ekleyip koyulaşana kadar karıştırın. Fırın kaplarına paylaştırıp 180 derecede üzeri kızarana kadar pişirin.",
                Category = RecipeCategory.Tatli,
                PreparationTimeMinutes = 15,
                CookingTimeMinutes = 45,
                Servings = 6,
                ImageUrl = "https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Izgara Köfte",
                Description = "Ev yapımı lezzetli ızgara köfte. Mangalda veya tavada pişirilebilir.",
                Ingredients = JsonSerializer.Serialize(new[] { "500 gr kıyma", "1 adet soğan", "2 diş sarımsak", "1 yumurta", "Maydanoz", "Tuz, karabiber, kimyon" }),
                Instructions = "Tüm malzemeleri yoğurun. Köfte şekli verin. Mangalda veya tavada her iki tarafını da pişirin.",
                Category = RecipeCategory.AnaYemek,
                PreparationTimeMinutes = 20,
                CookingTimeMinutes = 15,
                Servings = 4,
                ImageUrl = "https://images.unsplash.com/photo-1529042410759-befb1204b468?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Menemen",
                Description = "Kahvaltının baş tacı yumurtalı sebze kavurması.",
                Ingredients = JsonSerializer.Serialize(new[] { "3 adet yumurta", "2 adet domates", "1 adet yeşil biber", "1 adet soğan", "Tuz, karabiber", "Zeytinyağı" }),
                Instructions = "Sebzeleri doğrayıp kavurun. Domatesleri ekleyip suyunu çekin. Yumurtaları kırıp karıştırın.",
                Category = RecipeCategory.Kahvalti,
                PreparationTimeMinutes = 5,
                CookingTimeMinutes = 10,
                Servings = 2,
                ImageUrl = "https://images.unsplash.com/photo-1525351326368-efbb5cb6814d?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Çikolatalı Kurabiye",
                Description = "Yumuşacık damla çikolatalı kurabiye.",
                Ingredients = JsonSerializer.Serialize(new[] { "2 su bardağı un", "125 gr tereyağı", "1 su bardağı şeker", "1 yumurta", "1 paket kabartma tozu", "1 paket vanilin", "Damla çikolata" }),
                Instructions = "Tereyağı ve şekeri çırpın. Yumurtayı ekleyin. Kuru malzemeleri karıştırıp ekleyin. Şekil verip 180 derecede 15 dakika pişirin.",
                Category = RecipeCategory.Tatli,
                PreparationTimeMinutes = 15,
                CookingTimeMinutes = 15,
                Servings = 20,
                ImageUrl = "https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Smoothie Bowl",
                Description = "Sağlıklı ve renkli kahvaltı alternatifi.",
                Ingredients = JsonSerializer.Serialize(new[] { "2 adet muz", "1 su bardağı çilek", "1 su bardağı yoğurt", "1 yemek kaşığı bal", "Meyve dilimleri", "Granola", "Chia tohumu" }),
                Instructions = "Muz, çilek, yoğurt ve balı blenderdan geçirin. Kaseye alın. Üzerine meyve, granola ve chia tohumu ekleyin.",
                Category = RecipeCategory.Kahvalti,
                PreparationTimeMinutes = 10,
                CookingTimeMinutes = 0,
                Servings = 2,
                ImageUrl = "https://images.unsplash.com/photo-1590301157890-4810ed352733?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Pizza Margherita",
                Description = "İtalyan mutfağının klasiği basit ama lezzetli pizza.",
                Ingredients = JsonSerializer.Serialize(new[] { "2 su bardağı un", "1 paket instant maya", "1 tatlı kaşığı tuz", "1 su bardağı ılık su", "2 yemek kaşığı zeytinyağı", "Domates sosu", "Mozzarella peyniri", "Fesleğen" }),
                Instructions = "Hamur malzemelerini yoğurun ve 1 saat mayalandırın. İnce açıp fırın tepsisine yayın. Domates sosunu sürün. Mozzarella peynirini ve fesleğeni ekleyin. 220 derecede 15-20 dakika pişirin.",
                Category = RecipeCategory.AnaYemek,
                PreparationTimeMinutes = 90,
                CookingTimeMinutes = 20,
                Servings = 4,
                ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Recipe
            {
                Id = Guid.NewGuid(),
                Name = "Yeşil Çay",
                Description = "Antioksidan deposu sağlıklı içecek.",
                Ingredients = JsonSerializer.Serialize(new[] { "2 tatlı kaşığı yeşil çay", "500 ml su", "Limon dilimi", "Taze nane" }),
                Instructions = "Suyu kaynatın. Yeşil çayı ekleyin ve 3 dakika demlendirin. Limon ve nane ekleyip servis edin.",
                Category = RecipeCategory.Icecek,
                PreparationTimeMinutes = 2,
                CookingTimeMinutes = 3,
                Servings = 2,
                ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400",
                CreatedByUserId = demoUser.Id,
                IsFromExternalApi = false,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Recipes.AddRange(recipes);
        await context.SaveChangesAsync();
    }
}
