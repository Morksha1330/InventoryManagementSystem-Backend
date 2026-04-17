using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _database;

        public DashboardController(ApplicationDbContext database)
        {
            _database = database;
        }

        [HttpGet("UserCount")]
        public IActionResult TotalUserCount()
        {                         // take all users count

            var count = _database.Users.Where(x => x.Active == true).ToList().Count;
            return Ok(count);
        }

        [HttpGet("latestUsers")]                                    // take latest users details
        public IActionResult LatestUsers()
        {

            var latest = _database.Users.Where(x => x.Active == true).OrderByDescending(x => x.CreatedDate).Take(5).ToList();
            return Ok(latest);
        }

        [HttpGet("ProductCount")]                                   // take all product count
        public IActionResult TotalProductCount()
        {                         // take all users count

            var count = _database.Products.Where(x => x.Active == true).ToList().Count;
            return Ok(count);
        }

        [HttpGet("CategoryCount")]                                   // take all product count
        public IActionResult TotalCategoryCount()
        {                         // take all users count

            var count = _database.Categories.Where(x => x.Active == true).ToList().Count;
            return Ok(count);
        }

        [HttpGet("customerCount")]
        public IActionResult TotalCustomerCount()                   // take all customers count
        {

            var count = _database.Customers.ToList().Count;
            return Ok(count);
        }


        [HttpGet("totalSales")]
        public IActionResult TotalSales()                   // take total sales
        {

            var count = _database.SalesOrders.Sum(x => x.TotalAmount);
            return Ok(count);
        }

        [HttpGet("totalExpenses")]
        public IActionResult TotalExpenses()                   // take total expenses
        {

            var count = _database.PurchaseOrders.Sum(x => x.TotalAmount);

            return Ok(count);
        }

        [HttpGet("TopSellingProd")]
        public IActionResult TopSellingProd()                   // take top selling product
        {
            var xx = _database.SalesOrderItems
                    .GroupBy(o => o.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        TotalQuantity = g.Sum(x => x.Quantity),
                        
                    })
                    .OrderByDescending(x => x.TotalQuantity)
                    .FirstOrDefault()?.ProductId ?? 0;

            var vv = _database.Products.Where(x => x.Id == xx).Select(x => x.ProductName).ToList();
            return Ok(vv.FirstOrDefault() ?? "No product found");
        }

    }
}
