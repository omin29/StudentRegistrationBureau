using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.Context;
using Data.Entities;
using ApplicationService.Implementations;
using StudentRegistrationBureauMVC.Models.IndexVMs;
using StudentRegistrationBureauMVC.Models;
using StudentRegistrationBureauMVC.ActionFilters;

namespace StudentRegistrationBureauMVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly CourseManagementService _courseService;
        public CoursesController(CourseManagementService courseService)
        {
            _courseService = courseService;
        }

        // GET: Courses
        public IActionResult Index(CourseIndexVM model)
        {
            IEnumerable<Course> courses = _courseService.Get(model.Pager.Page, model.Pager.ItemsPerPage);
            model.Courses = courses.Select(course => new CourseVM(course));
            model.Pager.PagesCount = _courseService.GetPageCount(model.Pager.ItemsPerPage);
            return View(model);
        }

        // GET: Courses/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _courseService.GetById(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            return View(new CourseVM(course));
        }

        // GET: Courses/Create
        [Authenticated]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
       
        [HttpPost]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseVM courseVM)
        {
            if (ModelState.IsValid)
            {
                Course createdCourse = courseVM.ToEntity();
                createdCourse.Id = 0;
                bool isSuccessful = _courseService.Save(createdCourse);
                if (isSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewData["ErrorMessage"] =
                    "Failed to create course! Ensure that the course is unique and try again.";
            }
            return View(courseVM);
        }

        // GET: Courses/Edit/5

        [HttpGet]
        [Authenticated]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _courseService.GetById(id.Value);
            if (course == null)
            {
                return NotFound();
            }
            return View(new CourseVM(course));
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [Authenticated]
        public IActionResult Edit(int id, CourseVM courseVM)
        {
            if (id != courseVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Course editedCourse = courseVM.ToEntity();
                    bool isSuccessful = _courseService.Save(editedCourse);
                    if (isSuccessful)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_courseService.Exists(courseVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ErrorMessage"] =
                    "Failed to edit course! Ensure that the course is unique and try again.";
            }
            return View(courseVM);
        }

        // GET: Courses/Delete/5
        [Authenticated]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _courseService.GetById(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            return View(new CourseVM(course));
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _courseService.GetById(id);
            if (course != null)
            {
                bool isSuccessful = _courseService.Delete(id);
                if (!isSuccessful)
                {
                    ViewData["ErrorMessage"] =
                        "Failed to delete course! Ensure that the course has no enrollments and try again.";
                    return View(new CourseVM(course));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
