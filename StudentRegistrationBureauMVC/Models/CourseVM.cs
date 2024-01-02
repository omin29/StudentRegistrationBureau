using Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentRegistrationBureauMVC.Models
{
    public class CourseVM : BaseVM
    {
        [Required(ErrorMessage = "Course is required!")]
        [StringLength(100, ErrorMessage = "Course can't be longer than 100 characters!")]
        [Display(Name = "Course")]
        public required string Name { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }

        public CourseVM() { }

        [SetsRequiredMembers]
        public CourseVM(Course course)
        {
            Id = course.Id;
            Name = course.Name;
            Enrollments = course.Enrollments;
        }

        public Course ToEntity()
        {
            Course course = new Course()
            {
                Id = Id,
                Name = Name
            };

            return course;
        }
    }
}
