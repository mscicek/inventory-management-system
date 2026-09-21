# Inventory Management System

A console-based inventory management system developed with C# and .NET.

The project simulates basic ERP inventory operations such as product management, category management, supplier management, warehouse stock tracking, purchasing, sales and stock movements.

## Features

* Product management
* Category management
* Supplier management
* Warehouse management
* Warehouse-based stock tracking
* Manual stock entry and exit
* Purchase management
* Sales management
* Stock movement tracking
* Low-stock monitoring
* Product search by name or SKU
* Discounted price calculation
* Input validation with `TryParse`
* Stock quantity validation
* Basic object-oriented programming structure

## Technologies

* C#
* .NET 10
* Console Application
* LINQ
* Object-Oriented Programming

## Project Structure

```text
inventory-management-system
│
├── Models
│   ├── Product.cs
│   ├── Category.cs
│   ├── Supplier.cs
│   ├── Warehouse.cs
│   ├── WarehouseStock.cs
│   ├── StockMovement.cs
│   ├── StockMovementType.cs
│   ├── Purchase.cs
│   ├── PurchaseItem.cs
│   ├── Sale.cs
│   └── SaleItem.cs
│
├── Services
│   └── InventoryService.cs
│
└── Program.cs
```

## Main Concepts

### Product

Stores basic product information such as:

* Name
* SKU
* Price
* Minimum stock level
* Category

### Category

Groups products into categories.

### Supplier

Stores supplier information such as company name, contact person, phone number and email.

### Warehouse

Represents physical storage locations.

### WarehouseStock

Stores how many units of a product exist in a specific warehouse.

This allows the same product to have different stock quantities in different warehouses.

### Purchase

Represents a purchase from a supplier.

A purchase:

1. Selects a supplier.
2. Selects a warehouse.
3. Adds one or more products.
4. Increases warehouse stock.
5. Creates stock movement records.

### Sale

Represents a product sale.

A sale:

1. Selects a warehouse.
2. Selects a customer.
3. Adds one or more products.
4. Checks available stock.
5. Decreases warehouse stock.
6. Creates stock movement records.

## Example Flow

First create a category:

```text
Category: Electronics
```

Then create a product:

```text
Product: Laptop
SKU: LAP001
Price: 30000
Minimum Stock: 5
Category: Electronics
```

Create a warehouse:

```text
Warehouse: Main Warehouse
```

Create a supplier:

```text
Supplier: ABC Technology
```

Create a purchase:

```text
Supplier: ABC Technology
Warehouse: Main Warehouse

Laptop
Quantity: 20
Unit Price: 25000
```

The warehouse stock becomes:

```text
Laptop
Stock: 20
```

Then create a sale:

```text
Customer: Ahmet
Warehouse: Main Warehouse

Laptop
Quantity: 2
Unit Price: 30000
```

The warehouse stock becomes:

```text
Laptop
Stock: 18
```

A stock movement is also recorded for both the purchase and sale.

## Current Limitation

The current version uses in-memory `List<T>` collections.

Therefore, all data is lost when the application is closed.

There is currently no:

* SQL Server database
* Entity Framework Core
* REST API
* Authentication
* Authorization
* Web interface
* Persistent storage

## Roadmap

### Phase 1

* Console application
* C# fundamentals
* OOP
* Collections
* LINQ
* Input validation

### Phase 2

* SQL Server
* Entity Framework Core
* Database relationships
* Migrations
* Repository/Service structure

### Phase 3

* ASP.NET Core Web API
* RESTful endpoints
* DTOs
* Validation
* Dependency Injection
* Swagger
* Exception handling

### Phase 4

* JWT authentication
* Role-based authorization
* Admin/user roles
* User management

### Phase 5

* React frontend
* Dashboard
* Product management UI
* Warehouse management
* Purchase and sales screens
* Stock reports

### Phase 6

* Clean Architecture
* Logging with Serilog
* FluentValidation
* Automated testing
* Docker
* CI/CD

## Goal

The long-term goal of this project is to evolve the console application into a full ERP-style inventory management system using:

**C# + ASP.NET Core Web API + Entity Framework Core + SQL Server + React**

The project is being developed incrementally to understand the architecture behind real-world business applications.
