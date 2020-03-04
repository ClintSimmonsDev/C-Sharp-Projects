using CarInsuranceQuoteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsuranceQuoteMVC.ViewModels;

namespace CarInsuranceQuoteMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsuranceQuoteCustomer(string firstName, string lastName, string emailAddress, DateTime dateOfBirth, int? carYear, string carMake,
                                        string carModel, bool everDUI, int? speedingTickets, string fullCoverageOrLiability)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress)
                || dateOfBirth == null || (carYear ?? 0) == 0 || string.IsNullOrEmpty(carMake) || string.IsNullOrEmpty(carModel)
                || everDUI.ToString() == null || string.IsNullOrEmpty(fullCoverageOrLiability) || speedingTickets == null)
            {
                return View("~/Views/Shared/Error.cshtml");

            }
            else
            {
                using (CarInsuranceQuotesEntities carQuoteDb = new CarInsuranceQuotesEntities())
                {
                    var carQuote = new InsuranceQuoteCustomer();
                    carQuote.FirstName = firstName;
                    carQuote.LastName = lastName;
                    carQuote.EmailAddress = emailAddress;
                    carQuote.DateOfBirth = dateOfBirth;
                    carQuote.CarYear = carYear;
                    carQuote.CarMake = carMake;
                    carQuote.CarModel = carModel;
                    carQuote.EverDUI = everDUI;
                    carQuote.SpeedingTickets = speedingTickets;
                    carQuote.FullCoverageOrLiability = fullCoverageOrLiability;

                    decimal baseQuote = 50m;

                    int speedingPenalty = (int)speedingTickets * 10;
                    baseQuote += speedingPenalty;

                    var today = DateTime.Today;
                    var age = today.Year - dateOfBirth.Year;

                    string coverage = fullCoverageOrLiability.ToLower();
                    string make = carMake.ToLower();
                    string model = carModel.ToLower();

                    if (age < 25 || age > 100 || carYear < 2000 || carYear >= 2015)
                    {
                        baseQuote += 25;
                    }
                    if (age < 18)
                    {
                        baseQuote += 100;
                    }
                    if (make == "porsche")
                    {
                        baseQuote += 25;
                    }
                    if (make == "Porsche" && model == "911 Carrera")
                    {
                        baseQuote += 25;
                    }
                    if (everDUI == true)
                    {
                        baseQuote *= 1.25m;
                    }
                    if (fullCoverageOrLiability == "full" || fullCoverageOrLiability == "full coverage")
                    {
                        baseQuote *= 1.5m;
                    }
                    carQuote.FinalQuote = (int)baseQuote;
                    carQuoteDb.InsuranceQuoteCustomers.Add(carQuote);
                    CarInsuranceVm CarVm = new CarInsuranceVm();
                    CarVm.FinalQuote = (int)baseQuote;
                    carQuoteDb.SaveChanges();
                    return View("SuccessQuote");
                }
            }
        }
    }
}