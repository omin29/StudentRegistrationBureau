using ApplicationService.FilterBuilders;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class EnrollmentIndexVM : IViewModelWithPagination
    {
        public IEnumerable<EnrollmentVM>? Enrollments { get; set; }

        public PagerVM Pager { get; set; } = new PagerVM();

        public EnrollmentFilterBuilder Filter { get; set; } = new EnrollmentFilterBuilder();
    }
}
