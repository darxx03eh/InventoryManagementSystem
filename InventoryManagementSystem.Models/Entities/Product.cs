namespace InventoryManagementSystem.Models.Entities;

public class Product
{
    public string Name
    {
        get => field;
        set
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Product name cannot be empty.");
            if(value.Length < 2)
                throw new ArgumentException("Product name must be at least 2 characters long.");
            field = value.Trim();
        }
    } = string.Empty;

    public decimal Price
    {
        get => field;
        set
        {
            if(value <= 0)
                throw  new ArgumentException("Product price must be greater than zero.");
            field = value;
        }
    }

    public int Quantity
    {
        get => field;
        set
        {
            if(value < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            field = value;
        }
    }
    public Product(){}
    public Product(string name, decimal price, int quantity) => (Name, Price, Quantity) = (name, price, quantity);
    public override string ToString() => $"{Name} | {Price:F2} | {Quantity}";
}