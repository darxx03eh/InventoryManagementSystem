# Inventory Management System

A console-based inventory management application built with C# and Spectre.Console. The app provides a styled terminal interface for adding, viewing, editing, deleting, and searching products.

## Features

- Add products with name, price, and quantity validation.
- View all products in a formatted table.
- Search products by name.
- Edit existing product details.
- Delete products with confirmation.
- Dashboard with inventory summary and stock-health indicators.
- Spectre.Console UI with banners, panels, prompts, tables, charts, and loading animations.
- Global error handling in the main application entry point.

## Project Structure

```text
InventoryManagementSystem
+-- Program.cs
+-- InventoryManagementSystem.Models
|   +-- Product entity
+-- InventoryManagementSystem.Infrastructure
|   +-- Product repository
+-- InventoryManagementSystem.Service
|   +-- Inventory business logic
+-- InventoryManagementSystem.UI
    +-- Spectre.Console menu and console helpers
```

## Technologies

- C#
- .NET 10
- Spectre.Console
- Layered architecture

## Prerequisites

Install the .NET SDK that supports `net10.0`.

Check your installed SDKs:

```powershell
dotnet --list-sdks
```

## Getting Started

Clone the repository:

```powershell
git clone https://github.com/darxx03eh/InventoryManagementSystem.git
cd InventoryManagementSystem
```

Restore dependencies:

```powershell
dotnet restore InventoryManagementSystem.sln
```

Build the solution:

```powershell
dotnet build InventoryManagementSystem.sln
```

Run the application:

```powershell
dotnet run --project InventoryManagementSystem.csproj
```

## How To Use

When the application starts, you will see the Inventory Management dashboard.

Use the arrow keys to select an option:

- `Add Product`: create a new product record.
- `View Products`: display all products with summary information.
- `Edit Product`: update an existing product.
- `Delete Product`: remove a product after confirmation.
- `Search Product`: find a product by name.
- `Exit`: close the application.

## Product Rules

The application validates product data before saving:

- Product name cannot be empty.
- Product name must be at least 2 characters long.
- Product price must be greater than zero.
- Product quantity cannot be negative.
- Duplicate product names are not allowed.

## Example Workflow

1. Start the application.
2. Choose `Add Product`.
3. Enter the product name, price, and quantity.
4. Confirm the preview.
5. Choose `View Products` to see the inventory table and stock summary.
6. Use `Edit Product`, `Search Product`, or `Delete Product` as needed.

## Error Handling

Expected validation errors are shown inside styled error panels in the UI.

Unexpected application errors are caught in `Program.cs` and displayed using a fatal error panel with diagnostic details. The application returns a non-zero exit code when a fatal error occurs.

## Current Storage Behavior

Products are stored in memory using the repository layer. This means inventory data exists only while the application is running. When the application closes, the data is not saved permanently.

Persistent storage can be added later by replacing or extending the infrastructure repository.

## Development Commands

Build:

```powershell
dotnet build InventoryManagementSystem.sln
```

Run:

```powershell
dotnet run --project InventoryManagementSystem.csproj
```

Clean:

```powershell
dotnet clean InventoryManagementSystem.sln
```

## Notes

This project is designed as a clean, layered console application. The UI layer handles presentation, the service layer handles validation and business rules, and the infrastructure layer handles product storage.
