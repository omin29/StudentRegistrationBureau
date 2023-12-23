using StudentRegistrationBureauMVC.Models.Interfaces;
using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.IndexVMs
{
    public class MajorIndexVM : IViewModelWithPagination
    {
        public IEnumerable<MajorVM>? Majors { get; set; }
        public PagerVM Pager { get; set; } = new PagerVM();
    }
}
