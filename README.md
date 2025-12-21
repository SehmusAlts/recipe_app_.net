# Recipe Application - ASP.NET Core Web API

## 📋 Proje Özeti

Bu proje, Ankara Üniversitesi Mühendislik Fakültesi Bilgisayar Mühendisliği Bölümü için geliştirilmiş bir **Yemek Tarifi Web Uygulaması**dır. Proje, **Clean Architecture** ve **SOLID prensiplerine** uygun olarak geliştirilmiştir.

### 🎯 Temel Özellikler

1. **Kullanıcı Yönetimi**
   - Email ve şifre ile kayıt olma
   - JWT tabanlı güvenli giriş sistemi
   - Profil yönetimi

2. **Tarif Yönetimi**
   - DummyJSON API'den tarif listesi
   - Kullanıcıların kendi tariflerini ekleme
   - Kategorilere göre filtreleme
   - Detaylı tarif görüntüleme

3. **Favori Sistemi**
   - Tarifleri favorilere ekleme/çıkarma
   - Favori tarifleri listeleme

4. **Puanlama Sistemi**
   - Tariflere 1-5 arası puan verme
   - Ortalama puan hesaplama
   - Yorum ekleme

## 🏗️ Mimari Yapı

Proje **4 Katmanlı Clean Architecture** ile geliştirilmiştir:

```
RecipeApp/
├── src/
│   ├── RecipeApp.Domain/          # Domain Layer (Entities, Interfaces, Enums)
│   ├── RecipeApp.Application/     # Application Layer (Business Logic, DTOs, Services)
│   ├── RecipeApp.Infrastructure/  # Infrastructure Layer (Data Access, External APIs)
│   └── RecipeApp.API/             # Presentation Layer (Controllers, Middleware)
```

### 📦 Domain Layer
- **Entities**: User, Recipe, Favorite, Rating
- **Enums**: RecipeCategory, RatingValue
- **Interfaces**: IRepository<T>, IUnitOfWork
- **Exceptions**: DomainException, EntityNotFoundException, ValidationException

### 💼 Application Layer
- **DTOs**: Veri transfer nesneleri
- **Services**: Business logic implementasyonları
- **Validators**: FluentValidation ile input validasyonu
- **Mappings**: AutoMapper profilleri

### 🗄️ Infrastructure Layer
- **DbContext**: Entity Framework Core
- **Repositories**: Repository pattern implementasyonu
- **Configurations**: Entity yapılandırmaları
- **External APIs**: DummyJSON entegrasyonu

### 🌐 API Layer
- **Controllers**: RESTful API endpoints
- **Middleware**: Error handling, authentication
- **Authentication**: JWT Bearer token

## 🔧 Teknolojiler

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **MSSQL Server**
- **AutoMapper 15.1**
- **FluentValidation 12.0**
- **BCrypt.Net**
- **System.IdentityModel.Tokens.Jwt**
- **Swagger/OpenAPI**

## ⚙️ Kurulum

### Gereksinimler
- .NET 9 SDK
- SQL Server (LocalDB veya Express)
- Visual Studio 2022 veya VS Code

### Adımlar

1. **Projeyi klonlayın**
```bash
git clone <repository-url>
cd sehmusProje
```

2. **Veritabanı bağlantısını yapılandırın**

`src/RecipeApp.API/appsettings.json` dosyasında:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RecipeAppDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

3. **Migration oluşturun ve veritabanını güncelleyin**
```bash
cd src/RecipeApp.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../RecipeApp.API
dotnet ef database update --startup-project ../RecipeApp.API
```

4. **Projeyi çalıştırın**
```bash
cd ../RecipeApp.API
dotnet run
```

5. **Swagger UI'a erişin**
```
https://localhost:5001/swagger
```

## 📚 API Endpoints

### Authentication
- `POST /api/auth/register` - Yeni kullanıcı kaydı
- `POST /api/auth/login` - Kullanıcı girişi

### Recipes
- `GET /api/recipes` - Tüm tarifleri listele (sayfalama + filtreleme)
- `GET /api/recipes/{id}` - Tarif detayı
- `POST /api/recipes` - Yeni tarif oluştur (auth required)
- `PUT /api/recipes/{id}` - Tarif güncelle (auth required)
- `DELETE /api/recipes/{id}` - Tarif sil (auth required)
- `POST /api/recipes/sync` - External API'den tarifleri senkronize et

