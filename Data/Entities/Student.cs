using Data.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Student : BaseEntity
    {
        [Required]
        [StringLength(50)]
        [LettersOnly]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [LettersOnly]
        public required string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [LettersOnly]
        public required string LastName { get; set; }

        [Required]
        [DigitsOnly]
        [StringLength(maximumLength:10,
            ErrorMessage = "Faculty number must consist of 10 digits!", MinimumLength = 10)]
        public required string FacultyNumber { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty? Faculty { get; set; }

        [Required]
        public int MajorId { get; set; }

        [ForeignKey(nameof(MajorId))]
        public virtual Major? Major { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}
