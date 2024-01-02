using Data.Entities;
using Repository.Implementations;
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
            List<Course> coursesList = new List<Course>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                //Func for ordering courses alphabetically
                Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy =
                    q => q.OrderBy(course => course.Name);
                IEnumerable<Course> courses = unitOfWork.CourseRepository.Get(orderBy: orderBy);

                //Applying pagination
                if (ValidatePaginationOptions(page, itemsPerPage) && page <= GetPageCount(itemsPerPage, courses))
                {
                    courses = courses.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    //Returning empty list when accessing non-existent page
                    return coursesList;
                }

                coursesList = courses.ToList();
            }

            return coursesList;
        }

        public Course? GetById(int id)
        {
            Course? course = null;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                course = unitOfWork.CourseRepository.GetByID(id);
            }

            return course;
        }

        public bool Exists(int id)
        {
            return GetById(id) != null;
        }

        public bool Save(Course course)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (course.Id == 0)
                    {
                        unitOfWork.CourseRepository.Insert(course);
                    }
                    else
                    {
                        unitOfWork.CourseRepository.Update(course);
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
                    Course course = unitOfWork.CourseRepository.GetByID(id);
                    unitOfWork.CourseRepository.Delete(course);
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
                pageCount = (int)Math.Ceiling((double)unitOfWork.CourseRepository.Count() / itemsPerPage);
            }

            return pageCount;
        }
    }
}
