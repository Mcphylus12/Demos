using BE_CustomerStore.Data;

namespace BE_CustomerStore.Modelling
{
    public class Category : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Product> Products { get; set; }
    }
}