### Favorites
- `GET /api/favorites` - Favori tarifleri listele (auth required)
- `POST /api/favorites/{recipeId}` - Favorilere ekle (auth required)
- `DELETE /api/favorites/{recipeId}` - Favorilerden çıkar (auth required)

### Ratings
- `GET /api/ratings/recipe/{recipeId}` - Tarif puanlarını listele
- `POST /api/ratings` - Puan ver (auth required)
- `DELETE /api/ratings/{recipeId}` - Puanı sil (auth required)

## 🔐 Güvenlik

- **JWT Bearer Token** authentication
- **BCrypt** ile şifre hashleme
- **FluentValidation** ile input validasyonu
- **Soft delete** ile veri güvenliği
- **Global query filters** ile silinen verilerin gizlenmesi

## 📐 SOLID Prensipleri

### Single Responsibility Principle (SRP)
- Her sınıf tek bir sorumluluğa sahip
- Örnek: `AuthService` sadece authentication işlemlerinden sorumlu

### Open/Closed Principle (OCP)
- `BaseEntity` sınıfı genişletilebilir
- Yeni exception türleri `DomainException`'dan türetilebilir

### Liskov Substitution Principle (LSP)
- Tüm entity'ler `BaseEntity`'den türer ve yerine kullanılabilir

### Interface Segregation Principle (ISP)
- `IAuthService`, `IRecipeService`, `IRatingService` - odaklanmış interface'ler
- Her servis sadece ihtiyaç duyduğu metotları içerir

### Dependency Inversion Principle (DIP)
- Tüm katmanlar abstraction'lara (interface) bağımlı
- Örnek: Services → IRepository, IUnitOfWork

## 🎨 Design Patterns

- **Repository Pattern**: Veri erişim soyutlaması
- **Unit of Work Pattern**: Transaction yönetimi
- **Dependency Injection**: Loose coupling
- **DTO Pattern**: Veri transfer nesneleri
- **Factory Pattern**: Entity oluşturma

## 📊 Veritabanı Şeması

```sql
Users
├── Id (Guid, PK)
├── Email (string, unique)
├── PasswordHash (string)
├── FirstName (string)
├── LastName (string)
├── CreatedAt (DateTime)
└── UpdatedAt (DateTime?)

Recipes
├── Id (Guid, PK)
├── Name (string)
├── Description (string)
├── Ingredients (string, JSON)
├── Instructions (string)
├── Category (enum)
├── PreparationTimeMinutes (int)
├── CookingTimeMinutes (int)
├── Servings (int)
├── ImageUrl (string?)
├── ExternalApiId (int?)
├── IsFromExternalApi (bool)
├── CreatedByUserId (Guid?, FK)
├── CreatedAt (DateTime)
└── UpdatedAt (DateTime?)

Favorites
├── Id (Guid, PK)
├── UserId (Guid, FK)
├── RecipeId (Guid, FK)
├── AddedAt (DateTime)
└── Unique(UserId, RecipeId)

Ratings
├── Id (Guid, PK)
├── UserId (Guid, FK)
├── RecipeId (Guid, FK)
├── Value (enum: 1-5)
├── Comment (string?)
├── RatedAt (DateTime)
└── Unique(UserId, RecipeId)
```

## 👨‍💻 Geliştirici

**Şehmus Altaş**
- Öğrenci No: 20291316
- Bölüm: Bilgisayar Mühendisliği
- Fakülte: Mühendislik Fakültesi
- Üniversite: Ankara Üniversitesi

## 📝 Ders Bilgileri

- **Ders Adı**: Ağ Tabanlı Teknolojiler ve Uygulamaları
- **Ders Kodu**: BLM4531
- **Tarih**: 06/10/2025

## 🔄 Sonraki Adımlar (Opsiyonel)

- [ ] Unit testler ekleme
- [ ] Docker containerization
- [ ] CI/CD pipeline
- [ ] API rate limiting
- [ ] Caching (Redis)
- [ ] Logging (Serilog)
- [ ] API versioning
- [ ] GraphQL entegrasyonu

## 📄 Lisans

Bu proje eğitim amaçlı geliştirilmiştir.
