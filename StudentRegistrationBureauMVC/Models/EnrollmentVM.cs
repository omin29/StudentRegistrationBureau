using Data.Entities;
using System.Diagnostics.CodeAnalysis;

namespace StudentRegistrationBureauMVC.Models
{
    public class EnrollmentVM : BaseVM
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; } 

        public int StudentId { get; set; }
        public string StudentFullName { get; set; } 

        public virtual Student? Student { get; set; }
        public virtual Course? Course { get; set; }
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