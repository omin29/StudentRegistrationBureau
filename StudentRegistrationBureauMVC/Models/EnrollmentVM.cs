﻿using Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentRegistrationBureauMVC.Models
{
    public class EnrollmentVM : BaseVM
    {
        [Required(ErrorMessage = "Course is required!")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Display(Name = "Course")]
        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Student is required!")]
        [Display(Name = "Student")]
        public int StudentId { get; set; }

        [Display(Name = "Student")]
        public string? StudentFullName { get; set; }

        [Display(Name = "Faculty number")]
        public string? FacultyNumber
        {
            get { return Student?.FacultyNumber; }
        }

        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }

        [Range(2, 6)]
        public int? Grade { get; set; }

        public EnrollmentVM() { }


        [SetsRequiredMembers]
        public EnrollmentVM(Enrollment enrollment)
        {
            Id = enrollment.Id;
            CourseId = enrollment.CourseId;
            CourseName = enrollment.Course?.Name; // Handle potential null
            StudentFullName = enrollment.Student?.FirstName + " " + enrollment.Student?.MiddleName + " " + enrollment.Student?.LastName; // Handle potential null
            StudentId = enrollment.StudentId;
            Grade = enrollment.Grade;
            Student = enrollment.Student;
            Course = enrollment.Course;
        }

        public Enrollment ToEntity()
        {
            Enrollment enrollment = new Enrollment()
            {
                Course = Course,
                CourseId = CourseId,
                StudentId = StudentId,
                Id = Id,
                Student = Student,
                Grade = Grade
            };

            return enrollment;
        }

    }
}