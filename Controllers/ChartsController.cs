using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store444.Contexts;
using Store444.Dopomoga;
namespace Store444.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DrugShopContext _context;

        public ChartsController(DrugShopContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var products = _context.Products.ToList();
            var lst = new List<object>
            {
                new[] { "Products", "Product count" }
            };
            foreach (var c in products)
            {
                lst.Add(new object[] { c.RelaiseFromAndDosing, products.Count() });
            }
            return new JsonResult(lst);
        }
    }
}
