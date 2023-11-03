using BE_CustomerStore.Data;
using BE_CustomerStore.Modelling;
using BE_CustomerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BE_CustomerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IStore<Order> _orderStore;

        public OrdersController(IStore<Order> orderStore)
        {
            _orderStore = orderStore;
        }

        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var orders = await _orderStore.Get();
        //    return base.Ok(orders.Select(o => new OrderVM(o)));
        //}

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] OrderVM order)
        {
            var newOrder = order.ToDb();
            newOrder.Status = OrderStatus.Created;
            newOrder.AddComment("Order Created: " + DateTime.UtcNow);
            var id = await _orderStore.Add(newOrder);

            return Ok(id);
        }
    }
}
