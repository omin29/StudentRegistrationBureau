using ApplicationService.FilterBuilders;
using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class EnrollmentIndexVM : IViewModelWithPagination
    {
        public IEnumerable<StudentVM>? Students { get; set; }
        public IEnumerable<CourseVM>? Courses { get; set; }

        public PagerVM Pager { get; set; } = new PagerVM();

        public StudentFilterBuilder Filter { get; set; } = new StudentFilterBuilder();
    }
}
