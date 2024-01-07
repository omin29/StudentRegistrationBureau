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
using StudentRegistrationBureauMVC.Models;
using StudentRegistrationBureauMVC.Models.IndexVMs;
using StudentRegistrationBureauMVC.ActionFilters;

namespace StudentRegistrationBureauMVC.Controllers
{
    public class FacultiesController : Controller
    {
        private readonly FacultyManagementService _facultyService;

        public FacultiesController(FacultyManagementService facultyService)
        {
            _facultyService = facultyService;
        }

        // GET: Faculties
        public IActionResult Index(FacultyIndexVM model)
        {
            IEnumerable<Faculty> faculties = _facultyService.Get(model.Pager.Page, model.Pager.ItemsPerPage);
            model.Faculties = faculties.Select(faculty => new FacultyVM(faculty));
            model.Pager.PagesCount = _facultyService.GetPageCount(model.Pager.ItemsPerPage);
            return View(model);
        }

        // GET: Faculties/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = _facultyService.GetById(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(new FacultyVM(faculty));
        }

        // GET: Faculties/Create
        [Authenticated]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        [HttpPost]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FacultyVM facultyVM)
        {
            if (ModelState.IsValid)
            {
                Faculty createdFaculty = facultyVM.ToEntity();
                createdFaculty.Id = 0;
                bool isSuccessful = _facultyService.Save(createdFaculty);
                if (isSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewData["ErrorMessage"] =
                    "Failed to create faculty! Ensure that the faculty is unique and try again.";
            }
            return View(facultyVM);
        }

        // GET: Faculties/Edit/5
        [Authenticated]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = _facultyService.GetById(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }
            return View(new FacultyVM(faculty));
        }

        // POST: Faculties/Edit/5
        [HttpPost]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FacultyVM facultyVM)
        {
            if (id != facultyVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Faculty editedFaculty = facultyVM.ToEntity();
                    bool isSuccessful = _facultyService.Save(editedFaculty);
                    if (isSuccessful)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_facultyService.Exists(facultyVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ErrorMessage"] =
                    "Failed to edit faculty! Ensure that the faculty is unique and try again.";
            }
            return View(facultyVM);
        }

        // GET: Faculties/Delete/5
        [Authenticated]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var faculty = _facultyService.GetById(id.Value);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(new FacultyVM(faculty));
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faculty = _facultyService.GetById(id);
            if (faculty != null)
            {
                bool isSuccessful = _facultyService.Delete(id);
                if (!isSuccessful)
                {
                    ViewData["ErrorMessage"] =
                        "Failed to delete faculty! Ensure that the faculty has no students and try again.";
                    return View(new FacultyVM(faculty));
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
