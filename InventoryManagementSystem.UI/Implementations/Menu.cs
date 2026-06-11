using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Helpers;
namespace InventoryManagementSystem.UI.Implementations;

public class Menu : IMenu
{
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
        // !Add Product Logic
        ConsoleHelper.Success("Product added successfully.");
        ConsoleHelper.Pause();
    }
    private void ViewProducts()
    {
        ConsoleHelper.DrawHeader("All Products");
        var products = new List<string>();
        if (products.Any())
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
            Console.WriteLine($"│ {index,-2} │ {product,-18} │ {product,-8:F2} │ {product,-8} │");
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
        // !Search Product Logic
        ConsoleHelper.Success("Product found.");
        Console.WriteLine();
        Console.WriteLine($"""
                            Name     : {name}
                            Price    : {2.5:F2}
                            Quantity : {2}
                            """);
        ConsoleHelper.Pause();
    }
    private void DeleteProduct()
    {
        ConsoleHelper.DrawHeader("Delete Product");
        Console.Write("Product Name: ");
        string name = Console.ReadLine() ?? string.Empty;
        // !Delete Product Logic
        if(true)
            ConsoleHelper.Success("Product deleted successfully.");
        else ConsoleHelper.Error("Product not found");
        ConsoleHelper.Pause();
    }
    private void EditProduct()
    {
        ConsoleHelper.DrawHeader("Edit Product");
        Console.Write("Product Name: ");
        string oldName =  Console.ReadLine() ?? string.Empty;
        // !Search Product Logic
        if (false)
        {
            ConsoleHelper.Error("Product not found.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine($"""
                          
                          Current Values
                          Name     : {oldName}
                          Price    : {2.5:F2}
                          Quantity : {2}
                          """);
        Console.WriteLine("───────────────────────────────────────");
        Console.Write("New Name: ");
        string newName = Console.ReadLine() ?? string.Empty;
        decimal newPrice = ConsoleHelper.ReadDecimal("Enter Price: ");
        int newQuantity = ConsoleHelper.ReadInt("Enter Quantity: ");
        // !Edit Product Logic
        if(true)
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