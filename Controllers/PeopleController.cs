using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhoneBookApp1.Models;

namespace PhoneBookApp1.Controllers
{
    [Authorize]
    public class PeopleController : Controller
    {
        private PhoneBookDbEntities1 db = new PhoneBookDbEntities1();

        // GET: People
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            //var people = db.People.Include(p => p.AspNetUser);
            var people = from r in db.People where r.AspNetUser.Id == currentUserId select r;
            ViewBag.PersonCount = people.ToList().Count.ToString();
            return View(people.OrderBy(x=>x.FirstName).ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            TempData[User.Identity.GetUserId()] = person.PersonId;
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", User.Identity.GetUserId());
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,FirstName,MiddleName,LastName,DateOfBirth,AddedOn,AddedBy,HomeAddress,HomeCity,FaceBookAccountId,LinkedInId,UpdateOn,ImagePath,TwitterId,EmailId")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", person.AddedBy);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", person.AddedBy);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,FirstName,MiddleName,LastName,DateOfBirth,AddedOn,AddedBy,HomeAddress,HomeCity,FaceBookAccountId,LinkedInId,UpdateOn,ImagePath,TwitterId,EmailId")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AddedBy = new SelectList(db.AspNetUsers, "Id", "Email", person.AddedBy);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            var contactlist = db.Contacts.Where(c => c.PersonId == id);
            ViewBag.CountDeleteContact = contactlist.Count();
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.People.Find(id);
            var contactlist = db.Contacts.Where(c => c.PersonId == id);
            foreach (Contact contact in contactlist)
            {
                db.Contacts.Remove(contact);
            }
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
