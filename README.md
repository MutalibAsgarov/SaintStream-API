# Saint Stream API

Saint Stream API --- film və serial platforması üçün hazırlanmış ASP.NET
Core Web API layihəsidir. Layihə istifadəçilərin qeydiyyatdan keçməsini,
sistemə daxil olmasını, autentifikasiyasını və şəxsi Watchlist
siyahılarını idarə etməsini təmin edir.

Layihə RESTful API prinsiplərinə uyğun hazırlanıb. Backend və frontend
arasında HTTP vasitəsilə məlumat mübadiləsi aparılır. Database
əməliyyatları Entity Framework Core vasitəsilə idarə olunur.

------------------------------------------------------------------------

## Table of Contents

-   [Project Overview](#project-overview)
-   [Technologies](#technologies)
-   [Project Structure](#project-structure)
-   [Main Features](#main-features)
-   [Authentication System](#authentication-system)
-   [Watchlist System](#watchlist-system)
-   [Database Structure](#database-structure)
-   [API Endpoints](#api-endpoints)
-   [HTTP Status Codes](#http-status-codes)
-   [Installation and Setup](#installation-and-setup)
-   [Configuration](#configuration)
-   [Running the Project](#running-the-project)
-   [Testing](#testing)
-   [Security](#security)
-   [Developer](#developer)

------------------------------------------------------------------------

## Project Overview

Saint Stream API-nin əsas məqsədi film və serial platformasının backend
funksionallığını təmin etməkdir.

Layihənin əsas modulları:

1.  **Authentication:** İstifadəçilərin qeydiyyatı, login prosesi, JWT
    Access Token və Refresh Token mexanizmi.
2.  **Watchlist:** İstifadəçilərin film və serialları şəxsi siyahılarına
    əlavə etməsi, silməsi və siyahılarını əldə etməsi.
3.  **Home:** Platformanın əsas səhifəsi üçün məzmun məlumatlarının əldə
    edilməsi.

Hər istifadəçinin öz hesabına məxsus Watchlist siyahısı mövcuddur.
İstifadəçilər digər istifadəçilərin siyahılarına müdaxilə edə bilməzlər.

## Technologies

  Technology                             Purpose
  -------------------------------------- ------------------------------------------
  C#                                     Backend proqramlaşdırma dili
  ASP.NET Core Web API                   API və HTTP endpoint-lərinin yaradılması
  Entity Framework Core                  ORM və database əməliyyatları
  SQL Server                             Məlumatların saxlanılması
  JWT Bearer Authentication              İstifadəçi autentifikasiyası
  ASP.NET Core Identity PasswordHasher   Şifrələrin hash edilməsi
  Swagger / OpenAPI                      API sənədləşdirilməsi və test
  LINQ                                   Database sorğularının hazırlanması
  Dependency Injection                   Servislərin idarə olunması

## Project Structure

Layihə controller, service, DTO, entity və data hissələrinə ayrılıb.

``` text
Saint_Stream_API/
├── Controllers/
│   ├── AuthController.cs
│   ├── HomeController.cs
│   └── WatchlistController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   ├── Auth/
│   │   ├── RegisterDto.cs
│   │   ├── LoginDto.cs
│   │   ├── AuthResponseDto.cs
│   │   └── RefreshTokenDto.cs
│   └── Watchlist/
│       ├── AddToWatchlistDto.cs
│       └── WatchlistDto.cs
├── Entities/
│   ├── User.cs
│   ├── Content.cs
│   ├── Genre.cs
│   ├── ContentGenre.cs
│   ├── Collection.cs
│   ├── CollectionItem.cs
│   ├── WatchlistItem.cs
│   ├── RefreshToken.cs
│   ├── PasswordResetToken.cs
│   └── Platform.cs
├── Services/
│   ├── Interfaces/
│   ├── AuthService.cs
│   ├── HomeService.cs
│   └── WatchlistService.cs
├── Migrations/
├── Properties/
├── appsettings.json
├── Program.cs
└── Saint_Stream_API.csproj
```

*Qeyd: Bu struktur ümumi nümunədir. Faktiki qovluq və fayl adları
layihədəki strukturla uyğunlaşdırılmalıdır.*

## Main Features

### Authentication

-   İstifadəçi qeydiyyatı (Register)
-   İstifadəçi girişi (Login)
-   Password hashing
-   JWT Access Token yaradılması
-   Refresh Token yaradılması və yoxlanılması
-   Refresh Token Rotation
-   Logout zamanı Refresh Token-in ləğv edilməsi
-   Cari istifadəçi məlumatlarının əldə edilməsi

### Watchlist

-   Film və serialları Watchlist-ə əlavə etmək
-   Watchlist-dən silmək
-   İstifadəçinin öz Watchlist siyahısını əldə etmək
-   Eyni məzmunun təkrar əlavə edilməsinin qarşısını almaq
-   İstifadəçilərin Watchlist məlumatlarını bir-birindən ayırmaq
-   Siyahını əlavə edilmə tarixinə görə azalan ardıcıllıqla sıralamaq

## Authentication System

Layihədə JWT əsaslı authentication sistemi istifadə olunur.

### Registration

İstifadəçi username, email və password məlumatları ilə qeydiyyatdan
keçir.

Qeydiyyat zamanı: - Username və email yoxlanılır. - Eyni email və
username ilə təkrar qeydiyyatın qarşısı alınır. - Password hash edilərək
database-də saxlanılır. - Yeni istifadəçi database-ə əlavə edilir.

Password açıq formada database-də saxlanılmır.

### Login

İstifadəçi email və password məlumatlarını göndərərək sistemə daxil
olur.

Login prosesi: 1. Email vasitəsilə istifadəçi tapılır. 2. Password hash
yoxlanılır. 3. İstifadəçinin aktiv olub-olmadığı yoxlanılır. 4. Access
Token yaradılır. 5. Refresh Token yaradılaraq database-də saxlanılır. 6.
Hər iki token client-ə qaytarılır.

### JWT Access Token

Access Token qorunan endpoint-lərə müraciət zamanı istifadə olunur.
Token daxilində istifadəçinin ID-si və username kimi claim məlumatları
yerləşdirilir.

Token-in imzası, issuer-i, audience-i və etibarlılıq müddəti server
tərəfindən yoxlanılır.

### Refresh Token Rotation

Refresh Token Access Token-in müddəti bitdikdən sonra yeni token əldə
etməyə imkan verir.

Refresh prosesi zamanı: 1. Client mövcud Refresh Token-i göndərir. 2.
Backend token-in mövcudluğunu yoxlayır. 3. Token-in müddəti və
istifadəçinin aktivliyi yoxlanılır. 4. Köhnə Refresh Token ləğv edilir.
5. Yeni Access Token və Refresh Token yaradılır. 6. Yeni token-lər
client-ə qaytarılır.

### Logout

Logout zamanı client-in göndərdiyi Refresh Token database-dən silinir.
Beləliklə, həmin Refresh Token vasitəsilə yeni Access Token əldə etmək
mümkün olmur.

## Watchlist System

Watchlist istifadəçilərə bəyəndikləri film və serialları şəxsi
siyahılarında saxlamağa imkan verir. Bütün Watchlist endpoint-ləri JWT
Authentication ilə qorunur.

### Add to Watchlist

İstifadəçi film və ya serialı Watchlist-ə əlavə edə bilər.

Əməliyyat zamanı: - Content-in mövcudluğu yoxlanılır. - Eyni content-in
istifadəçinin siyahısında olub-olmadığı yoxlanılır. - Content
istifadəçinin Watchlist-inə əlavə edilir. - Əlavə edilmə tarixi qeyd
olunur.

Eyni istifadəçi eyni content-i ikinci dəfə əlavə edə bilməz.

### Remove from Watchlist

İstifadəçi öz siyahısındakı filmi və ya serialı silə bilər. Silinmə
zamanı həm `user_id`, həm də `content_id` yoxlanılır. Buna görə
istifadəçi başqa hesabın Watchlist qeydini silə bilməz.

### Get Watchlist

İstifadəçinin Watchlist məlumatları əldə edilir. Response-a content ID,
title, poster URL, type, rating, age rating, genres və added date
məlumatları daxil edilir.

Siyahı ən son əlavə edilən content-dən başlayaraq sıralanır. Watchlist
boş olduqda boş massiv qaytarılır.

## Database Structure

Layihədə SQL Server və Entity Framework Core istifadə olunur.

  Table                     Description
  ------------------------- ------------------------------------
  `users`                   İstifadəçi məlumatları
  `contents`                Film və serial məlumatları
  `genres`                  Janr məlumatları
  `content_genres`          Content və Genre əlaqəsi
  `collections`             Collection məlumatları
  `collection_items`        Collection daxilindəki content-lər
  `watchlist_items`         İstifadəçilərin Watchlist qeydləri
  `refresh_tokens`          Refresh Token məlumatları
  `password_reset_tokens`   Password reset token məlumatları
  `platforms`               Platforma məlumatları

### Watchlist Constraints

`watchlist_items` cədvəlində: - `user_id` sütunu `users.id` ilə
əlaqəlidir. - `content_id` sütunu `contents.id` ilə əlaqəlidir. -
`user_id` və `content_id` üzərində composite UNIQUE index mövcuddur.

Bu UNIQUE index eyni istifadəçinin eyni content-i təkrar əlavə etməsinin
qarşısını database səviyyəsində də alır. Fərqli istifadəçilər isə eyni
content-i öz siyahılarına əlavə edə bilərlər.

## API Endpoints

### Authentication

  ---------------------------------------------------------------------------------
  Method            Endpoint                    Description       Authentication
  ----------------- --------------------------- ----------------- -----------------
  POST              `/api/auth/register`        Yeni istifadəçi   Not required
                                                qeydiyyatı        

  POST              `/api/auth/login`           İstifadəçi girişi Not required

  POST              `/api/auth/refresh-token`   Token yenilənməsi Not required

  POST              `/api/auth/logout`          Refresh Token-in  Not required
                                                ləğvi             

  GET               `/api/auth/me`              Cari istifadəçi   Required
                                                məlumatları       
  ---------------------------------------------------------------------------------

#### Register

**POST** `/api/auth/register`

Request body:

``` json
{
  "username": "example",
  "email": "example@gmail.com",
  "password": "Password123!"
}
```

Uğurlu qeydiyyat zamanı layihənin hazırkı controller implementasiyasına
əsasən `200 OK` qaytarılır.

#### Login

**POST** `/api/auth/login`

Request body:

``` json
{
  "email": "example@gmail.com",
  "password": "Password123!"
}
```

Response nümunəsi:

``` json
{
  "token": "access_token",
  "refreshToken": "refresh_token"
}
```

#### Refresh Token

**POST** `/api/auth/refresh-token`

Request body:

``` json
{
  "refreshToken": "refresh_token"
}
```

Uğurlu əməliyyat zamanı yeni Access Token və Refresh Token qaytarılır.

#### Logout

**POST** `/api/auth/logout`

Request body:

``` json
{
  "refreshToken": "refresh_token"
}
```

Uğurlu əməliyyat zamanı:

``` json
{
  "message": "Logout successful."
}
```

#### Current User

**GET** `/api/auth/me`

Authorization tələb olunur. Cari istifadəçinin ID və username
məlumatları qaytarılır.

### Watchlist

Bütün Watchlist endpoint-ləri Access Token tələb edir.

Authorization header:

``` http
Authorization: Bearer <access_token>
```

#### Add to Watchlist

**POST** `/api/watchlist`

Request body:

``` json
{
  "contentId": 1
}
```

Success response --- `201 Created`:

``` json
{
  "id": 1,
  "content_id": 1,
  "added_at": "2026-11-21T12:00:00Z"
}
```

Mümkün xətalar:

  Status   Description
  -------- -------------------------------------------
  400      Yanlış və ya boş `contentId`
  401      Token yoxdur, yanlışdır və ya vaxtı bitib
  404      Content not found
  409      Already in watchlist

#### Remove from Watchlist

**DELETE** `/api/watchlist/{contentId}`

Nümunə:

``` http
DELETE /api/watchlist/1
```

Success response --- `200 OK`:

``` json
{
  "message": "Removed from watchlist"
}
```

Mümkün xətalar:

  Status   Description
  -------- -------------------------------------------
  400      Yanlış `contentId`
  401      Token yoxdur, yanlışdır və ya vaxtı bitib
  404      Not in watchlist

#### Get Watchlist

**GET** `/api/watchlist`

Success response --- `200 OK`:

``` json
[
  {
    "id": 1,
    "title": "Example Movie",
    "posterUrl": "poster-url",
    "type": "movie",
    "rating": 8.5,
    "ageRating": "PG-13",
    "genres": [
      "Action",
      "Adventure"
    ],
    "addedAt": "2026-11-21T12:00:00Z"
  }
]
```

Watchlist boş olduqda:

``` json
[]
```

*Qeyd: JSON nümunələri izahlı nümunələrdir. Faktiki response property
adları DTO-ların serialization konfiqurasiyasından asılı ola bilər.*

## HTTP Status Codes

  -----------------------------------------------------------------------
  Status Code                         Meaning
  ----------------------------------- -----------------------------------
  200                                 OK --- Əməliyyat uğurludur

  201                                 Created --- Yeni qeyd yaradılıb

  400                                 Bad Request --- Yanlış request

  401                                 Unauthorized --- Authentication
                                      tələb olunur və ya token
                                      etibarsızdır

  404                                 Not Found --- Məlumat tapılmadı

  409                                 Conflict --- Təkrar və ya
                                      ziddiyyətli əməliyyat

  500                                 Internal Server Error --- Server
                                      xətası
  -----------------------------------------------------------------------

## Installation and Setup

### Prerequisites

-   .NET SDK
-   SQL Server
-   SQL Server Management Studio (SSMS)
-   Visual Studio və ya Visual Studio Code

### 1. Clone Repository

``` bash
git clone <repository-url>
```

### 2. Navigate to Project Directory

``` bash
cd Saint_Stream_API
```

### 3. Configure Database Connection

`appsettings.json` və ya uyğun təhlükəsiz konfiqurasiya mənbəyində SQL
Server connection string təyin edilməlidir.

Nümunə:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SaintStreamDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

`YOUR_SERVER` lokal SQL Server instance adı ilə əvəz edilməlidir.

### 4. Configure JWT

JWT parametrləri aşağıdakı formada konfiqurasiya olunur:

``` json
{
  "Jwt": {
    "Key": "YOUR_SECRET_KEY",
    "Issuer": "SaintStream",
    "Audience": "SaintStreamUsers",
    "ExpiryMinutes": 60
  }
}
```

`Key` üçün güclü və təsadüfi secret istifadə edilməlidir. Real secret
key və connection string GitHub-a commit edilməməlidir. Production
mühitində User Secrets və ya environment variables istifadə etmək
tövsiyə olunur.

### 5. Apply Database Migrations

Visual Studio Package Manager Console:

``` powershell
Update-Database
```

Alternativ olaraq .NET CLI:

``` bash
dotnet ef database update
```

### 6. Run the Application

``` bash
dotnet run
```

Tətbiq başladıqdan sonra terminalda göstərilən URL vasitəsilə API-yə
müraciət etmək mümkündür.

## Testing

API endpoint-ləri Swagger və ya Postman vasitəsilə test edilə bilər.

Tövsiyə olunan test ardıcıllığı:

1.  Register endpoint-i ilə yeni istifadəçi yaradın.
2.  Login endpoint-i ilə Access Token əldə edin.
3.  Swagger Authorize bölməsində və ya Postman Authorization hissəsində
    Bearer Token əlavə edin.
4.  Watchlist endpoint-lərini test edin.
5.  Refresh Token endpoint-i ilə token yenilənməsini yoxlayın.
6.  Köhnə Refresh Token-in yenidən istifadə edilə bilmədiyini yoxlayın.
7.  Logout endpoint-i ilə Refresh Token-i ləğv edin.
8.  `/api/auth/me` endpoint-i ilə cari istifadəçi məlumatlarını
    yoxlayın.

### Important Test Scenarios

-   Token olmadan qorunan endpoint-lərə müraciət.
-   Yanlış və vaxtı bitmiş token ilə müraciət.
-   `contentId` olmadan Watchlist-ə əlavə etmə.
-   Mövcud olmayan content-in əlavə edilməsi.
-   Eyni content-in təkrar əlavə edilməsi.
-   Watchlist-də olmayan content-in silinməsi.
-   İki fərqli istifadəçi ilə Watchlist məlumatlarının ayrılığının
    yoxlanılması.
-   Refresh Token Rotation və Logout yoxlamaları.
-   Boş Watchlist response-un yoxlanılması.

## Security

Layihədə aşağıdakı təhlükəsizlik mexanizmlərindən istifadə olunur:

-   JWT Bearer Authentication
-   Password Hashing
-   Access Token Lifetime Validation
-   JWT Issuer və Audience Validation
-   JWT Signing Key Validation
-   Refresh Token Validation
-   Refresh Token Rotation
-   Refresh Token Revocation
-   Protected API Endpoints
-   User-specific Watchlist Access
-   Database-level Unique Constraints

Watchlist əməliyyatlarında istifadəçinin ID-si client-dən qəbul edilmir.
ID etibarlı JWT token-in claim məlumatlarından əldə edilir. Bu yanaşma
istifadəçilərin bir-birinin şəxsi Watchlist məlumatlarına müdaxilə
etməsinin qarşısını alır.

## Developer

**Mütalib Əsgərov**

Computer Engineering Student

-   GitHub: [MutalibAsgarov](https://github.com/MutalibAsgarov)
-   LinkedIn: [Mütalib
    Əsgərov](https://www.linkedin.com/in/mutalib-asgarov-3a532b284/)

------------------------------------------------------------------------

**Saint Stream API** --- ASP.NET Core Web API project.
