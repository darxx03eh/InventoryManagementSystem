using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Service.Interfaces;
using InventoryManagementSystem.UI.Helpers;
using InventoryManagementSystem.UI.Interfaces;
using Spectre.Console;

namespace InventoryManagementSystem.UI.Implementations;

public class Menu(IInventoryService inventoryService) : IMenu
{
    private const string AddProductOption = "Add Product";
    private const string ViewProductsOption = "View Products";
    private const string EditProductOption = "Edit Product";
    private const string DeleteProductOption = "Delete Product";
    private const string SearchProductOption = "Search Product";
    private const string ExitOption = "Exit";

    private readonly IInventoryService _inventoryService = inventoryService;

    public void Start()
    {
        ConsoleHelper.PlayStartupAnimation();

        var isRunning = true;
        while (isRunning)
        {
            ConsoleHelper.DrawShell("Dashboard");
            DrawDashboard();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]Select command[/]")
                    .HighlightStyle(new Style(Color.Black, Color.Aqua))
                    .PageSize(8)
                    .MoreChoicesText("[grey]Move up and down to see more options[/]")
                    .AddChoices(
                        AddProductOption,
                        ViewProductsOption,
                        EditProductOption,
                        DeleteProductOption,
                        SearchProductOption,
                        ExitOption));

