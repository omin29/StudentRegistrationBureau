using StudentRegistrationBureauMVC.Models.Shared;

namespace StudentRegistrationBureauMVC.Models.Interfaces
{
    public interface IViewModelWithPagination
    {
        public PagerVM Pager { get; set; }
    }
}
