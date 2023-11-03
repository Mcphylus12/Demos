namespace BE_CustomerStore.Modelling
{
    public class ProductOrder
    {
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
        public Product Product { get; set; }
        public Guid ProductId { get; set; }
        public decimal Amount { get; set; }
    }
}
