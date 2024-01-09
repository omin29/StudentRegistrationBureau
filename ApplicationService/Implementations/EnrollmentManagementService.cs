using ApplicationService.FilterBuilders;
using Data.Entities;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationService.Implementations
{
    public class EnrollmentManagementService : BaseService
    {
        public IEnumerable<Student> Get(int page, int itemsPerPage, IFilterBuilder<Student>? filterBuilder = null)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                string include = "Enrollments.Course"; // Include the "Course" navigation property

                IEnumerable<Student> students;
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
                    return Enumerable.Empty<Student>();
                }

                return students.ToList();
            }
        }



        public int GetPageCount(int itemsPerPage, IFilterBuilder<Student>? filterBuilder = null)
        {
            if (itemsPerPage <= 0)
            {
                return 1;
            }

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

                return (int)Math.Ceiling((double)studentCount / itemsPerPage);
            }
        }

        // New method to fetch enrollments for a list of student IDs
        public IEnumerable<Enrollment> GetEnrollmentsForStudents(IEnumerable<int> studentIds, string selectedCourse)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                // Fetching enrollments based on student IDs and the selected course
                var enrollments = unitOfWork.EnrollmentRepository
                    .Get(filter: e => studentIds.Contains(e.StudentId) && (string.IsNullOrEmpty(selectedCourse) || e.Course.Name == selectedCourse), includeProperties: "Course")
                    .ToList();

                return enrollments;
            }
        }

        public Enrollment? GetById(int id)
        {
            Enrollment? enrollment = null;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                enrollment = unitOfWork.EnrollmentRepository.GetByID(id,"Student,Course");
            }

            return enrollment;
        }

        public bool Exists(int id)
        {
            return GetById(id) != null;
        }

        public bool Delete(int id)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    Enrollment enrollment = unitOfWork.EnrollmentRepository.GetByID(id);
                    unitOfWork.EnrollmentRepository.Delete(enrollment);
                    unitOfWork.Save();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(Enrollment enrollment)
        {
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    if (enrollment.Id == 0)
                    {
                        unitOfWork.EnrollmentRepository.Insert(enrollment);
                    }
                    else
                    {
                        unitOfWork.EnrollmentRepository.Update(enrollment);
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
    }
}