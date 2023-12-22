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
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(maximumLength:10, ErrorMessage = "Invalid faculty number length!", MinimumLength = 10)]
        public required string FacultyNumber { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        [ForeignKey(nameof(FacultyId))]
        public required virtual Faculty Faculty { get; set; }

        [Required]
        public int MajorId { get; set; }

        [Required]
        [ForeignKey(nameof(MajorId))]
        public required virtual Major Major { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}