            switch (choice)
            {
                case AddProductOption:
                    AddProduct();
                    break;
                case ViewProductsOption:
                    ViewProducts();
                    break;
                case EditProductOption:
                    EditProduct();
                    break;
                case DeleteProductOption:
                    DeleteProduct();
                    break;
                case SearchProductOption:
                    SearchProduct();
                    break;
                case ExitOption:
                    isRunning = false;
                    break;
            }
        }

        ExitApplication();
    }

    private void AddProduct()
    {
        do
        {
            ConsoleHelper.DrawShell("Add Product");
            ConsoleHelper.DrawSectionIntro(
                "Create Inventory Item",
                "Enter the product details, review the preview, then confirm the save.",
                "green");

            var name = PromptProductName("Product name");
            var price = PromptPrice("Price");
            var quantity = PromptQuantity("Quantity");
            var product = new Product(name, price, quantity);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(BuildProductPanel(product, "New Product Preview"));
            if (!AnsiConsole.Confirm("Save this product?"))
            {
                ConsoleHelper.WarningPanel("Add cancelled", "No product was added.");
                continue;
            }

            try
            {
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.Dots)
                    .SpinnerStyle(Style.Parse("cyan"))
                    .Start("Adding product...", _ =>
                    {
                        Thread.Sleep(350);
                        _inventoryService.AddProduct(product.Name, product.Price, product.Quantity);
                    });

                ConsoleHelper.SuccessPanel("Product added", $"{product.Name} is now available in inventory.");
            }
            catch (Exception exp) when (exp is ArgumentException or InvalidOperationException)
            {
                ConsoleHelper.ErrorPanel("Could not add product", exp.Message);
            }
        } while (AnsiConsole.Confirm("Add another product?"));
    }

    private void ViewProducts()
    {
        ConsoleHelper.DrawShell("Products");
        ConsoleHelper.DrawSectionIntro(
            "Inventory Browser",
            "Review product counts, total value, and stock health from one screen.",
            "cyan");

        var products = _inventoryService.GetProducts();
        if (!products.Any())
        {
            ConsoleHelper.WarningPanel("No products yet", "Add your first product to start tracking inventory.");
            ConsoleHelper.Pause();
            return;
        }

        AnsiConsole.Write(BuildSummaryPanel(products));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(BuildStockChart(products));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(BuildProductsTable(products));
        ConsoleHelper.Pause();
    }

    private void SearchProduct()
    {
        do
        {
            ConsoleHelper.DrawShell("Search Product");
            ConsoleHelper.DrawSectionIntro(
                "Find Product",
                "Search is case-insensitive and uses the exact product name.",
                "blue");

            var name = AnsiConsole.Ask<string>("Search by [cyan]product name[/]:").Trim();
            var product = _inventoryService.SearchProduct(name);

            if (product is null)
            {
                ConsoleHelper.WarningPanel("No match found", $"There is no product named '{Markup.Escape(name)}'.");
            }
            else
            {
                AnsiConsole.Write(BuildProductPanel(product, "Search Result"));
            }
        } while (AnsiConsole.Confirm("Search again?"));
    }

    private void DeleteProduct()
    {
        do
        {
            ConsoleHelper.DrawShell("Delete Product");
            ConsoleHelper.DrawSectionIntro(
                "Remove Inventory Item",
                "A preview is shown before anything is deleted.",
                "red");

            var name = AnsiConsole.Ask<string>("Product [cyan]name[/] to delete:").Trim();
            var product = _inventoryService.SearchProduct(name);

            if (product is null)
            {
                ConsoleHelper.WarningPanel("Product not found", $"Nothing was deleted for '{Markup.Escape(name)}'.");
                continue;
            }

            AnsiConsole.Write(BuildProductPanel(product, "Product Selected"));
            if (!AnsiConsole.Confirm($"Delete [red]{Markup.Escape(product.Name)}[/]?"))
            {
                ConsoleHelper.WarningPanel("Delete cancelled", "Inventory was not changed.");
                continue;
            }

            var isDeleted = false;
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("red"))
                .Start("Deleting product...", _ =>
                {
                    Thread.Sleep(350);
                    isDeleted = _inventoryService.DeleteProduct(product.Name);
                });

            if (isDeleted)
                ConsoleHelper.SuccessPanel("Product deleted", $"{product.Name} was removed from inventory.");
            else
                ConsoleHelper.ErrorPanel("Delete failed", "The product could not be removed.");
        } while (AnsiConsole.Confirm("Delete another product?"));
    }

    private void EditProduct()
    {
        do
        {
            ConsoleHelper.DrawShell("Edit Product");
            ConsoleHelper.DrawSectionIntro(
                "Update Inventory Item",
                "Review the current product, enter the replacement values, then confirm the change.",
                "yellow");

            var oldName = AnsiConsole.Ask<string>("Product [cyan]name[/] to edit:").Trim();
            var product = _inventoryService.SearchProduct(oldName);

            if (product is null)
            {
                ConsoleHelper.WarningPanel("Product not found", $"There is no product named '{Markup.Escape(oldName)}'.");
                continue;
            }

            AnsiConsole.Write(BuildProductPanel(product, "Current Product"));
            AnsiConsole.WriteLine();

            var newName = PromptProductName("New name");
            var newPrice = PromptPrice("New price");
            var newQuantity = PromptQuantity("New quantity");
            var updatedProduct = new Product(newName, newPrice, newQuantity);

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Columns(
                BuildProductPanel(product, "Before"),
                BuildProductPanel(updatedProduct, "After"))
            {
                Expand = true
            });

            if (!AnsiConsole.Confirm("Apply these changes?"))
            {
                ConsoleHelper.WarningPanel("Update cancelled", "Inventory was not changed.");
                continue;
            }

            try
            {
                var isUpdated = false;
                AnsiConsole.Status()
                    .Spinner(Spinner.Known.BouncingBar)
                    .SpinnerStyle(Style.Parse("yellow"))
                    .Start("Updating product...", _ =>
                    {
                        Thread.Sleep(350);
                        isUpdated = _inventoryService.UpdateProduct(
                            oldName,
                            updatedProduct.Name,
                            updatedProduct.Price,
                            updatedProduct.Quantity);
                    });

                if (isUpdated)
                    ConsoleHelper.SuccessPanel("Product updated", $"{oldName} was updated successfully.");
                else
                    ConsoleHelper.ErrorPanel("Update failed", "The product could not be updated.");
            }
            catch (Exception exp) when (exp is ArgumentException or InvalidOperationException)
            {
                ConsoleHelper.ErrorPanel("Could not update product", exp.Message);
            }
        } while (AnsiConsole.Confirm("Edit another product?"));
    }

    private void DrawDashboard()
    {
        var products = _inventoryService.GetProducts();
        AnsiConsole.Write(BuildCommandTable());
        AnsiConsole.WriteLine();

        if (!products.Any())
        {
            ConsoleHelper.WarningPanel("Inventory is empty", "Start with Add Product to create the first item.");
            return;
        }

        AnsiConsole.Write(BuildSummaryPanel(products));
        AnsiConsole.WriteLine();
        AnsiConsole.Write(BuildStockChart(products));
        AnsiConsole.WriteLine();
    }

    private static string PromptProductName(string label)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"{label} [grey](2+ characters)[/]:")
                .PromptStyle("cyan")
                .ValidationErrorMessage("[red]Name must be at least 2 characters long.[/]")
                .Validate(name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length >= 2))
            .Trim();
    }

    private static decimal PromptPrice(string label)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<decimal>($"{label} [grey](greater than 0)[/]:")
                .PromptStyle("yellow")
                .ValidationErrorMessage("[red]Price must be greater than zero.[/]")
                .Validate(price => price > 0));
    }

    private static int PromptQuantity(string label)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"{label} [grey](0 or more)[/]:")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]Quantity cannot be negative.[/]")
                .Validate(quantity => quantity >= 0));
    }

    private static Table BuildProductsTable(IEnumerable<Product> products)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Expand();

        table.AddColumn(new TableColumn("[bold cyan]Product[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold yellow]Price[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold green]Quantity[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold blue]Stock[/]").Centered());

        foreach (var product in products.OrderBy(product => product.Name))
        {
            table.AddRow(
                Markup.Escape(product.Name),
                $"[yellow]{product.Price:C}[/]",
                $"[green]{product.Quantity}[/]",
                GetStockLabel(product.Quantity));
        }

        return table;
    }

    private static Table BuildCommandTable()
    {
        return new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey)
            .AddColumn(new TableColumn("[bold]Command[/]").LeftAligned())
            .AddColumn(new TableColumn("[bold]Purpose[/]").LeftAligned())
            .AddRow("[green]Add Product[/]", "[grey]Create a new inventory record[/]")
            .AddRow("[cyan]View Products[/]", "[grey]Browse table, value summary, and stock health[/]")
            .AddRow("[yellow]Edit Product[/]", "[grey]Change values with a before/after preview[/]")
            .AddRow("[red]Delete Product[/]", "[grey]Remove a record after confirmation[/]")
            .AddRow("[blue]Search Product[/]", "[grey]Open a focused product details card[/]");
    }

    private static Panel BuildSummaryPanel(IReadOnlyCollection<Product> products)
    {
        var totalUnits = products.Sum(product => product.Quantity);
        var totalValue = products.Sum(product => product.Price * product.Quantity);
        var lowStockCount = products.Count(product => product.Quantity <= 5);

        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddColumn();
        grid.AddRow(
            $"[bold cyan]{products.Count}[/]\n[grey]Products[/]",
            $"[bold green]{totalUnits}[/]\n[grey]Units[/]",
            $"[bold yellow]{totalValue:C}[/]\n[grey]Inventory Value[/]");
        grid.AddRow(
            $"[bold red]{lowStockCount}[/]\n[grey]Low Stock[/]",
            $"[bold blue]{products.Max(product => product.Price):C}[/]\n[grey]Highest Price[/]",
            $"[bold white]{products.Average(product => product.Price):C}[/]\n[grey]Average Price[/]");

        return new Panel(grid)
        {
            Header = new PanelHeader("[bold]Inventory Snapshot[/]"),
            Border = BoxBorder.Double,
            BorderStyle = Style.Parse("cyan")
        };
    }

    private static Panel BuildStockChart(IReadOnlyCollection<Product> products)
    {
        var chart = new BarChart()
            .Width(60)
            .Label("[bold]Stock Health[/]")
            .CenterLabel();

        chart.AddItem("Out of stock", products.Count(product => product.Quantity == 0), Color.Red);
        chart.AddItem("Low stock", products.Count(product => product.Quantity is > 0 and <= 5), Color.Yellow);
        chart.AddItem("Healthy", products.Count(product => product.Quantity > 5), Color.Green);

        return new Panel(chart)
        {
            Header = new PanelHeader("[bold]Health Distribution[/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = Style.Parse("green")
        };
    }

    private static Panel BuildProductPanel(Product product, string title)
    {
        var table = new Table()
            .NoBorder()
            .HideHeaders();

        table.AddColumn("Field");
        table.AddColumn("Value");
        table.AddRow("[grey]Name[/]", $"[cyan]{Markup.Escape(product.Name)}[/]");
        table.AddRow("[grey]Price[/]", $"[yellow]{product.Price:C}[/]");
        table.AddRow("[grey]Quantity[/]", $"[green]{product.Quantity}[/]");
        table.AddRow("[grey]Stock[/]", GetStockLabel(product.Quantity));

        return new Panel(table)
        {
            Header = new PanelHeader($"[bold]{Markup.Escape(title)}[/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = Style.Parse("cyan")
        };
    }

    private static string GetStockLabel(int quantity)
    {
        return quantity switch
        {
            0 => "[red]Out[/]",
            <= 5 => "[yellow]Low[/]",
            _ => "[green]Healthy[/]"
        };
    }

    private static void ExitApplication()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new Rule("[yellow]Closing Inventory Management System[/]")
                .RuleStyle("yellow")
                .Centered());

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Star)
            .SpinnerStyle(Style.Parse("cyan"))
            .Start("Saving console session...", _ => Thread.Sleep(900));

        AnsiConsole.MarkupLine("[green]Session closed successfully.[/]");
    }
}
