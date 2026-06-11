using InventoryManagementSystem.Infrastructure.IRepositories;
using InventoryManagementSystem.Models.Entities;
namespace InventoryManagementSystem.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly Dictionary<string, Product> _products = new(StringComparer.OrdinalIgnoreCase);
    public bool Add(Product product)
    {
        if(product is null)
            throw new ArgumentNullException(nameof(product));
        return _products.TryAdd(product.Name, product);
    }
    public bool Exists(string name) => _products.ContainsKey(name);
    public Product? GetByName(string name)
    {
        _products.TryGetValue(name, out var product);
        return product;
    }
    public IReadOnlyCollection<Product> GetAll() => _products.Values.ToList().AsReadOnly();
    public bool Update(string oldName, Product updatedProduct)
    {
        if (!_products.ContainsKey(oldName))
            return false;
        if (oldName.Equals(updatedProduct.Name, StringComparison.OrdinalIgnoreCase))
        {
            _products[oldName] = updatedProduct;
            return true;
        }
        _products.Remove(oldName);
        _products[updatedProduct.Name] = updatedProduct;
        return true;
    }
    public bool Delete(string name) =>  _products.Remove(name);
}