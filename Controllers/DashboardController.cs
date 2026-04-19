using InventoryMgtSystem.Data;
using InventoryMgtSystem.DTO;
using InventoryMgtSystem.Models;
using InventoryMgtSystem.Models.Entities;
using InventoryMgtSystem.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMgtSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _database;
        private readonly IDashboardService _service;


        public DashboardController(ApplicationDbContext database, IDashboardService service)
        {
            _database = database;
            _service = service;
        }
        
        //dashboard summery eka ganna
        [HttpGet("summary")]
        [ProducesResponseType(typeof(HttpResponseData<DashboardDtos.DashboardSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(HttpResponseData<DashboardDtos.DashboardSummaryDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSummary(
            [FromQuery] int lowStockThreshold = 10,
            [FromQuery] int recentUserCount   = 5,
            [FromQuery] int topProductCount   = 5,
            [FromQuery] int trendDays         = 7,
            CancellationToken cancellationToken = default)
        {
            var queryParams = new DashboardDtos.DashboardQueryParams
            {
                LowStockThreshold = lowStockThreshold,
                RecentUserCount   = recentUserCount,
                TopProductCount   = topProductCount,
                TrendDays         = trendDays,
            };
 
            var response = await _service.GetDashboardSummaryAsync(queryParams, cancellationToken);
 
            return response.Success
                ? Ok(response)
                : StatusCode(response.ResponsCode, response);
        }

        //[HttpGet("UserCount")]
        //public IActionResult TotalUserCount()
        //{                         

        //    var count = _database.Users.Where(x => x.Active == true).ToList().Count;
        //    return Ok(count);
        //}

        //[HttpGet("latestUsers")]                                    
        //public IActionResult LatestUsers()
        //{

        //    var latest = _database.Users.Where(x => x.Active == true).OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        //    return Ok(latest);
        //}

        //[HttpGet("ProductCount")]                                   
        //public IActionResult TotalProductCount()
        //{                         

        //    var count = _database.Products.Where(x => x.Active == true).ToList().Count;
        //    return Ok(count);
        //}

        //[HttpGet("CategoryCount")]                                   
        //public IActionResult TotalCategoryCount()
        //{                        

        //    var count = _database.Categories.Where(x => x.Active == true).ToList().Count;
        //    return Ok(count);
        //}

        //[HttpGet("customerCount")]
        //public IActionResult TotalCustomerCount()                   
        //{

        //    var count = _database.Customers.ToList().Count;
        //    return Ok(count);
        //}


        //[HttpGet("totalSales")]
        //public IActionResult TotalSales()                   
        //{

        //    var count = _database.SalesOrders.Sum(x => x.TotalAmount);
        //    return Ok(count);
        //}

        //[HttpGet("totalExpenses")]
        //public IActionResult TotalExpenses()                   
        //{

        //    var count = _database.PurchaseOrders.Sum(x => x.TotalAmount);

        //    return Ok(count);
        //}

        //[HttpGet("TopSellingProd")]
        //public IActionResult TopSellingProd()                   // take top selling product
        //{
        //    var xx = _database.SalesOrderItems
        //            .GroupBy(o => o.ProductId)
        //            .Select(g => new
        //            {
        //                ProductId = g.Key,
        //                TotalQuantity = g.Sum(x => x.Quantity),
                        
        //            })
        //            .OrderByDescending(x => x.TotalQuantity)
        //            .FirstOrDefault()?.ProductId ?? 0;

        //    var vv = _database.Products.Where(x => x.Id == xx).Select(x => x.ProductName).ToList();
        //    return Ok(vv.FirstOrDefault() ?? "No product found");
        //}

    }
}
