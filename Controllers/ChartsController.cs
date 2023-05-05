using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store444.Contexts;

namespace OOPLab1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChartController : ControllerBase
    {

        private DrugShopContext _context;
        public ChartController(DrugShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult JsonShipData()
        {
            var types = _context.ShipTypes.Include(p => p.Orders).ToList();
            List<object> data = new List<object>();
            data.Add(new[] { "Тип доставки", "Кількість замовлень" });
            foreach (var p in types)
            {
                data.Add(new object[] { p.Name, p.Orders.Count });
            }

            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult JsonTypeData()
       {
            var pharmasies = _context.PaymentTypes.Include(p => p.Orders).ToList();
            List<object> data = new List<object>();
            data.Add(new[] { "Тип оплати", "Кількість замовлень" });
            foreach (var p in pharmasies)
            {
                data.Add(new object[] { p.Name, p.Orders.Count });
            }

            return new JsonResult(data);
        }
    }
}
