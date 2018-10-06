using PhoneBookApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace PhoneBookApp1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private PhoneBookDbEntities1 db = new PhoneBookDbEntities1();
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var people = from r in db.People where r.AspNetUser.Id == currentUserId select r;
            List<Person> birthdays = new List<Person> ();
            List<Person> updated = new List<Person>();
            foreach (Person person in people.ToList())
            {
                if((person.DateOfBirth.Value.DayOfYear - DateTime.Today.DayOfYear <= 10) && (person.DateOfBirth.Value.DayOfYear - DateTime.Today.DayOfYear >= 0))
                {
                    birthdays.Add(person);
                }
                if((DateTime.Today.DayOfYear - person.UpdateOn.Value.DayOfYear) <= 7)
                {
                    updated.Add(person);
                }
            }
            ViewBag.PersonCount = people.ToList().Count.ToString();
            ViewBag.Birthdays = birthdays;
            ViewBag.Updated = updated;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}