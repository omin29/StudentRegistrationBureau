using ApplicationService.FilterBuilders;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public class EnrollmentManagementService : BaseService
    {
        public IEnumerable<Enrollment> Get(int page, int itemsPerPage, IFilterBuilder<Enrollment>? filterBuilder = null)
        {
            throw new NotImplementedException();
        }

        //TODO: Implement the other methods

        public int GetPageCount(int itemsPerPage, IFilterBuilder<Enrollment>? filterBuilder = null)
        {
            throw new NotImplementedException();
        }
    }
}
