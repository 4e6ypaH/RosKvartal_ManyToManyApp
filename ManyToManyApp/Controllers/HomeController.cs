using ManyToManyApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManyToManyApp.Controllers
{
    public class HomeController : Controller
    {
        private StudentContext db = new StudentContext();

        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }
        
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Courses = db.Cources.ToList();
            return View(student);
        }

        [HttpPost]
        public ActionResult Edit(Student student, int[] selectedCourses)
        {
            Student newStudent = db.Students.Find(student.Id);
            newStudent.Name = student.Name;
            newStudent.Surname = student.Surname;

            newStudent.Courses.Clear();
            if (selectedCourses != null)
            {
                //получаем выбранные курсы
                foreach (var c in db.Cources.Where(co=> selectedCourses.Contains(co.Id)))
                {
                    newStudent.Courses.Add(c);
                }
            }

            db.Entry(newStudent).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create(int id = 0)
        {
            ViewBag.Courses = db.Cources.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Student student, int[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                //получаем выбранные курсы
                foreach (var c in db.Cources.Where(co => selectedCourses.Contains(co.Id)))
                {
                    student.Courses.Add(c);
                }
            }

            db.Students.Add(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id = 0)
        {
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}