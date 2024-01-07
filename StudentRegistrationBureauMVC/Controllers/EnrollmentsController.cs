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

public class EnrollmentsController : Controller
{
    private readonly EnrollmentManagementService _enrollmentService;
    private readonly CourseManagementService _courseService;

    public EnrollmentsController(EnrollmentManagementService enrollmentService, CourseManagementService courseService)
    {
        _enrollmentService = enrollmentService;
        _courseService = courseService;
    }

    // GET: Students
    public IActionResult Index(EnrollmentIndexVM model)
    {
        IEnumerable<Student> students = _enrollmentService.Get(model.Pager.Page, model.Pager.ItemsPerPage, model.Filter);
        model.Students = students.Select(student => new StudentVM(student));
        model.Pager.PagesCount = _enrollmentService.GetPageCount(model.Pager.ItemsPerPage, model.Filter);

        List<Course> courses = _courseService.Get(1, int.MaxValue).ToList();
        courses.Insert(0, new Course() { Id = 0, Name = "-" });
        model.Students = students.Select(student => new StudentVM(student));
        model.Courses = courses.Select(course => new CourseVM(course));
        model.Pager.PagesCount = _enrollmentService.GetPageCount(model.Pager.ItemsPerPage, model.Filter);

       


        //ViewData["Majors"] = new SelectList(majors, "Id", "Name", model.Filter.MajorId);

        return View(model);
    }
}