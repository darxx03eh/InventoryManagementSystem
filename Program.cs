using InventoryManagementSystem.Infrastructure.IRepositories;
using InventoryManagementSystem.Infrastructure.Repositories;
using InventoryManagementSystem.Service.Implementations;
using InventoryManagementSystem.Service.Interfaces;
using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Implementations;

namespace InventoryManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            IProductRepository productRepository = new ProductRepository();
            IInventoryService inventoryService = new InventoryService(productRepository);
            IMenu menu = new Menu(inventoryService);
            menu.Start();
        }
        catch (Exception exp)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"""
                               Critical error occured!
                               {exp.Message}")"
                               """);
            Console.ResetColor();
        }
        finally
        {
            Console.WriteLine("\nApplication closed.");
        }
    }
}