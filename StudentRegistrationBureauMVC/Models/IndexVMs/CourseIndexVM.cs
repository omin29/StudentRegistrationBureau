using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class CourseIndexVM : IViewModelWithPagination
    {
        public IEnumerable<CourseVM>? Courses { get; set; }
        public PagerVM Pager { get; set; } = new PagerVM();
    }
}
