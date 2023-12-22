using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Enrollment : BaseEntity
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [ForeignKey(nameof(CourseId))]
        public required virtual Course Course { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey(nameof(StudentId))]
        public required virtual Student Student { get; set; }

        [Range(2, 6)]
        public int? Grade { get; set; }
    }
}
