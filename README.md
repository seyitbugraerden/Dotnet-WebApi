<div align="center">

# 🛒 ShopApp — ASP.NET Core E-Commerce API

### Layered e-commerce backend built with ASP.NET Core, Entity Framework Core and SQL Server

A multi-layer .NET application demonstrating **REST API development, Entity Framework Core, Repository & Unit of Work patterns, business services, DTOs and SQL Server persistence**.

<br />

![.NET](https://img.shields.io/badge/.NET_Core-3.1-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-3.1-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-SQL_Server-2496ED?style=for-the-badge&logo=docker&logoColor=white)

</div>

---

## About the Project

**ShopApp** is a layered e-commerce application built with **ASP.NET Core 3.1**, **Entity Framework Core** and **Microsoft SQL Server**.

The solution separates responsibilities into dedicated projects for:

- Domain entities
- Data access
- Business logic
- REST API
- MVC web interface

The Web API exposes product CRUD operations while the shared business and data layers also contain abstractions for:

- Products
- Categories
- Shopping carts
- Orders

The repository demonstrates a traditional enterprise-style .NET architecture based on dependency injection, repositories, service abstractions and Unit of Work.

---

## Solution Architecture

```text
                    Client
                      │
                      ▼
               ASP.NET Core API
                      │
                      ▼
                 DTO Layer
                      │
                      ▼
               Business Layer
                      │
                      ▼
             Unit of Work Pattern
                      │
                      ▼
              Repository Layer
                      │
                      ▼
            Entity Framework Core
                      │
                      ▼
                 SQL Server
```

The repository also contains a separate MVC application:

```text
ASP.NET Core MVC
       │
       ▼
Business Layer
       │
       ▼
Data Access Layer
       │
       ▼
SQL Server
```

---

## Solution Projects

```text
shopapp.sln
│
├── shopapp.entity
├── shopapp.data
├── shopapp.business
├── shopapp.webapi
└── shopapp.webui
```

---

## Layer Responsibilities

### `shopapp.entity`

Contains the domain entities:

- Product
- Category
- ProductCategory
- Cart
- CartItem
- Order
- OrderItem

---

### `shopapp.data`

Responsible for database access.

Includes:

- Entity Framework Core
- Repository pattern
- Generic repository
- Entity-specific repositories
- Unit of Work
- DbContext
- Entity configurations
- Database migrations
- Seed data

---

### `shopapp.business`

Contains application/business logic.

Includes:

- Product services
- Category services
- Cart services
- Order services
- Validation rules
- Service interfaces

---

### `shopapp.webapi`

REST API layer.

Includes:

- API controllers
- Product DTO
- Dependency injection
- SQL Server configuration
- Async product CRUD operations

---

### `shopapp.webui`

ASP.NET Core MVC application built on top of the same business and data layers.

The repository contains interfaces for:

- Product catalogue
- Categories
- User accounts
- Admin pages
- Cart
- Checkout
- Orders

---

## Tech Stack

| Technology | Purpose |
| --- | --- |
| **ASP.NET Core 3.1** | Web API and MVC applications |
| **C#** | Backend language |
| **Entity Framework Core 3.1** | ORM |
| **SQL Server** | Relational database |
| **SQL Server 2022 Docker Image** | Local database environment |
| **Repository Pattern** | Data-access abstraction |
| **Unit of Work** | Transaction/data-access coordination |
| **Dependency Injection** | Service management |
| **DTO Pattern** | API response shaping |
| **EF Core Migrations** | Database versioning |
| **ASP.NET Core Identity** | Dependency available for identity-backed UI |
| **Docker Compose** | Local SQL Server orchestration |

---

## REST API

The main API controller is:

```text
shopapp.webapi/Controllers/ProductsConroller.cs
```

and exposes:

```text
/api/products
```

---

## Product Endpoints

### Get All Products

```http
GET /api/products
```

Returns a collection of `ProductDTO` objects.

Example response structure:

```json
[
  {
    "productId": 1,
    "name": "Product",
    "url": "product",
    "price": 100,
    "description": "Description",
    "imageUrl": "1.jpg"
  }
]
```

---

## Get Product by ID

```http
GET /api/products/{id}
```

Example:

```http
GET /api/products/1
```

Returns:

```text
200 OK
```

when the product exists.

If the entity cannot be found:

```text
404 Not Found
```

is returned.

---

## Create Product

```http
POST /api/products
```

The controller creates the entity asynchronously:

```csharp
await _productService.CreateAsync(entity);
```

and returns:

```text
201 Created
```

using:

```csharp
CreatedAtAction(...)
```

---

## Update Product

```http
PUT /api/products/{id}
```

The API verifies:

```text
Route ID == Product ID
```

before performing the update.

Possible responses include:

```text
204 No Content
400 Bad Request
404 Not Found
```

---

## Delete Product

```http
DELETE /api/products/{id}
```

Deletes an existing product asynchronously.

Returns:

```text
204 No Content
```

when successful.

---

## Product DTO

API responses use:

```text
ProductDTO
```

instead of directly returning every domain property.

Current DTO fields:

```text
ProductId
Name
Url
Price
Description
ImageUrl
```

Domain-specific properties such as:

```text
IsApproved
IsHome
DateAdded
ProductCategories
```

are not included in this DTO.

---

## Dependency Injection

Services are registered in:

```text
shopapp.webapi/Startup.cs
```

Example:

```csharp
services.AddScoped<IUnitOfWork, UnitOfWork>();

services.AddScoped<IProductService, ProductManager>();
services.AddScoped<ICategoryService, CategoryManager>();
services.AddScoped<ICartService, CartManager>();
services.AddScoped<IOrderService, OrderManager>();
```

This keeps controllers dependent on abstractions rather than concrete data-access implementations.

---

## Request Flow

A typical product request follows:

```text
HTTP Request
    │
    ▼
ProductsController
    │
    ▼
IProductService
    │
    ▼
ProductManager
    │
    ▼
IUnitOfWork
    │
    ▼
IProductRepository
    │
    ▼
EfCoreProductRepository
    │
    ▼
ShopContext
    │
    ▼
SQL Server
```

---

## Repository Pattern

The data layer defines a generic repository abstraction:

```text
IRepository<T>
```

alongside specialized repositories:

```text
IProductRepository
ICategoryRepository
ICartRepository
IOrderRepository
```

This keeps data access separate from business logic.

---

## Unit of Work

The repository implements:

```text
IUnitOfWork
```

to coordinate repositories through a shared `ShopContext`.

Available repositories:

```csharp
Carts
Categories
Orders
Products
```

Changes can be committed synchronously:

```csharp
Save()
```

or asynchronously:

```csharp
SaveAsync()
```

---

## Business Layer

Product business logic lives inside:

```text
ProductManager
```

The manager supports:

- Create
- CreateAsync
- Update
- UpdateAsync
- Delete
- DeleteAsync
- GetAll
- GetById
- Get product details
- Get products by category
- Search products
- Homepage products
- Product/category relations

---

## Product Validation

The business layer includes rules such as:

```text
Product name cannot be empty
Product price cannot be negative
```

Example:

```csharp
if (string.IsNullOrEmpty(entity.Name))
{
    ErrorMessage += "ürün ismi girmelisiniz.";
}
```

and:

```csharp
if (entity.Price < 0)
{
    ErrorMessage += "ürün fiyatı negatif olamaz.";
}
```

---

## Entity Framework Core

The application uses:

```text
Entity Framework Core 3.1
```

with:

```text
Microsoft.EntityFrameworkCore.SqlServer
```

as the active database provider.

---

## ShopContext

The main database context defines:

```csharp
public DbSet<Product> Products { get; set; }
public DbSet<Category> Categories { get; set; }
public DbSet<Cart> Carts { get; set; }
public DbSet<CartItem> CartItems { get; set; }
public DbSet<Order> Orders { get; set; }
public DbSet<OrderItem> OrderItems { get; set; }
```

---

## Entity Configuration

Entity mappings are separated from the DbContext.

Current configurations include:

```text
ProductConfiguration
CategoryConfiguration
ProductCategoryConfiguration
```

They are applied through:

```csharp
modelBuilder.ApplyConfiguration(...)
```

---

## Seed Data

The context also executes:

```csharp
modelBuilder.Seed();
```

during model creation.

The current database migration contains example product data for local development.

---

## Database Migrations

Existing migrations are stored under:

```text
shopapp.data/Migrations/
```

including:

```text
20210308142502_InitialCreate
ShopContextModelSnapshot
```

---

## Product Entity

The current `Product` model contains:

```text
ProductId
Name
Url
Price
Description
ImageUrl
IsApproved
IsHome
DateAdded
ProductCategories
```

---

## Category Relationship

Products and categories are connected using:

```text
ProductCategory
```

which represents the many-to-many relationship between the two entities.

---

## Shopping Cart Domain

The solution contains:

```text
Cart
CartItem
```

entities along with:

```text
ICartRepository
ICartService
CartManager
```

providing the domain foundation for persistent shopping carts.

---

## Order Domain

Order support includes:

```text
Order
OrderItem
```

The current `Order` model stores:

```text
OrderNumber
OrderDate
UserId

FirstName
LastName
Address
City
Phone
Email
Note

PaymentId
ConversationId

PaymentType
OrderState
OrderItems
```

---

## Payment Types

The domain currently defines:

```text
CreditCard
Eft
```

as payment types.

---

## Order States

Available order states include:

```text
waiting
unpaid
completed
```

---

## MVC Web Application

The solution contains a separate:

```text
shopapp.webui
```

ASP.NET Core MVC project.

It includes UI flows for:

### Account

```text
Login
Register
Confirm Email
Forgot Password
Reset Password
Access Denied
```

### Admin

```text
Product Management
Category Management
Role Management
User Management
```

### Shopping

```text
Product Catalogue
Product Details
Search
Cart
Checkout
Orders
```

---

## Admin Views

The repository contains dedicated admin pages such as:

```text
ProductList
ProductCreate
ProductEdit

CategoryList
CategoryCreate
CategoryEdit

RoleList
RoleCreate
RoleEdit

UserList
UserEdit
```

---

## Local SQL Server

For local development, the repository includes:

```text
compose.sqlserver.yml
```

using:

```text
mcr.microsoft.com/mssql/server:2022-latest
```

---

## Docker SQL Architecture

```text
ASP.NET Core
      │
      ▼
127.0.0.1:1433
      │
      ▼
SQL Server 2022
      │
      ▼
Docker Volume
```

The SQL Server port is bound only to:

```text
127.0.0.1:1433
```

for local access.

---

## Start SQL Server

Create a local environment file containing:

```text
MSSQL_SA_PASSWORD
```

and run:

```bash
docker compose --env-file .env.sqlserver \
  -f compose.sqlserver.yml up -d
```

Stop without deleting data:

```bash
docker compose --env-file .env.sqlserver \
  -f compose.sqlserver.yml stop
```

---

## Local Database

Current local database name:

```text
shopappdb
```

Server:

```text
127.0.0.1,1433
```

Authentication:

```text
SQL Server Authentication
```

---

## Database Environment

The helper script:

```text
scripts/with-local-db.py
```

loads the database connection through:

```text
ConnectionStrings__MsSqlConnection
```

without requiring the local development password to be committed to source control.

---

## Run the Web API

From the repository root:

```bash
cd shopapp.webapi
python3 ../scripts/with-local-db.py dotnet run
```

The documented local products endpoint is:

```text
http://localhost:4200/api/products
```

---

## Run Database Migrations

For the `ShopContext`:

```bash
cd shopapp.webapi

python3 ../scripts/with-local-db.py \
  dotnet ef database update \
  --project ../shopapp.data \
  --startup-project . \
  --context ShopContext
```

---

## Run the MVC Application

The MVC project can use the same local `shopappdb` database.

```bash
cd shopapp.webui

python3 ../scripts/with-local-db.py dotnet run
```

---

## Project Structure

```text
dotnet-web-api/
│
├── shopapp.entity/
│   ├── Product.cs
│   ├── Category.cs
│   ├── ProductCategory.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   └── OrderItem.cs
│
├── shopapp.data/
│   ├── Abstract/
│   │   ├── IRepository.cs
│   │   ├── IProductRepository.cs
│   │   ├── ICategoryRepository.cs
│   │   ├── ICartRepository.cs
│   │   ├── IOrderRepository.cs
│   │   └── IUnitOfWork.cs
│   │
│   ├── Concrete/
│   │   └── EfCore/
│   │       ├── EfCoreGenericRepository.cs
│   │       ├── EfCoreProductRepository.cs
│   │       ├── EfCoreCategoryRepository.cs
│   │       ├── EfCoreCartRepository.cs
│   │       ├── EfCoreOrderRepository.cs
│   │       ├── ShopContext.cs
│   │       └── UnitOfWork.cs
│   │
│   ├── Configurations/
│   └── Migrations/
│
├── shopapp.business/
│   ├── Abstract/
│   │   ├── IProductService.cs
│   │   ├── ICategoryService.cs
│   │   ├── ICartService.cs
│   │   └── IOrderService.cs
│   │
│   └── Concrete/
│       ├── ProductManager.cs
│       ├── CategoryManager.cs
│       ├── CartManager.cs
│       └── OrderManager.cs
│
├── shopapp.webapi/
│   ├── Controllers/
│   │   └── ProductsConroller.cs
│   ├── DTO/
│   │   └── ProductDTO.cs
│   ├── Program.cs
│   ├── Startup.cs
│   └── appsettings.json
│
├── shopapp.webui/
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── ViewComponents/
│   └── wwwroot/
│
├── scripts/
│   └── with-local-db.py
│
├── compose.sqlserver.yml
├── LOCAL-SQL.md
└── shopapp.sln
```

---

## Current Development Status

### Implemented

- Multi-project .NET solution
- Entity layer
- Repository pattern
- Generic repository
- Unit of Work
- Business/service layer
- Dependency injection
- Entity Framework Core
- SQL Server integration
- Database migrations
- Seed data
- Async CRUD
- Product REST API
- Product DTO
- Categories
- Shopping carts
- Orders
- MVC storefront
- Admin views
- Account views
- Docker-based local SQL Server

### Web API Scope

The currently verified REST controller exposes CRUD endpoints for:

```text
Products
```

Although category, cart and order business/data abstractions exist, dedicated REST controllers for those resources are not present in the inspected Web API tree.

---

## Current Limitations

The Web API currently does not include a verified implementation of:

- Swagger / OpenAPI UI
- JWT authentication
- API authorization policies
- Category REST controller
- Cart REST controller
- Order REST controller
- Global exception middleware
- API versioning
- Automated API tests
- Request DTO validation framework
- Pagination on `GET /api/products`

---

## Modernization Opportunities

The project currently targets:

```text
.NET Core 3.1
```

which represents an older .NET application architecture.

A modernized version could migrate toward:

```text
.NET 8 / .NET 10+
ASP.NET Core Web API
Modern hosting model
Swagger / OpenAPI
JWT authentication
FluentValidation
EF Core modern versions
ProblemDetails
API versioning
Automated integration tests
```

---

## Recommended Future API Architecture

```text
Controller
   │
   ▼
Request DTO
   │
   ▼
Validation
   │
   ▼
Application Service
   │
   ▼
Unit of Work
   │
   ▼
Repository
   │
   ▼
EF Core
   │
   ▼
SQL Server
```

---

## Developer

<div align="center">

### Seyit Buğra Erden

**Full Stack Developer · Software Engineer**

[GitHub](https://github.com/seyitbugraerden) ·
[LinkedIn](https://www.linkedin.com/in/sbugraerden/)

<br />

Built with **ASP.NET Core · C# · Entity Framework Core · SQL Server · Docker**

</div>
