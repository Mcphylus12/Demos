using BE_CustomerStore.Modelling;

namespace BE_CustomerStore.ViewModels;

public class OrderVM
{
    public OrderVM()
    {

    }

    public Guid Id { get; set; }
    public string DeliveryInformation { get; set; }
    public Guid Product { get; set; }
    public int Amount { get; set; }

    internal Order ToDb() => new()
    {
        Id = Id,
        DeliveryInformation = DeliveryInformation,
        ProductOrders = new List<ProductOrder>
        {
            new ProductOrder
            {
                OrderId = Id,
                ProductId = Product,
                Amount = Amount
            }
        }
    };
}
