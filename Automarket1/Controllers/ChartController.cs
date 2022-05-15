using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Automarket1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly laba1Context _context;
        public ChartController(laba1Context context)
        {
            _context = context;
        }
        [HttpGet("JsonDataModels")]
        public JsonResult JsonDataModels()
        {
            var cars = _context.Models.Include(a => a.Cars).ToList();
            List<object> modelCars = new List<object>();
            modelCars.Add(new[] { "Модель", "Кількість моделей машини" });
            foreach(var a in cars)
            {
                modelCars.Add(new object[] {a.Model1, a.Cars.Count });
            }

            return new JsonResult(modelCars);
        }

        [HttpGet("JsonDataCustomers")]
        public JsonResult JsonDataCustomers()
        {
            var sales = _context.Customers.Include(a => a.Sales).ToList();
            List<object> customerSales = new List<object>();
            customerSales.Add(new[] { "Клієнт", "Кількість покупок клієнта" });
            foreach (var a in sales)
            {
                customerSales.Add(new object[] { a.FullName, a.Sales.Count });
            }

            return new JsonResult(customerSales);
        }
        [HttpGet("JsonDataEmployees")]
        
        public JsonResult JsonDataEmployees()
        {
            var curators = _context.Employees.Include(a => a.Curators).ToList();
            List<object> employeeCurator = new List<object>();
            employeeCurator.Add(new[] { "Працівник", "Кількість кураторства" });
            foreach (var a in curators)
            {
                employeeCurator.Add(new object[] { a.FullName, a.Curators.Count });
            }

            return new JsonResult(employeeCurator);
        }
    }


}
