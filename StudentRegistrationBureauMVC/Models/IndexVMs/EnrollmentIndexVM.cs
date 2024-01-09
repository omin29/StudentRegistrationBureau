using ApplicationService.FilterBuilders;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class EnrollmentIndexVM
    {
        public IEnumerable<EnrollmentVM>? Enrollments { get; set; }
        public IEnumerable<StudentVM>? Students { get; set; }
        public IEnumerable<CourseVM>? Courses { get; set; }

        public PagerVM Pager { get; set; } = new PagerVM();

        public StudentFilterBuilder Filter { get; set; } = new StudentFilterBuilder();

        public List<SelectListItem> CourseList { get; set; } = new List<SelectListItem>();
    }
}
