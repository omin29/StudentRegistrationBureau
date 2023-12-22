using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Major : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        public required virtual ICollection<Student> Students { get; set; }
    }
}
