using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsurance_MVC.Models;

namespace CarInsurance_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateQuote(string firstName, string lastName, int dobMonth, int dobDay, int dobYear,
            string emailAddress, string dui, string fullCoverage, string carMake, string carModel, int numberOfTickets = 0, int carYear = 2020)
        {
            bool hasDUI = !(dui is null);      // if string dui is null, not checked ==> No DUI, false
            bool wantsFullCoverage = !(fullCoverage is null);
            DateTime userAge = new DateTime(dobYear, dobMonth, dobDay);

            decimal quote = 50;

            // If the user is under 25, add $25 to the monthly total.
            if ((DateTime.Now - userAge).TotalDays / 365 < 25) quote += 25;

            // If the user is under 18, add $100 to the monthly total.
            if ((DateTime.Now - userAge).TotalDays / 365 < 18) quote += 100;

            // If the user is over 100, add $25 to the monthly total.
            if ((DateTime.Now - userAge).TotalDays / 365 > 100) quote += 25;

            // If the car's year is before 2000, add $25 to the monthly total.
            if (carYear < 2000) quote += 25;

            // If the car's year is after 2015, add $25 to the monthly total.
            if (carYear > 2015) quote += 25;

            // If the car's Make is a Porsche, add $25 to the price.
            if (carMake == "Porsche") quote += 25;

            // If the car's Make is a Porsche and its model is a 911 Carrera, add an additional $25 to the price.
            if (carMake == "Porsche" && carModel == "911 Carrera") quote += 25;

            // Add $10 to the monthly total for every speeding ticket the user has.
            if (numberOfTickets > 0) quote += numberOfTickets * 10;

            // If the user has ever had a DUI, add 25% to the total.
            if (hasDUI) quote *= 1.25M;

            // If it's full coverage, add 50% to the total.
            if (wantsFullCoverage) quote *= 1.50M;

            using (CarInsuranceEntities1 db = new CarInsuranceEntities1())
            {
                var user = new User();
                user.FirstName = firstName;
                user.LastName = lastName;
                user.EmailAddress = emailAddress;
                user.DateOfBirth = userAge;
                user.CarYear = carYear;
                user.CarModel = carModel;
                user.CarMake = carMake;
                user.DUI = hasDUI;
                user.SpeedingTickets = numberOfTickets;
                user.FullCoverage = wantsFullCoverage;
                user.Quote = quote;

                db.Users.Add(user);
                db.SaveChanges();
            }

            ViewBag.CarYear = carYear;
            ViewBag.CarModel = carModel;
            ViewBag.CarMake = carMake;
            ViewBag.Quote = quote;
            return View();
        }
    }
}