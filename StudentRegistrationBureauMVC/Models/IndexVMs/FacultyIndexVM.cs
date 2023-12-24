using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class FacultyIndexVM : IViewModelWithPagination
    {
        public IEnumerable<FacultyVM>? Faculties { get; set; }
        public PagerVM Pager { get; set; } = new PagerVM();
    }
}
