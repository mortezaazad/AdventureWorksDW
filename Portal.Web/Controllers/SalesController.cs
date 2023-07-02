using Microsoft.AspNetCore.Mvc;
using Portal.Data;
using Portal.Data.Models;
using System.Linq.Expressions;

namespace Portal.Web.Controllers
{
    public static class ResellerExtensions
    {
        public static IQueryable<DimReseller> GetLargeResellers(this IQueryable<DimReseller> resellers)
        {
            return resellers.Where(x => x.NumberEmployees > 80);
        }
    }
    public class SalesController : Controller
    {
        private readonly AdventureWorksDbContext _db;

        public SalesController(AdventureWorksDbContext db)
        {
            _db = db;
        }

        [Route("api/resellers")]
        public IActionResult GetLargeSellers()
        {
            return Ok(
                _db.DimResellers
                .Where(CheckLargeReseller)
                .Count()
                ) ;
        }

        [Route("api/resellers/lastOrders")]
        public IActionResult GetLargeSellersLastOrders()
        {
            return Ok(
                _db.DimResellers
                //.Where(IsLargeReseller)
                .GetLargeResellers()
                .Select(r=>new
                {
                    r.ResellerName,
                    r.LastOrderYear
                }
                ));
        }

        private Expression<Func<DimReseller,bool>> IsLargeReseller
        {
            get { return r => r.NumberEmployees > 80; }
        }
        private bool CheckLargeReseller(DimReseller reseller)
        {
            return reseller.NumberEmployees > 50;
        }
    }
}
