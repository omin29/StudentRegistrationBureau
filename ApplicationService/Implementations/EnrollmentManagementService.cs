using ApplicationService.FilterBuilders;
using Data.Entities;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.Implementations
{
    public class EnrollmentManagementService : BaseService
    {
        public IEnumerable<Student> Get(int page, int itemsPerPage, IFilterBuilder<Student>? filterBuilder = null)
        {
            List<Student> studentList = new List<Student>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                IEnumerable<Student> students;
                string include = "Enrollments.Course"; // Include the "Course" navigation property

                if (filterBuilder == null)
                {
                    students = unitOfWork.StudentRepository.Get(includeProperties: include);
                }
                else
                {
                    var filter = filterBuilder.BuildFilter();
                    students = unitOfWork.StudentRepository.Get(filter: filter, includeProperties: include);
                }

                // Applying pagination
                if (ValidatePaginationOptions(page, itemsPerPage) && page <= GetPageCount(itemsPerPage, students))
                {
                    students = students.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    // Returning empty list when accessing non-existent page
                    return studentList;
                }

                studentList = students.ToList();
            }

            return studentList;
        }


        public int GetPageCount(int itemsPerPage, IFilterBuilder<Student>? filterBuilder = null)
        {
            if (itemsPerPage <= 0)
            {
                return 1;
            }

            int pageCount = 0;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                int studentCount = 0;
                if (filterBuilder == null)
                {
                    studentCount = unitOfWork.StudentRepository.Count();
                }
                else
                {
                    var filter = filterBuilder.BuildFilter();
                    studentCount = unitOfWork.StudentRepository.Count(filter);
                }

                pageCount = (int)Math.Ceiling((double)studentCount / itemsPerPage);
            }

            return pageCount;
        }
    }
}
