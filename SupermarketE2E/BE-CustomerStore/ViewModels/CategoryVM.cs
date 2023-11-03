using BE_CustomerStore.Modelling;

namespace BE_CustomerStore.ViewModels;

public class CategoryVM
{
    public CategoryVM()
    {

    }

    public CategoryVM(Category c)
    {
        this.Id = c.Id;
        this.Name = c.Name;
        this.Description = c.Description;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    internal Category ToDB() => new Category
    {
        Id = Id,
        Name = Name,
        Description = Description
    };
}
