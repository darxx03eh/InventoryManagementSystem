using InventoryManagementSystem.Service.Interfaces;
using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Helpers;
namespace InventoryManagementSystem.UI.Implementations;

public class Menu(IInventoryService inventoryService) : IMenu
{
    private readonly IInventoryService _inventoryService = inventoryService;
    public void Start()
    {
        while (true)
        {
            DisplayMenu();
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddProduct();
                    break;
                case "2":
                    ViewProducts();
                    break;
                case "3":
                    EditProduct();
                    break;
                case "4":
                    DeleteProduct();
                    break;
                case "5":
                    SearchProduct();
                    break;
                case "6":
                    ExitApplication();
                    return;
                default:
                    ConsoleHelper.Error("Invalid option!");
                    ConsoleHelper.Pause();
                    break;
            }
        }
    }
    private void DisplayMenu()
    {
        ConsoleHelper.DrawHeader("Inventory Management System");
        Console.WriteLine("""
                          [1] Add Product
                          [2] View Products
                          [3] Edit Product
                          [4] Delete Product
                          [5] Search Product
                          [6] Exit
                          """);
        Console.WriteLine();
        Console.WriteLine("───────────────────────────────────────");
        Console.Write("Choose option: ");
    }
    private void AddProduct()
    {
        ConsoleHelper.DrawHeader("Add Product");
        Console.Write("Product Name: ");
        string name =  Console.ReadLine() ?? string.Empty;
        decimal price = ConsoleHelper.ReadDecimal("Enter Price: ");
        int quantity = ConsoleHelper.ReadInt("Enter Quantity: ");
        _inventoryService.AddProduct(name, price, quantity);
        ConsoleHelper.Success("Product added successfully.");
        ConsoleHelper.Pause();
    }
    private void ViewProducts()
    {
        ConsoleHelper.DrawHeader("All Products");
        var products = _inventoryService.GetProducts();
        if (!products.Any())
        {
            ConsoleHelper.Warning("Inventory is empty.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine("""
                          ┌────┬────────────────────┬──────────┬──────────┐
                          │ #  │ Name               │ Price    │ Quantity │
                          ├────┼────────────────────┼──────────┼──────────┤
                          """);
        int index = 1;
        foreach (var product in products)
        {
            Console.WriteLine($"│ {index,-2} │ {product.Name,-18} │ {product.Price,-8:F2} │ {product.Quantity,-8} │");
            index++;
        }
        Console.WriteLine("└────┴────────────────────┴──────────┴──────────┘");
        ConsoleHelper.Pause();
    }
    private void SearchProduct()
    {
        ConsoleHelper.DrawHeader("Search Product");
        Console.Write("Product Name: ");
        string name = Console.ReadLine() ?? string.Empty;
        var product =  _inventoryService.SearchProduct(name);
        if (product is null)
        {
            ConsoleHelper.Error("Product not found.");
            ConsoleHelper.Pause();
            return;
        }
        ConsoleHelper.Success("Product found.");
        Console.WriteLine();
        Console.WriteLine($"""
                            Name     : {product.Name}
                            Price    : {product.Price:F2}
                            Quantity : {product.Quantity}
                            """);
        ConsoleHelper.Pause();
    }
    private void DeleteProduct()
    {
        ConsoleHelper.DrawHeader("Delete Product");
        Console.Write("Product Name: ");
        string name = Console.ReadLine() ?? string.Empty;
        bool isDeleted = _inventoryService.DeleteProduct(name);
        if(isDeleted)
            ConsoleHelper.Success("Product deleted successfully.");
        else ConsoleHelper.Error("Product not found");
        ConsoleHelper.Pause();
    }
    private void EditProduct()
    {
        ConsoleHelper.DrawHeader("Edit Product");
        Console.Write("Product Name: ");
        string oldName =  Console.ReadLine() ?? string.Empty;
        var existing = _inventoryService.SearchProduct(oldName);
        if (existing is null)
        {
            ConsoleHelper.Error("Product not found.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine($"""
                          
                          Current Values
                          Name     : {existing.Name}
                          Price    : {existing.Price:F2}
                          Quantity : {existing.Quantity}
                          """);
        Console.WriteLine("───────────────────────────────────────");
        Console.Write("New Name: ");
        string newName = Console.ReadLine() ?? string.Empty;
        decimal newPrice = ConsoleHelper.ReadDecimal("Enter Price: ");
        int newQuantity = ConsoleHelper.ReadInt("Enter Quantity: ");
        bool isUpdated = _inventoryService.UpdateProduct(oldName, newName, newPrice, newQuantity);
        if(isUpdated)
            ConsoleHelper.Success("Product edited successfully.");
        else ConsoleHelper.Error("Update failed.");
        ConsoleHelper.Pause();
    }
    private void ExitApplication()
    {
        ConsoleHelper.DrawHeader("Goodbye");
        ConsoleHelper.Success("Thank you for using Inventory Management System.");
        Thread.Sleep(1500);
        Environment.Exit(0);
    }
}