using Data.Entities;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public class FacultyManagementService : BaseService
    {
        public IEnumerable<Faculty> Get(int page, int itemsPerPage)
        {
            List<Faculty> facultiesList = new List<Faculty>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                //Func for ordering faculties alphabetically
                Func<IQueryable<Faculty>, IOrderedQueryable<Faculty>> orderBy =
                    q => q.OrderBy(faculty => faculty.Name);
                IEnumerable<Faculty> faculties = unitOfWork.FacultyRepository.Get(orderBy: orderBy);

                //Applying pagination
                if(ValidatePaginationOptions(page, itemsPerPage) && page <= GetPageCount(itemsPerPage, faculties))
                {
                    faculties = faculties.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    //Returning empty list when accessing non-existent page
                    return facultiesList;
                }

                facultiesList = faculties.ToList();
            }

            return facultiesList;
        }

        public Faculty? GetById(int id)
        {
            Faculty? faculty = null;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                faculty = unitOfWork.FacultyRepository.GetByID(id);
            }

            return faculty;
        }

        public bool Exists(int id)
        {
            return GetById(id) != null;
        }

        public bool Save(Faculty faculty)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if(faculty.Id == 0)
                    {
                        unitOfWork.FacultyRepository.Insert(faculty);
                    }
                    else
                    {
                        unitOfWork.FacultyRepository.Update(faculty);
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
                    Faculty faculty = unitOfWork.FacultyRepository.GetByID(id);
                    unitOfWork.FacultyRepository.Delete(faculty);
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
                pageCount = (int)Math.Ceiling((double)unitOfWork.FacultyRepository.Count() / itemsPerPage);
            }

            return pageCount;
        }
    }
}
