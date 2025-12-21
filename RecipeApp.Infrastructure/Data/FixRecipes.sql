-- Mevcut tarifleri düzelt
USE RecipeAppDb_Dev;
GO

-- Önce mevcut tarifleri sil
DELETE FROM Ratings;
DELETE FROM Favorites;
DELETE FROM Recipes;
GO

-- Demo user ID'sini al
DECLARE @DemoUserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Users WHERE Email = 'demo@recipeapp.com');

-- Tarifleri düzgün Türkçe karakterlerle ekle
INSERT INTO Recipes (Id, Name, Description, Ingredients, Instructions, Category, PreparationTimeMinutes, CookingTimeMinutes, Servings, ImageUrl, CreatedByUserId, IsFromExternalApi, IsDeleted, CreatedAt, UpdatedAt)
VALUES 
-- Mercimek Çorbası (Ana Yemek)
(NEWID(), 
'Mercimek Çorbası', 
'Geleneksel Türk mutfağının vazgeçilmez çorbası.',
'["1 su bardağı kırmızı mercimek","1 adet soğan","1 adet havuç","1 yemek kaşığı salça","1 tatlı kaşığı tuz","1 tatlı kaşığı karabiber","Sıvı yağ"]',
'Mercimekleri yıkayın. Soğan ve havucu küp doğrayın. Tencereye yağ koyup sebzeleri kavurun. Salçayı ekleyin. Mercimekleri ve suyu ekleyip pişirin. Blenderdan geçirin.',
0, 10, 30, 4, 'https://images.unsplash.com/photo-1547592166-23ac45744acd?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Sütlaç (Tatlı)
(NEWID(), 
'Sütlaç', 
'Fırında pişmiş kremalı pirinç tatlısı.',
'["1 su bardağı pirinç","1 litre süt","1 su bardağı şeker","1 paket vanilya","2 yemek kaşığı nişasta","Tarçın"]',
'Pirinci haşlayın. Süt, şeker ve nişastayı ekleyip koyulaşana kadar karıştırın. Fırın kaplarına paylaştırıp 180 derecede üzeri kızarana kadar pişirin.',
1, 15, 45, 6, 'https://images.unsplash.com/photo-1563805042-7684c019e1cb?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Izgara Köfte (Ana Yemek)
(NEWID(), 
'Izgara Köfte', 
'Ev yapımı lezzetli ızgara köfte. Mangalda veya tavada pişirilebilir.',
'["500 gr kıyma","1 adet soğan","2 diş sarımsak","1 yumurta","Maydanoz","Tuz, karabiber, kimyon"]',
'Tüm malzemeleri yoğurun. Köfte şekli verin. Mangalda veya tavada her iki tarafını da pişirin.',
0, 20, 15, 4, 'https://images.unsplash.com/photo-1529042410759-befb1204b468?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Menemen (Kahvaltı)
(NEWID(), 
'Menemen', 
'Kahvaltının baş tacı yumurtalı sebze kavurması.',
'["3 adet yumurta","2 adet domates","1 adet yeşil biber","1 adet soğan","Tuz, karabiber","Zeytinyağı"]',
'Sebzeleri doğrayıp kavurun. Domatesleri ekleyip suyunu çekin. Yumurtaları kırıp karıştırın.',
2, 5, 10, 2, 'https://images.unsplash.com/photo-1525351326368-efbb5cb6814d?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Çikolatalı Kurabiye (Tatlı)
(NEWID(), 
'Çikolatalı Kurabiye', 
'Yumuşacık damla çikolatalı kurabiye.',
'["2 su bardağı un","125 gr tereyağı","1 su bardağı şeker","1 yumurta","1 paket kabartma tozu","1 paket vanilin","Damla çikolata"]',
'Tereyağı ve şekeri çırpın. Yumurtayı ekleyin. Kuru malzemeleri karıştırıp ekleyin. Şekil verip 180 derecede 15 dakika pişirin.',
1, 15, 15, 20, 'https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Smoothie Bowl (Kahvaltı)
(NEWID(), 
'Smoothie Bowl', 
'Sağlıklı ve renkli kahvaltı alternatifi.',
'["2 adet muz","1 su bardağı çilek","1 su bardağı yoğurt","1 yemek kaşığı bal","Meyve dilimleri","Granola","Chia tohumu"]',
'Muz, çilek, yoğurt ve balı blenderdan geçirin. Kaseye alın. Üzerine meyve, granola ve chia tohumu ekleyin.',
2, 10, 0, 2, 'https://images.unsplash.com/photo-1590301157890-4810ed352733?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Pizza Margherita (Ana Yemek)
(NEWID(), 
'Pizza Margherita', 
'İtalyan mutfağının klasiği basit ama lezzetli pizza.',
'["2 su bardağı un","1 paket instant maya","1 tatlı kaşığı tuz","1 su bardağı ılık su","2 yemek kaşığı zeytinyağı","Domates sosu","Mozzarella peyniri","Fesleğen"]',
'Hamur malzemelerini yoğurun ve 1 saat mayalandırın. İnce açıp fırın tepsisine yayın. Domates sosunu sürün. Mozzarella peynirini ve fesleğeni ekleyin. 220 derecede 15-20 dakika pişirin.',
0, 90, 20, 4, 'https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE()),

-- Yeşil Çay (İçecek)
(NEWID(), 
'Yeşil Çay', 
'Antioksidan deposu sağlıklı içecek.',
'["2 tatlı kaşığı yeşil çay","500 ml su","Limon dilimi","Taze nane"]',
'Suyu kaynatın. Yeşil çayı ekleyin ve 3 dakika demlendirin. Limon ve nane ekleyip servis edin.',
3, 2, 3, 2, 'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=400', @DemoUserId, 0, 0, GETDATE(), GETDATE());

GO

SELECT 'Tarifler başarıyla güncellendi!' AS Sonuc;
SELECT Name, Category, LEN(Description) AS DescriptionLength FROM Recipes;
