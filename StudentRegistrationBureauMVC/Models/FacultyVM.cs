using Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentRegistrationBureauMVC.Models
{
    public class FacultyVM : BaseVM
    {
        [Required(ErrorMessage = "Faculty is required!")]
        [StringLength(100, ErrorMessage = "Faculty can't be longer than 100 characters!")]
        [Display(Name = "Faculty")]
        public required string Name { get; set; }

        public virtual ICollection<Student>? Students { get; set; }

        public FacultyVM() { }

        [SetsRequiredMembers]
        public FacultyVM(Faculty faculty)
        {
            Id = faculty.Id;
            Name = faculty.Name;
            Students = faculty.Students;
        }

        public Faculty ToEntity()
        {
            Faculty faculty = new Faculty()
            {
                Id = Id,
                Name = Name
            };

            return faculty;
        }
    }
}
