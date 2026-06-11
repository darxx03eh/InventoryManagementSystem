using InventoryManagementSystem.Models.Entities;
namespace InventoryManagementSystem.Infrastructure.IRepositories;

public interface IProductRepository
{
    public bool Add(Product product);
    public bool Exists(string name);
    public Product? GetByName(string name);
    public IReadOnlyCollection<Product> GetAll();
    public bool Update(string oldName, Product updatedProduct);
    public bool Delete(string name);
}