﻿using System;
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

namespace StudentRegistrationBureauMVC.Controllers
{
    public class MajorsController : Controller
    {
        private readonly MajorManagementService _majorService;

        public MajorsController(MajorManagementService majorService)
        {
            _majorService = majorService;
        }

        // GET: Majors
        public IActionResult Index(MajorIndexVM model)
        {
            IEnumerable<Major> majors = _majorService.Get(model.Pager.Page, model.Pager.ItemsPerPage);
            model.Majors = majors.Select(major => new MajorVM(major));
            model.Pager.PagesCount = _majorService.GetPageCount(model.Pager.ItemsPerPage);
            return View(model);
        }

        // GET: Majors/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = _majorService.GetById(id.Value);
            if (major == null)
            {
                return NotFound();
            }

            return View(new MajorVM(major));
        }

        // GET: Majors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Majors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MajorVM majorVM)
        {
            if (ModelState.IsValid)
            {
                Major createdMajor = majorVM.ToEntity();
                createdMajor.Id = 0;
                _majorService.Save(createdMajor);

                return RedirectToAction(nameof(Index));
            }
            return View(majorVM);
        }

        // GET: Majors/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = _majorService.GetById(id.Value);
            if (major == null)
            {
                return NotFound();
            }
            return View(new MajorVM(major));
        }

        // POST: Majors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MajorVM majorVM)
        {
            if (id != majorVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Major editedMajor = majorVM.ToEntity();
                    _majorService.Save(editedMajor);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_majorService.Exists(majorVM.Id))
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
            return View(majorVM);
        }

        // GET: Majors/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = _majorService.GetById(id.Value);
            if (major == null)
            {
                return NotFound();
            }

            return View(new MajorVM(major));
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var major = _majorService.GetById(id);
            if (major != null)
            {
                _majorService.Delete(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}