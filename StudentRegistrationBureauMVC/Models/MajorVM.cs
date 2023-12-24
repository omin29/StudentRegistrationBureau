using Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace StudentRegistrationBureauMVC.Models
{
    public class MajorVM : BaseVM
    {
        [Required(ErrorMessage = "Major is required!")]
        [StringLength(100, ErrorMessage = "Major can't be longer than 100 characters!")]
        [Display(Name = "Major")]
        public required string Name { get; set; }

        public virtual ICollection<Student>? Students { get; set; }

        public MajorVM() { }

        [SetsRequiredMembers]
        public MajorVM(Major major)
        {
            Id = major.Id;
            Name = major.Name;
            Students = major.Students;
        }

        public Major ToEntity()
        {
            Major major = new Major()
            {
                Id = Id,
                Name = Name
            };

            return major;
        }
    }
}
