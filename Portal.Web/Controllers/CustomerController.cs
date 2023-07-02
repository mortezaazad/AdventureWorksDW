﻿using Microsoft.AspNetCore.Mvc;
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

        public CustomerController(AdventureWorksDbContext db)
        {
            _db = db;
        }


        //[Route("api/customer")]
        [Route("Customers")]
        public IActionResult Index()
        {
            var q1= _db.DimCustomers
                .Select(c=>new CustomerData{
                    FirstName=c.FirstName,
                    LastName=c.LastName,
                    EmailAddress=c.EmailAddress
                })
                .Take(100);

            var q2 = q1.Where(c => c.FirstName.StartsWith("A"));
            var q3 = q2.Where(c => c.LastName.StartsWith("B"))
                .ToList();
            return View(q3);
        }
    }
}
