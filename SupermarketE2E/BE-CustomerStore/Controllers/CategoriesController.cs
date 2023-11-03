using BE_CustomerStore.Data;
using BE_CustomerStore.Modelling;
using BE_CustomerStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BE_CustomerStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStore<Category> _categoryStore;

        public CategoriesController(IStore<Category> categoryStore)
        {
            _categoryStore = categoryStore;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryStore.Get();
            return base.Ok(categories.Select(c => new CategoryVM(c)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryVM category)
        {
            var id = await _categoryStore.Add(category.ToDB());

            return Ok(id);
        }
    }
}
