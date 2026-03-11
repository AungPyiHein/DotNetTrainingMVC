using Microsoft.AspNetCore.Mvc;
using NorthwindCharts.MVC.Dtos;
using NorthwindCharts.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace NorthwindCharts.MVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetBestSellingProducts()
        {
            var SqlQuery = @"
                    SELECT TOP 5
                    P.ProductName,
                    SUM(OD.Quantity) AS TotalQuantity
                FROM
                    [Order Details] OD
                JOIN
                    Products P ON OD.ProductID = P.ProductID
                GROUP BY
                    P.ProductName
                ORDER BY
                    TotalQuantity DESC";

            var topProducts = await _context.Database
                                      .SqlQueryRaw<BestSellingProductDto>(SqlQuery)
                                      .ToListAsync();
            return Json(topProducts);
        }
    }
}
