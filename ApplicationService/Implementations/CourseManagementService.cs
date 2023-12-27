using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public class CourseManagementService : BaseService
    {
        public IEnumerable<Course> Get(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        //TODO: Implement the other methods

        public int GetPageCount(int itemsPerPage)
        {
            throw new NotImplementedException();
        }
    }
}
