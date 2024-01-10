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
using Microsoft.AspNetCore.Authorization;
using StudentRegistrationBureauMVC.ActionFilters;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System.Diagnostics;

namespace StudentRegistrationBureauMVC.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentManagementService _studentService;
        private readonly MajorManagementService _majorService;
        private readonly FacultyManagementService _facultyService;

        public StudentsController(StudentManagementService studentService,
            MajorManagementService majorService, FacultyManagementService facultyService)
        {
            _studentService = studentService;
            _majorService = majorService;
            _facultyService = facultyService;
        }

        // GET: Students
        public IActionResult Index(StudentIndexVM model)
        {
            IEnumerable<Student> students = _studentService.Get(model.Pager.Page, model.Pager.ItemsPerPage, model.Filter);
            model.Students = students.Select(student => new StudentVM(student));
            model.Pager.PagesCount = _studentService.GetPageCount(model.Pager.ItemsPerPage, model.Filter);

            /*Getting all faculties and majors for making drop-down lists which will be used for filtering.
              The newly inserted elements are default values which indicate that no filter is being applied.*/
            List<Faculty> faculties = _facultyService.Get(1, int.MaxValue).ToList();
            faculties.Insert(0, new Faculty() { Id = 0, Name = "-" });
            List<Major> majors = _majorService.Get(1, int.MaxValue).ToList();
            majors.Insert(0, new Major() { Id = 0, Name = "-" });
            ViewData["Faculties"] = new SelectList(faculties, "Id", "Name", model.Filter.FacultyId);
            ViewData["Majors"] = new SelectList(majors, "Id", "Name", model.Filter.MajorId);

            return View(model);
        }

        // GET: Students/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentService.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(new StudentVM(student));
        }

        // GET: Students/Create
        [Authenticated]

        public IActionResult Create()
        {
            //Getting all faculties and majors for making drop-down lists
            ViewData["FacultyId"] = new SelectList(_facultyService.Get(1, int.MaxValue), "Id", "Name");
            ViewData["MajorId"] = new SelectList(_majorService.Get(1, int.MaxValue), "Id", "Name");
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentVM studentVM)
        {
            if (ModelState.IsValid)
            {
                Student createdStudent = studentVM.ToEntity();
                createdStudent.Id = 0;
                bool isSuccessful = _studentService.Save(createdStudent);
                if (isSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }

                ViewData["ErrorMessage"] = 
                    "Failed to create student! Ensure that the faculty number is unique and try again.";
            }
            //Getting all faculties and majors for making drop-down lists
            ViewData["FacultyId"] = new SelectList(_facultyService.Get(1, int.MaxValue), "Id", "Name", studentVM.FacultyId);
            ViewData["MajorId"] = new SelectList(_majorService.Get(1, int.MaxValue), "Id", "Name", studentVM.MajorId);
            return View(studentVM);
        }

        // GET: Students/Edit/5
        [Authenticated]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentService.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            //Getting all faculties and majors for making drop-down lists
            ViewData["FacultyId"] = new SelectList(_facultyService.Get(1, int.MaxValue), "Id", "Name", student.FacultyId);
            ViewData["MajorId"] = new SelectList(_majorService.Get(1, int.MaxValue), "Id", "Name", student.MajorId);
            return View(new StudentVM(student));
        }

        // POST: Students/Edit/5
        [HttpPost]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, StudentVM studentVM)
        {
            if (id != studentVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Student editedStudent = studentVM.ToEntity();
                    bool isSuccessful = _studentService.Save(editedStudent);
                    if (isSuccessful)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_studentService.Exists(studentVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["ErrorMessage"] =
                    "Failed to edit student! Ensure that the faculty number is unique and try again.";
            }
            //Getting all faculties and majors for making drop-down lists
            ViewData["FacultyId"] = new SelectList(_facultyService.Get(1, int.MaxValue), "Id", "Name", studentVM.FacultyId);
            ViewData["MajorId"] = new SelectList(_majorService.Get(1, int.MaxValue), "Id", "Name", studentVM.MajorId);
            return View(studentVM);
        }

        // GET: Students/Delete/5
        [Authenticated]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _studentService.GetById(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            return View(new StudentVM(student));
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authenticated]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _studentService.GetById(id);
            if (student != null)
            {
                bool isSuccessful = _studentService.Delete(id);
                if(!isSuccessful) 
                {
                    ViewData["ErrorMessage"] = 
                        "Failed to delete student! Ensure that the student has no enrollments and try again.";
                    return View(new StudentVM(student));
                }
            }

            return RedirectToAction(nameof(Index));
        }


        [Authenticated]
        public IActionResult ExportAllStudents()
        {
            StudentIndexVM model = new StudentIndexVM();

            model.Pager.ItemsPerPage = int.MaxValue;

            List<Student> students = _studentService.Get(model.Pager.Page, model.Pager.ItemsPerPage, model.Filter).ToList();


            var stream = new MemoryStream();

            using(var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("Students");

                worksheet.Cells["A1"].Value = "First Name";
                worksheet.Cells["B1"].Value = "Middle Name";
                worksheet.Cells["C1"].Value = "Last Name";
                worksheet.Cells["D1"].Value = "Faculty number";
                worksheet.Cells["E1"].Value = "Faculty";
                worksheet.Cells["F1"].Value = "Major";


                var row = 2;
                foreach(var student in students)
                {

                    worksheet.Cells[row, 1].Value = student.FirstName;
                    worksheet.Cells[row, 2].Value = student.MiddleName;
                    worksheet.Cells[row, 3].Value = student.LastName;
                    worksheet.Cells[row, 4].Value = student.FacultyNumber;
                    worksheet.Cells[row, 5].Value = student.Faculty.Name;
                    worksheet.Cells[row, 6].Value = student.Major.Name;

                    row++;
                }

                xlPackage.Workbook.Properties.Title = "Student List";
                xlPackage.Workbook.Properties.Author = "Nikola";

                xlPackage.Save();

            }

            stream.Position = 0;

            return File(stream,"application/vnd.openxmlformats-officedocument.spreadsheethml.sheet", "students.xlsx");
        }

        [HttpGet]
        public IActionResult ImportStudents()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportStudents(IFormFile file)
        {
            if(ModelState.IsValid)
            {
                if (file?.Length > 0)
                {
                    var stream = file.OpenReadStream();

                    List<Student> students = new List<Student>();

                    try
                    {
                        using(var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            for(var row = 1 ; row <= rowCount; row++)
                            {
                                try
                                {
                                    var firstName = worksheet.Cells[row, 1].Value?.ToString();
                                    var middleName = worksheet.Cells[row, 2].Value?.ToString();
                                    var lastName = worksheet.Cells[row, 3].Value?.ToString();
                                    var facultyNumber = worksheet.Cells[row, 4].Value?.ToString();
                                    var facultyId = worksheet.Cells[row, 5].Value;
                                    var majorId = worksheet.Cells[row, 6].Value;

                                    var student = new Student()
                                    {
                                        FirstName = firstName,
                                        MiddleName = middleName,
                                        LastName = middleName,
                                        FacultyNumber = facultyNumber,
                                        Faculty = (Faculty)facultyId,
                                        Major = (Major)majorId
                                    
                                    };

                                    students.Add(student);
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

        
            return View();
        }
    }
}
