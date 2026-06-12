using InventoryManagementSystem.Service.Interfaces;
using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Helpers;
using Spectre.Console;

namespace InventoryManagementSystem.UI.Implementations;

public class Menu(IInventoryService inventoryService) : IMenu
{
    private readonly IInventoryService _inventoryService = inventoryService;
    public void Start()
    {
        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[cyan]Inventory Management System[/]")
                    .PageSize(6)
                    .AddChoices(
                        "Add Product",
                        "View Products",
                        "Edit Product",
                        "Delete Product",
                        "Search Product",
                        "Exit"));
            switch (choice)
            {
                case "Add Product":
                    AddProduct();
                    break;
                case "View Products":
                    ViewProducts();
                    break;
                case "Edit Product":
                    EditProduct();
                    break;
                case "Delete Product":
                    DeleteProduct();
                    break;
                case "Search Product":
                    SearchProduct();
                    break;
                case "Exit":
                    ExitApplication();
                    return;
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
        var name = AnsiConsole.Ask<string>("Enter [green]Product Name[/]: ");
        var price = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Enter [yellow]Price[/]: ")
                .ValidationErrorMessage("[red]Invalid price[/]")
                .Validate(price => price > 0));
        var quantity = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter [yellow]Quantity[/]: ")
                .ValidationErrorMessage("[red]Invalid quantity[/]")
                .Validate(quantity => quantity >= 0));
        _inventoryService.AddProduct(name, price, quantity);
        AnsiConsole.MarkupLine("[green]✔ Product added successfully[/]");
        if(AnsiConsole.Confirm("Continue?"))
            AddProduct();
        return;
    }
    private void ViewProducts()
    {
        var products = _inventoryService.GetProducts();
        if (!products.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No products available[/]");
            return;
        }

        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn(new TableColumn("[cyan]Name[/]").Centered());
        table.AddColumn(new TableColumn("[cyan]Price[/]").Centered());
        table.AddColumn(new TableColumn("Quantity").Centered());
        foreach(var product in products)
            table.AddRow(product.Name, $"{product.Price}", $"{product.Quantity}");
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
        if (AnsiConsole.Confirm("Back?"))
        {
            AnsiConsole.Clear();
            return;
        }
        ViewProducts();
    }
    private void SearchProduct()
    {
        var name = AnsiConsole.Ask<string>("Enter [green]Product Name[/]: ");
        var product = _inventoryService.SearchProduct(name);
        if (product is null)
        {
            AnsiConsole.MarkupLine("[red]Product not found[/]");
            return;
        }

        var panel = new Panel($"""
                               [green]Name: [/] {product.Name}
                               [yellow]Price: [/] {product.Price}
                               [blue]Quantity: [/] {product.Quantity}
                               """)
        {
            Header = new PanelHeader("Product Details"),
            Border = BoxBorder.Double
        };
        AnsiConsole.Write(panel);
        if (AnsiConsole.Confirm("Back?"))
        {
            AnsiConsole.Clear();
            return;
        }
        SearchProduct();
    }
    private void DeleteProduct()
    {
        var name = AnsiConsole.Ask<string>("Enter [green]Product Name[/]: ");
        var isDeleted = _inventoryService.DeleteProduct(name);
        if(isDeleted)
            AnsiConsole.MarkupLine("[green]✔ Deleted successfully[/]");
        else AnsiConsole.MarkupLine("[red]✖ Product not found[/]");
        if (AnsiConsole.Confirm("Back?"))
        {
            AnsiConsole.Clear();
            return;
        }
        DeleteProduct();
    }
    private void EditProduct()
    {
        var oldName =  AnsiConsole.Ask<string>("Enter [green]Product Name[/] to edit: ");
        var product = _inventoryService.SearchProduct(oldName);
        if (product is null)
        {
            AnsiConsole.MarkupLine("[red]Product not found[/]");
            return;
        }
        var newName = AnsiConsole.Ask<string>("Enter [green]New Name[/]: ");
        var newPrice = AnsiConsole.Prompt(
            new TextPrompt<decimal>("Enter new [yellow]Price[/]: ")
                .ValidationErrorMessage("[red]Invalid price[/]")
                .Validate(price => price > 0));
        var newQuantity = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter new [yellow]Quantity[/]: ")
                .ValidationErrorMessage("[red]Invalid quantity[/]")
                .Validate(quantity => quantity >= 0));
        var isUpdated = _inventoryService.UpdateProduct(oldName, newName, newPrice, newQuantity);
        if(isUpdated)
            AnsiConsole.MarkupLine("[green]✔ Updated successfully[/]");
        else AnsiConsole.MarkupLine("[red]✖ Product not found[/]");
        if (AnsiConsole.Confirm("Back?"))
        {
            AnsiConsole.Clear();
            return;
        }
        EditProduct();
    }
    private void ExitApplication()
    {
        AnsiConsole.Clear();

        var rule = new Rule("[yellow]Goodbye[/]");
        rule.Justification = Justify.Center;

        AnsiConsole.Write(rule);

        AnsiConsole.MarkupLine("");
        AnsiConsole.MarkupLine("[green]Thank you for using Inventory Management System![/]");
        AnsiConsole.MarkupLine("[grey]See you next time.[/]");
        AnsiConsole.MarkupLine("");

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .Start("Saving session...", _ =>
            {
                Thread.Sleep(1200);
            });

        Thread.Sleep(1000);

        Environment.Exit(0);
    }
}