using InventoryManagementSystem.Models.Entities;

namespace InventoryManagementSystem.Service.Interfaces;

public interface IInventoryService
{
    public void AddProduct(string name, decimal price, int quantity);
    public IReadOnlyCollection<Product> GetProducts();
    public Product? SearchProduct(string name);
    public bool DeleteProduct(string name);
    public bool UpdateProduct(string oldName, string newName, decimal price, int quantity);
}