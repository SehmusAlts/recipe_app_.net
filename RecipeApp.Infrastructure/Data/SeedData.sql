-- Seed Data for RecipeApp
-- Insert sample users and recipes

USE RecipeAppDb_Dev;
GO

-- Insert a default user (password: Test123!)
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'demo@recipeapp.com')
BEGIN
    INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, CreatedAt, UpdatedAt, IsDeleted)
    VALUES (
        NEWID(),
        'demo@recipeapp.com',
        'AQAAAAIAAYagAAAAEJ5qvxKOHh2+8z9QN1YqVZ9W8c9+XF3n7kZz2Y8YhX3Q9vGxJ0K5sW3nP7L8tA==', -- Test123!
        'Demo',
        'User',
        GETDATE(),
        GETDATE(),
        0
    );
END
GO

-- Get the demo user ID
DECLARE @DemoUserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Users WHERE Email = 'demo@recipeapp.com');

-- Insert Recipes
IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Mercimek Çorbası')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Mercimek Çorbası',
        'Geleneksel Türk mutfağının vazgeçilmez çorbası. Sağlıklı ve doyurucu.',
        '["1 su bardağı kırmızı mercimek", "1 adet soğan", "1 yemek kaşığı salça", "2 yemek kaşığı tereyağı", "1 tatlı kaşığı tuz", "1 tatlı kaşığı karabiber", "6 su bardağı su"]',
        'Mercimeği yıkayın ve süzün. Soğanı ince doğrayın. Tencereye tereyağını alın ve soğanları kavurun. Salçayı ekleyip karıştırın. Mercimekleri ve suyu ekleyin. Kaynamaya başlayınca kısık ateşte 30 dakika pişirin. Blenderdan geçirin ve baharatları ekleyin.',
        0, -- Ana Yemek
        10,
        30,
        4,
        'https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Sütlaç')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Sütlaç',
        'Fırında pişmiş geleneksel Türk tatlısı. Hafif ve lezzetli.',
        '["1 litre süt", "1 su bardağı pirinç", "1 su bardağı şeker", "2 yemek kaşığı pirinç unu", "1 paket vanilya"]',
        'Pirinci yıkayın ve suda 30 dakika bekletin. Tencereye sütü alın ve kaynayınca pirinci ekleyin. Kısık ateşte pirinç yumuşayana kadar pişirin. Şeker ve vanilyayı ekleyip 10 dakika daha pişirin. Fırın kaplarına alıp 180 derecede üzeri kızarana kadar pişirin.',
        1, -- Tatli
        15,
        45,
        6,
        'https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Izgara Köfte')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Izgara Köfte',
        'Ev yapımı lezzetli ızgara köfte. Mangalda veya tavada pişirilebilir.',
        '["500 gr kıyma", "1 adet soğan", "2 diş sarımsak", "1 demet maydanoz", "1 tatlı kaşığı kimyon", "1 tatlı kaşığı karabiber", "1 tatlı kaşığı tuz"]',
        'Soğan ve sarımsağı rendeleyin. Maydanozu ince doğrayın. Tüm malzemeleri karıştırıp iyice yoğurun. 30 dakika buzdolabında dinlendirin. Köfte şekli verip ızgarada veya tavada pişirin.',
        0, -- Ana Yemek
        20,
        15,
        4,
        'https://images.unsplash.com/photo-1529692236671-f1f6cf9683ba?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Menemen')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Menemen',
        'Kahvaltının vazgeçilmezi menemen. Pratik ve lezzetli.',
        '["4 adet yumurta", "2 adet domates", "1 adet yeşil biber", "1 adet soğan", "2 yemek kaşığı zeytinyağı", "Tuz, karabiber"]',
        'Soğanı doğrayıp kavurun. Domatesleri küp küp doğrayıp ekleyin. Biberleri ekleyip pişirin. Yumurtaları çırpıp üzerine dökün. Karıştırarak pişirin.',
        2, -- Kahvalti
        10,
        15,
        2,
        'https://images.unsplash.com/photo-1525351484163-7529414344d8?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Çikolatalı Kurabiye')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Çikolatalı Kurabiye',
        'Damla çikolatalı nefis kurabiye. Çay saatlerinin vazgeçilmezi.',
        '["200 gr tereyağı", "1 su bardağı pudra şekeri", "2 su bardağı un", "1 paket vanilya", "1 paket kabartma tozu", "100 gr damla çikolata"]',
        'Tereyağı ve şekeri çırpın. Unu ve kabartma tozunu ekleyip yoğurun. Damla çikolatayı ekleyin. Küçük toplar yapıp fırın tepsisine dizin. 180 derecede 15-20 dakika pişirin.',
        1, -- Tatli
        20,
        20,
        20,
        'https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Smoothie Bowl')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Smoothie Bowl',
        'Sağlıklı ve renkli kahvaltı alternatifi.',
        '["2 adet muz", "1 su bardağı çilek", "1 su bardağı yaban mersini", "1 su bardağı süt", "1 yemek kaşığı bal", "Granola, meyve dilimleri"]',
        'Muz, çilek, yaban mersini ve sütü blenderdan geçirin. Kaseye alın. Üzerine granola, meyve dilimleri ve bal ekleyin.',
        2, -- Kahvalti
        10,
        0,
        2,
        'https://images.unsplash.com/photo-1590301157890-4810ed352733?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Pizza Margherita')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Pizza Margherita',
        'İtalyan mutfağının klasiği basit ama lezzetli pizza.',
        '["2 su bardağı un", "1 paket instant maya", "1 tatlı kaşığı tuz", "1 su bardağı ılık su", "2 yemek kaşığı zeytinyağı", "Domates sosu", "Mozzarella peyniri", "Fesleğen"]',
        'Hamur malzemelerini yoğurun ve 1 saat mayalandırın. İnce açıp fırın tepsisine yayın. Domates sosunu sürün. Mozzarella peynirini rendeleyin ve üzerine serpin. 200 derecede 15-20 dakika pişirin. Üzerine fesleğen yaprakları serpin.',
        0, -- Ana Yemek
        90,
        20,
        4,
        'https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

IF NOT EXISTS (SELECT 1 FROM Recipes WHERE Name = 'Yeşil Çay')
BEGIN
    INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, CreatedAt, UpdatedAt, IsDeleted, IsFromExternalApi)
    VALUES (
        NEWID(),
        'Yeşil Çay',
        'Sağlıklı ve ferahlatıcı yeşil çay demleme rehberi.',
        '["1 tatlı kaşığı yeşil çay", "200 ml su", "İsteğe göre bal veya limon"]',
        'Suyu 80 dereceye kadar ısıtın (kaynamamalı). Yeşil çayı bardağa alın ve üzerine sıcak suyu dökün. 2-3 dakika demleyin. İsteğe göre bal veya limon ekleyin.',
        3, -- Icecek
        2,
        3,
        1,
        'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400',
        @DemoUserId,
        GETDATE(),
        GETDATE(),
        0,
        0
    );
END

PRINT 'Seed data successfully inserted!';
GO
