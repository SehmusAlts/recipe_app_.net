@echo off
echo ========================================
echo  Starting RecipeApp Frontend
echo ========================================
echo.

cd src\RecipeApp.Web

echo Starting Frontend...
echo Backend API should be running on http://localhost:5243
echo.

dotnet run

pause
