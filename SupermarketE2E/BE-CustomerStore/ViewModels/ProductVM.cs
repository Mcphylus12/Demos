using BE_CustomerStore.Modelling;

namespace BE_CustomerStore.ViewModels;

public class ProductVM
{
    public ProductVM()
    {

    }

    public ProductVM(Product p)
    {
        this.Id = p.Id;
        this.CategoryId = p.CategoryId;
        this.Barcode = p.Barcode;
        this.Description = p.Description;
        this.Price = p.Price;
        this.Name = p.Name;
    }

    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Barcode { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }

    internal Product ToDb() => new()
    {
        Name = Name,
        Price = Price,
        Description = Description,
        Barcode = Barcode,
        CategoryId = CategoryId,
        Id = Id
    };
}
