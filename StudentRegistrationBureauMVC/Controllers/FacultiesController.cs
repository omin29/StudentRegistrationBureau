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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FacultyVM facultyVM)
        {
            if (ModelState.IsValid)
            {
                Faculty createdFaculty = facultyVM.ToEntity();
                createdFaculty.Id = 0;
                _facultyService.Save(createdFaculty);

                return RedirectToAction(nameof(Index));
            }
            return View(facultyVM);
        }

        // GET: Faculties/Edit/5
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
                    _facultyService.Save(editedFaculty);
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
                return RedirectToAction(nameof(Index));
            }
            return View(facultyVM);
        }

        // GET: Faculties/Delete/5
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
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var faculty = _facultyService.GetById(id);
            if (faculty != null)
            {
                _facultyService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
