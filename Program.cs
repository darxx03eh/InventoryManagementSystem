using InventoryManagementSystem.Infrastructure.IRepositories;
using InventoryManagementSystem.Infrastructure.Repositories;
using InventoryManagementSystem.Service.Implementations;
using InventoryManagementSystem.Service.Interfaces;
using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Implementations;
using Spectre.Console;

namespace InventoryManagementSystem;

class Program
{
    static int Main(string[] args)
    {
        try
        {
            Console.Title = "Inventory Management System";

            IProductRepository productRepository = new ProductRepository();
            IInventoryService inventoryService = new InventoryService(productRepository);
            IMenu menu = new Menu(inventoryService);
            menu.Start();

            return 0;
        }
        catch (Exception exp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Panel($"""
                           [red]{Markup.Escape(exp.Message)}[/]

                           [grey]The application stopped unexpectedly. Restart the app and try again.[/]
                           """)
                {
                    Header = new PanelHeader("[bold red]Fatal Error[/]"),
                    Border = BoxBorder.Double,
                    BorderStyle = Style.Parse("red"),
                    Padding = new Padding(1, 1)
                });

            if (!string.IsNullOrWhiteSpace(exp.StackTrace))
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Panel(Markup.Escape(exp.StackTrace))
                    {
                        Header = new PanelHeader("[grey]Diagnostic Details[/]"),
                        Border = BoxBorder.Rounded,
                        BorderStyle = Style.Parse("grey"),
                        Padding = new Padding(1, 0)
                    });
            }

            return 1;
        }
    }
}
