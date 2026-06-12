using Spectre.Console;

namespace InventoryManagementSystem.UI.Helpers;

public static class ConsoleHelper
{
    public static void PlayStartupAnimation()
    {
        AnsiConsole.Clear();
        DrawBanner();

        AnsiConsole.Progress()
            .AutoClear(true)
            .HideCompleted(true)
            .Columns(
                new SpinnerColumn { Style = Style.Parse("cyan") },
                new TaskDescriptionColumn(),
                new ProgressBarColumn { CompletedStyle = Style.Parse("cyan") },
                new PercentageColumn())
            .Start(context =>
            {
                var task = context.AddTask("[cyan]Preparing inventory workspace[/]", maxValue: 100);

                while (!task.IsFinished)
                {
                    task.Increment(20);
                    Thread.Sleep(120);
                }
            });
    }

    public static void DrawShell(string section)
    {
        AnsiConsole.Clear();
        DrawBanner();

        AnsiConsole.Write(
            new Rule($"[bold yellow]{Markup.Escape(section)}[/]")
                .RuleStyle("yellow")
                .LeftJustified());
        AnsiConsole.WriteLine();
    }

    public static void DrawSectionIntro(string title, string description, string color)
    {
        AnsiConsole.Write(
            new Panel(new Markup($"[bold]{Markup.Escape(title)}[/]\n[grey]{Markup.Escape(description)}[/]"))
            {
                Border = BoxBorder.Rounded,
                BorderStyle = Style.Parse(color),
                Padding = new Padding(1, 0)
            });
        AnsiConsole.WriteLine();
    }

    public static void SuccessPanel(string title, string message)
    {
        WriteMessagePanel(title, message, "green", BoxBorder.Rounded);
    }

    public static void WarningPanel(string title, string message)
    {
        WriteMessagePanel(title, message, "yellow", BoxBorder.Rounded);
    }

    public static void ErrorPanel(string title, string message)
    {
        WriteMessagePanel(title, message, "red", BoxBorder.Double);
    }

    public static void Pause()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Press any key to return to the dashboard.[/]");
        Console.ReadKey(intercept: true);
    }

    private static void DrawBanner()
    {
        AnsiConsole.Write(
            new FigletText("IMS")
                .Color(Color.Cyan1));

        AnsiConsole.Write(new Rule("[bold white]Inventory Management System[/] [grey]|[/] [cyan]Console Operations[/]")
            .RuleStyle("cyan")
            .Centered());
        AnsiConsole.WriteLine();
    }

    private static void WriteMessagePanel(string title, string message, string color, BoxBorder border)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.Write(
            new Panel(Markup.Escape(message))
            {
                Header = new PanelHeader($"[bold {color}]{Markup.Escape(title)}[/]"),
                Border = border,
                BorderStyle = Style.Parse(color),
                Padding = new Padding(1, 0)
            });
        AnsiConsole.WriteLine();
    }
}
