using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Data.ValidationAttributes;

namespace StudentRegistrationBureauMVC.Models
{
    public class StudentVM : BaseVM
    {
        [Required(ErrorMessage = "First name is required!")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters!")]
        [LettersOnly]
        [Display(Name = "First name")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Middle name is required!")]
        [StringLength(50, ErrorMessage = "Middle name can't be longer than 50 characters!")]
        [LettersOnly]
        [Display(Name = "Middle name")]
        public required string MiddleName { get; set; }

        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters!")]
        [LettersOnly]
        [Display(Name = "Last name")]
        public required string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName
        {
            get { return $"{FirstName} {MiddleName} {LastName}"; }
        }

        [Required(ErrorMessage = "Faculty number is required!")]
        [DigitsOnly]
        [StringLength(maximumLength: 10,
            ErrorMessage = "Faculty number must consist of 10 digits!", MinimumLength = 10)]
        [Display(Name = "Faculty number")]
        public required string FacultyNumber { get; set; }

        [Required(ErrorMessage = "Faculty is required!")]
        [Display(Name = "Faculty")]
        public int FacultyId { get; set; }

        public virtual Faculty? Faculty { get; set; }

        [Required(ErrorMessage = "Major is required!")]
        [Display(Name = "Major")]
        public int MajorId { get; set; }

        public virtual Major? Major { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }

        public StudentVM() { }

        [SetsRequiredMembers]
        public StudentVM(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            MiddleName = student.MiddleName;
            LastName = student.LastName;
            FacultyNumber = student.FacultyNumber;
            FacultyId = student.FacultyId;
            Faculty = student.Faculty;
            MajorId = student.MajorId;
            Major = student.Major;
            Enrollments = student.Enrollments;
        }

        public Student ToEntity()
        {
            Student student = new Student()
            {
                Id = Id,
                FirstName = FirstName,
                MiddleName = MiddleName,
                LastName = LastName,
                FacultyNumber = FacultyNumber,
                FacultyId = FacultyId,
                Faculty = Faculty!,
                MajorId = MajorId,
                Major = Major!,
            };

            return student;
        }
    }
}
