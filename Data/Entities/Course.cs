using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Course : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}
