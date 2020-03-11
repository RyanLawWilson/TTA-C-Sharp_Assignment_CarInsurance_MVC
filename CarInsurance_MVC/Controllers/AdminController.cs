using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarInsurance_MVC.Models;
using CarInsurance_MVC.ViewModels;

namespace CarInsurance_MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            using (CarInsuranceEntities db = new CarInsuranceEntities())
            {
                var users = db.Users;

                var userVMList = new List<UserVM>();

                foreach (var user in users)
                {
                    var userVM = new UserVM();
                    userVM.FirstName = user.FirstName;
                    userVM.LastName = user.LastName;
                    userVM.EmailAddress = user.EmailAddress;
                    userVM.Quote = user.Quote;

                    userVMList.Add(userVM);
                }

                return View(userVMList);
            }
        }
    }
}