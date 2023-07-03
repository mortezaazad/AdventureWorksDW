using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal.Data;

namespace Portal.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly AdventureWorksDbContext _db;

        public ReportController(AdventureWorksDbContext db)
        {
            _db = db;
        }

        [Route("sales/territory")]
        public IActionResult Index()
        {
            //Include => شامل مورادی هست که با یک جدول دیگه جوین میشه
            var q = _db.FactResellerSales
                .Include(c => c.SalesTerritoryKeyNavigation)
                .Take(100)
                .Select(k => new { k.SalesAmount, k.SalesTerritoryKeyNavigation.SalesTerritoryCountry })
                .ToList();




            //با استفاده از دستور join
            var q1 = _db.FactResellerSales.Join(_db.DimSalesTerritories,
                f => f.SalesTerritoryKey,
                t => t.SalesTerritoryKey,
                (f, t) => new
                {
                    Sales = f,
                    Ter = t
                }).Select(q =>
                new
                {
                    q.Sales.SalesAmount,
                    q.Ter.SalesTerritoryCountry
                })
                .Take(10)
                .ToList();



            // استفاده از Expresion
            var q2 = from Sales in _db.FactResellerSales
                     join Ter in _db.DimSalesTerritories
                     on Sales.SalesTerritoryKey equals Ter.SalesTerritoryKey
                     select new
                     {
                         Sales.SalesAmount,
                         Ter.SalesTerritoryCountry
                     };

            return Ok(q2.Take(10).ToList());
        }

        [Route("sales/territory/raw")]
        public IActionResult GetTerritory()
        {
            var q = _db.DimSalesTerritories.FromSqlRaw("Select * from DimSalesTerritory");
            return Ok(q.ToList());
        }
    }
}
