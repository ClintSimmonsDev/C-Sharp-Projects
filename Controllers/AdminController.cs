using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsuranceQuoteMVC.Models;
using CarInsuranceQuoteMVC.ViewModels;

namespace CarInsuranceQuoteMVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CarInsuranceQuotesEntities db = new CarInsuranceQuotesEntities())
            {
                var quotes = db.InsuranceQuoteCustomers;
                var quoteVms = new List<CarInsuranceVm>();
                foreach (var quote in quotes)
                {
                    var CarInsuranceVm = new CarInsuranceVm();
                    //CarInsuranceVm.Id = quote.Id;
                    CarInsuranceVm.FirstName = quote.FirstName;
                    CarInsuranceVm.LastName = quote.LastName;
                    CarInsuranceVm.EmailAddress = quote.EmailAddress;
                    CarInsuranceVm.FinalQuote = (int)quote.FinalQuote;
                    quoteVms.Add(CarInsuranceVm);
                }
                return View(quoteVms);
            }
        }
    }
}