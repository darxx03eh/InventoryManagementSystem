using InventoryManagementSystem.Infrastructure.IRepositories;
using InventoryManagementSystem.Models.Entities;
using InventoryManagementSystem.Service.Interfaces;

namespace InventoryManagementSystem.Service.Implementations;

public class InventoryService(IProductRepository productRepository) : IInventoryService
{
    private readonly IProductRepository _productRepository = productRepository;
    public void AddProduct(string name, decimal price, int quantity)
    {
        ValidateInputs(name, price, quantity);
        var exists = _productRepository.Exists(name);
        if(exists)
            throw new InvalidOperationException("Product already exists.");
        var product = new Product(name, price, quantity);
        _productRepository.Add(product);
    }
    public IReadOnlyCollection<Product> GetProducts() => _productRepository.GetAll();
    public Product? SearchProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;
        return _productRepository.GetByName(name);
    }
    public bool DeleteProduct(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;
        return  _productRepository.Delete(name);
    }
    public bool UpdateProduct(string oldName, string newName, decimal price, int quantity)
    {
        ValidateInputs(newName, price, quantity);
        var existing = _productRepository.GetByName(oldName);
        if (existing is null)
            return false;
        var updatedProduct = new Product(newName, price, quantity);
        return _productRepository.Update(oldName, updatedProduct);
    }
    private void ValidateInputs(string name,  decimal price, int quantity)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.");
        if(name.Length < 2)
            throw new ArgumentException("Product name must be at least 2 characters long.");
        if(price <= 0)
            throw  new ArgumentException("Product price must be greater than zero.");
        if(quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");
    }
}