using BE_CustomerStore.Data;
using BE_CustomerStore.Modelling;
using BE_CustomerStore.Queries;
using BE_CustomerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BE_CustomerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IStore<Product> _productStore;

        public ProductsController(IStore<Product> productStore)
        {
            _productStore = productStore;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Guid? category)
        {
            var query = new GetProductsQuery(category);
            var products = await _productStore.Get(query);
            return base.Ok(products.Select(p => new ProductVM(p)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductVM product)
        {
            var id = await _productStore.Add(product.ToDb());

            return Ok(id);
        }
    }
}
