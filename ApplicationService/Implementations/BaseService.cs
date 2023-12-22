using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public abstract class BaseService
    {
        protected bool ValidatePaginationOptions(int page, int itemsPerPage)
        {
			try
			{
                if (page <= 0)
                {
                    throw new ArgumentException("Invalid page argument! Page must be positive number!", nameof(page));
                }

                if (itemsPerPage <= 0)
                {
                    throw new ArgumentException(
                        "Invalid items per page argument! The number of items per page must be positive", nameof(itemsPerPage));
                }

                return true;
            }
			catch
			{
                return false;
			}
        }

        public int GetPageCount(int itemsPerPage, IEnumerable<object> entities)
        {
            if (itemsPerPage <= 0)
            {
                return 1;
            }

            int pageCount = (int)Math.Ceiling((double)entities.Count() / itemsPerPage);
            return pageCount;
        }
    }
}
