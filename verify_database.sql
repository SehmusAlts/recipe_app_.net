-- Verify database and tables
USE RecipeAppDb_Dev;
GO

-- Check all tables
SELECT
    TABLE_NAME,
    TABLE_TYPE
FROM
    INFORMATION_SCHEMA.TABLES
WHERE
    TABLE_TYPE = 'BASE TABLE'
ORDER BY
    TABLE_NAME;
GO

-- Count records in each table
SELECT 'Users' AS TableName, COUNT(*) AS RecordCount FROM Users
UNION ALL
SELECT 'Recipes', COUNT(*) FROM Recipes
UNION ALL
SELECT 'Favorites', COUNT(*) FROM Favorites
UNION ALL
SELECT 'Ratings', COUNT(*) FROM Ratings;
GO
