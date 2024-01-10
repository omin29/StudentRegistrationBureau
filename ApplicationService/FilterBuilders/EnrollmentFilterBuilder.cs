using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.FilterBuilders
{
    public class EnrollmentFilterBuilder : IFilterBuilder<Enrollment>
    {

        [Display(Name = "Faculty number")]
        public string? FacultyNumber { get; set; }

        [Display(Name = "Course")]
        public string? Course { get; set; }

        public Expression<Func<Enrollment, bool>> BuildFilter()
        {
            Expression<Func<Enrollment, bool>> filter =
                (enrollment =>
                (!string.IsNullOrEmpty(FacultyNumber) ? enrollment.Student!.FacultyNumber.Equals(FacultyNumber) : true) &&
                (!string.IsNullOrEmpty(Course) ? enrollment.Course!.Name.Equals(Course) : true));

            return filter;
        }
    }
}
