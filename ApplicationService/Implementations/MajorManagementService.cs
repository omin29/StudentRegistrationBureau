using Data.Entities;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public class MajorManagementService : BaseService
    {
        public IEnumerable<Major> Get(int page, int itemsPerPage)
        {
            List<Major> majorsList = new List<Major>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                //Func for ordering majors alphabetically
                Func<IQueryable<Major>, IOrderedQueryable<Major>> orderBy =
                    q => q.OrderBy(major => major.Name);
                IEnumerable<Major> majors = unitOfWork.MajorRepository.Get(orderBy: orderBy);

                //Applying pagination
                if (ValidatePaginationOptions(page, itemsPerPage) && page <= GetPageCount(itemsPerPage, majors))
                {
                    majors = majors.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    //Returning empty list when accessing non-existent page
                    return majorsList;
                }

                majorsList = majors.ToList();
            }

            return majorsList;
        }

        public Major? GetById(int id)
        {
            Major? major = null;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                major = unitOfWork.MajorRepository.GetByID(id);
            }

            return major;
        }

        public bool Exists(int id)
        {
            return GetById(id) != null;
        }

        public bool Save(Major major)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (major.Id == 0)
                    {
                        unitOfWork.MajorRepository.Insert(major);
                    }
                    else
                    {
                        unitOfWork.MajorRepository.Update(major);
                    }

                    unitOfWork.Save();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    Major major = unitOfWork.MajorRepository.GetByID(id);
                    unitOfWork.MajorRepository.Delete(major);
                    unitOfWork.Save();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int GetPageCount(int itemsPerPage)
        {
            if (itemsPerPage <= 0)
            {
                return 1;
            }

            int pageCount = 0;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                pageCount = (int)Math.Ceiling((double)unitOfWork.MajorRepository.Count() / itemsPerPage);
            }

            return pageCount;
        }
    }
}
