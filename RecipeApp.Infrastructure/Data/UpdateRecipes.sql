USE RecipeAppDb_Dev;
GO

-- Tüm tariflerin Name ve Description alanlarını düzelt
UPDATE Recipes SET 
    Name = 'Mercimek Çorbası',
    Description = 'Geleneksel Türk mutfağının vazgeçilmez çorbası.'
WHERE Name LIKE '%Mercimek%';

UPDATE Recipes SET 
    Name = 'Sütlaç',
    Description = 'Fırında pişmiş kremalı pirinç tatlısı.'
WHERE Name LIKE '%tla%';

UPDATE Recipes SET 
    Name = 'Izgara Köfte',
    Description = 'Ev yapımı lezzetli ızgara köfte. Mangalda veya tavada pişirilebilir.'
WHERE Name LIKE '%zgara%' AND Name LIKE '%fte%';

UPDATE Recipes SET 
    Name = 'Menemen',
    Description = 'Kahvaltının baş tacı yumurtalı sebze kavurması.'
WHERE Name = 'Menemen';

UPDATE Recipes SET 
    Name = 'Çikolatalı Kurabiye',
    Description = 'Yumuşacık damla çikolatalı kurabiye.'
WHERE Name LIKE '%ikolata%';

UPDATE Recipes SET 
    Name = 'Smoothie Bowl',
    Description = 'Sağlıklı ve renkli kahvaltı alternatifi.'
WHERE Name = 'Smoothie Bowl';

UPDATE Recipes SET 
    Name = 'Pizza Margherita',
    Description = 'İtalyan mutfağının klasiği basit ama lezzetli pizza.'
WHERE Name = 'Pizza Margherita';

UPDATE Recipes SET 
    Name = 'Yeşil Çay',
    Description = 'Antioksidan deposu sağlıklı içecek.'
WHERE Name LIKE '%ay%' AND Category = 3;

GO

SELECT 'Güncelleme tamamlandı!' AS Sonuc;
SELECT Name, Description FROM Recipes;
