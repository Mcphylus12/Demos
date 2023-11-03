using BE_CustomerStore.Data;

namespace BE_CustomerStore.Modelling
{
    public class Product : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PriceUnit Unit { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }

        public Category Category { get; set; }
        public Guid CategoryId { get; set; }

        public IList<Order> Orders { get; set; }
        public IList<ProductOrder> ProductOrders { get; set; }
    }
}
