using ApplicationService.Implementations;
using Data.Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NuGet.Configuration;
using StudentRegistrationBureauMVC.Models.IndexVMs;
using StudentRegistrationBureauMVC.Models;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using StudentRegistrationBureauMVC.Models.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistrationBureauMVC.ActionFilters;
using Microsoft.EntityFrameworkCore;

public class EnrollmentsController : Controller
{
    private readonly EnrollmentManagementService _enrollmentService;
    private readonly CourseManagementService _courseService;
    private readonly StudentManagementService _studentService;

    public EnrollmentsController(EnrollmentManagementService enrollmentService, CourseManagementService courseService, StudentManagementService studentService)
    {
        _enrollmentService = enrollmentService;
        _courseService = courseService;
        _studentService = studentService;
    }

    // GET: Students
    public IActionResult Index(EnrollmentIndexVM model)
    {
        IEnumerable<Enrollment> enrollments = _enrollmentService.Get(model.Pager.Page, model.Pager.ItemsPerPage, model.Filter);
        model.Enrollments = enrollments.Select(enrollment => new EnrollmentVM(enrollment));
        model.Pager.PagesCount = _enrollmentService.GetPageCount(model.Pager.ItemsPerPage, model.Filter);

        List<Course> courses = _courseService.Get(1, int.MaxValue).ToList();
        courses.Insert(0, new Course() { Id = 0, Name = string.Empty });
        ViewData["Courses"] = new SelectList(courses, "Name", "Name", model.Filter.Course);

        return View(model);
    }

    // GET: Enrollment/Delete/5
    [Authenticated]
    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = _enrollmentService.GetById(id.Value);
        if (enrollment == null)
        {
           return NotFound();
        }

        var viewModel = new EnrollmentVM(enrollment);
        return View(viewModel);
    }

    // POST: Students/Delete/5
    [HttpPost, ActionName("Delete")]
    [Authenticated]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var enrollment = _enrollmentService.GetById(id);
        if (enrollment != null)
        {
            bool isSuccessful = _enrollmentService.Delete(id);
            if (!isSuccessful)
            {
                ViewData["ErrorMessage"] =
                    "Failed to delete enrollment!";
                return View(new EnrollmentVM(enrollment));
            }
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = _enrollmentService.GetById(id.Value);
        if (enrollment == null)
        {
            return NotFound();
        }

        return View(new EnrollmentVM(enrollment));
    }

    [Authenticated]
    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var enrollment = _enrollmentService.GetById(id.Value);
        if (enrollment == null)
        {
            return NotFound();
        }
        //Getting all faculties and majors for making drop-down lists
        return View(new EnrollmentVM(enrollment));
    }

    [HttpPost]
    [Authenticated]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, EnrollmentVM enrollmentVM)
    {
        if (id != enrollmentVM.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                Enrollment editedEnrollment = enrollmentVM.ToEntity();
                bool isSuccessful = _enrollmentService.Save(editedEnrollment);
                if (isSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_enrollmentService.Exists(enrollmentVM.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ViewData["ErrorMessage"] =
                "Failed to edit enrollment!";
        }

        return View(enrollmentVM);
    }

    [Authenticated]
    public IActionResult Create()
    {
        // Getting all faculties and majors for making drop-down lists
        ViewData["StudentId"] = new SelectList(_studentService.Get(1, int.MaxValue)
         .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.FirstName} {s.MiddleName} {s.LastName} - {s.FacultyNumber}" }), "Value", "Text");

        ViewData["CourseId"] = new SelectList(_courseService.Get(1, int.MaxValue), "Id", "Name");
        return View();
    }

    // POST: Students/Create
    [HttpPost]
    [Authenticated]
    [ValidateAntiForgeryToken]
    public IActionResult Create(EnrollmentVM enrollmentVM)
    {
        if (ModelState.IsValid)
        {
            Enrollment createdEnrollment = enrollmentVM.ToEntity();
            createdEnrollment.Id = 0;
            bool isSuccessful = _enrollmentService.Save(createdEnrollment);
            if (isSuccessful)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["ErrorMessage"] =
                "Failed to create enrollment! The student is already enrolled in the specified Course.";
        }

        // Getting all faculties and majors for making drop-down lists
        ViewData["StudentId"] = new SelectList(_studentService.Get(1, int.MaxValue)
         .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = $"{s.FirstName} {s.MiddleName} {s.LastName} - {s.FacultyNumber}" }), "Value", "Text");

        ViewData["CourseId"] = new SelectList(_courseService.Get(1, int.MaxValue), "Id", "Name");
        return View(enrollmentVM);
    }
}