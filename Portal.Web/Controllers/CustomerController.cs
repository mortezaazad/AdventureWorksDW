using Microsoft.AspNetCore.Mvc;
using Portal.Data;

namespace Portal.Web.Controllers
{
    public class CustomerData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
    public class CustomerController : Controller
    {
        private readonly AdventureWorksDbContext _db;
        private readonly ILogger _logger;

        public CustomerController(AdventureWorksDbContext db, ILogger<CustomerController> logger)
        {
            _db = db;
            _logger = logger;
        }


        //[Route("api/customer")]
        [Route("Customers")]
        public IActionResult Index()
        {
            var q1 = _db.DimCustomers
                .Select(c => new CustomerData
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    EmailAddress = c.EmailAddress
                })
                .Take(100);

            var q2 = q1.Where(c => c.FirstName.StartsWith("A"));
            var q3 = q2.Where(c => c.LastName.StartsWith("B"))
                .ToList();
            return View(q3);
        }

        [Route("api/customer/{id}")]
        public IActionResult Get(int id)
        {
            var customer = _db.DimCustomers.Find(id);

            _logger.LogInformation("Logging the second customer with {0}", id);
            var c = _db.DimCustomers.Find(id);
            return Ok(customer);
        }

        [Route("api/customer/take")]
        public IActionResult GetTake(int id)
        {

            //Take & Skip is going to use for pagination
            var customer = _db.DimCustomers
                .OrderBy(g=>g.BirthDate) 
                .Skip(100)                 // صد 100 تا رکورد اول رو فراموش کن و بعد 10 تا رو برداشت کن 
                .Take(10);                 // ده 10 تا رو برداشت کن
            return Ok(customer);
        }


        [Route("api/customer")]
        public IActionResult GetSingle(int id)
        {
            //single =>
            //حتما و حتما باید یک نمونه از اون در دیتابیس وجود داشته باشه. اگر نباشه به خطا میخوره
            //و حتی اگه بیشتر از یکی باشه هم به خطا میخوره
            //var customer = _db.DimCustomers.Single(c => c.FirstName.ToLower() == "david");


            //singleOrDefault => اگر موردی وجود نداشت نال بر میگرداند
            var customer2 = _db.DimCustomers.SingleOrDefault(c => c.FirstName.ToLower() == "david121245");

            //First => اولین آیتمیی که دیدی رو برگردونه
            var customer3 = _db.DimCustomers
                .OrderByDescending(v=>v.BirthDate)
                .First(c => c.FirstName.ToLower() == "david");


            //FirstOrDefault => اولین مورد رو برمیگرداند درغیر اینصورت نال بر میگرداند
            var customer4 = _db.DimCustomers
                .OrderByDescending(v => v.BirthDate)
                .FirstOrDefault(c => c.FirstName.ToLower() == "david");

            //Last => آخرین آیتمیی که دیدی رو برگردونه
            var customer5 = _db.DimCustomers
                .OrderBy(v => v.BirthDate)
                .Last(c => c.FirstName.ToLower() == "david");


            //LastOrDefault => آخرین مورد رو برمیگرداند درغیر اینصورت نال بر میگرداند
            var customer6 = _db.DimCustomers
                .OrderBy(v => v.BirthDate)
                .LastOrDefault(c => c.FirstName.ToLower() == "dana");



            //_logger.LogInformation("Logging the second customer with {0}", id);
            //var c = _db.DimCustomers.Single(c => c.CustomerKey == id);
            return Ok(customer6);
        }

        bool IsLeapYear(int year)
        { return year % 4 == 0;}

        [Route("api/customer/takeLeap")]
        public IActionResult GetTakeLeapYear(int id)
        {
            //در صورتی که تابع رو بصورت زیر بنویسیم در زمان اجرا به خطا میرسیم . چون نمیتواند تابع
            //IsLeapYear رو به دستورات SQL تبدیل کنه
            //به همین خاطر از دستورات زیر میتوانیم استفاده کنیم.
            // 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or 'ToListAsync'
            // این توابع ابتدا کل اطلاعات رو از اس کیو ال واکشی می کنند، سپس روی آن دیتا کوئری می زنه

            //var customer = _db.DimCustomers
            //    .Where(c=>c.BirthDate.HasValue && IsLeapYear(c.BirthDate.Value.Year))
            //    .OrderBy(g => g.BirthDate)
            //    .Take(10);
            //    


            // به صورت زیر تغییر می دهیم تا بتوانیم نتیجه رو دریافت کنیم
            var customer = _db.DimCustomers
                .AsEnumerable()
                .Where(c => c.BirthDate.HasValue && IsLeapYear(c.BirthDate.Value.Year))
                .OrderBy(g => g.BirthDate)
                .Take(10);

            //AsEnumerable() =>  دیتا خط به خطا از اس کیو ال گرفته می شه و شرط درون برنامه روی آن اجرا می شه. حافظه کمتری مورد نیاز است
            //ToList() =>  ابتدا کل اطلاعات از اس کیو ال واکشی میشه و در رم قرار می گیره، سپس شرط درون برنامه روی آن اجرا می شود. برای دیتای بزرگ نیاز به رم زیاد داره و توصیه نمی شود.

            return Ok(customer);
        }
    }
}
