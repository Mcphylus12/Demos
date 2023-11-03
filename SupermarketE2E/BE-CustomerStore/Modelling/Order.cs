using BE_CustomerStore.Data;

namespace BE_CustomerStore.Modelling
{
    public class Order : IEntity
    {
        public Guid Id { get; set; }
        public string DeliveryInformation { get; set; }
        public OrderStatus Status { get; set; }
        public string Comments { get; set; }

        public void AddComment(string comment)
        {
            Comments += $"{comment}{Environment.NewLine}";
        }

        public IList<Product> Products { get; set; }
        public IList<ProductOrder> ProductOrders { get; set; }
    }
}
