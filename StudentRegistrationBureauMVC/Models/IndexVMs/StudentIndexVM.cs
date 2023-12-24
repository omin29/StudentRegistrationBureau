using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class StudentIndexVM : IViewModelWithPagination
    {
        public IEnumerable<StudentVM>? Students { get; set; }
        public PagerVM Pager { get; set; } = new PagerVM();
    }
}
