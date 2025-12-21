@echo off
echo ========================================
echo  RecipeApp Database Setup
echo ========================================
echo.
echo This script will:
echo 1. Create the database
echo 2. Apply all migrations
echo 3. Verify the setup
echo.
echo Press any key to continue...
pause > nul
echo.

cd src\RecipeApp.API

echo Step 1: Checking database migrations...
dotnet ef migrations list
echo.

echo Step 2: Applying database migrations...
dotnet ef database update

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo  SUCCESS: Database created successfully!
    echo ========================================
    echo.
    echo Database: RecipeAppDb
    echo Server: (localdb)\mssqllocaldb
    echo.
    echo Tables created:
    echo - Users
    echo - Recipes
    echo - Favorites
    echo - Ratings
    echo.
    echo You can now start the backend API.
    echo.
) else (
    echo.
    echo ========================================
    echo  ERROR: Database setup failed!
    echo ========================================
    echo.
    echo Please check the error message above.
    echo.
)

pause
