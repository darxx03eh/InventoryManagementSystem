using InventoryManagementSystem.UI.Interfaces;
using InventoryManagementSystem.UI.Implementations;

namespace InventoryManagementSystem;

class Program
{
    static void Main(string[] args)
    {
        IMenu menu = new Menu();
        menu.Start();
    }
}