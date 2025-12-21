@echo off
echo ========================================
echo  Starting RecipeApp Backend API
echo ========================================
echo.

cd src\RecipeApp.API

echo Checking database...
dotnet ef database update

echo.
echo Starting API on http://localhost:5243
echo.

dotnet run

pause
