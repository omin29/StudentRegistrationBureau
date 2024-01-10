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
        public IEnumerable<Enrollment> Get(int page, int itemsPerPage, IFilterBuilder<Enrollment>? filterBuilder = null)
        {
            List<Enrollment> enrollmentList = new List<Enrollment>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                IEnumerable<Enrollment> enrollments;
                string include = "Student,Course";

                if(filterBuilder == null)
                {
                    enrollments = unitOfWork.EnrollmentRepository.Get(includeProperties: include);
                }
                else
                {
                    var filter = filterBuilder.BuildFilter();
                    enrollments = unitOfWork.EnrollmentRepository.Get(filter: filter, includeProperties: include);
                }

                //Applying pagination
                if (ValidatePaginationOptions(page, itemsPerPage) && page <= GetPageCount(itemsPerPage, enrollments))
                {
                    enrollments = enrollments.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                }
                else
                {
                    //Returning empty list when accessing non-existent page
                    return enrollmentList;
                }

                enrollmentList = enrollments.ToList();
            }

            return enrollmentList;
        }

        public int GetPageCount(int itemsPerPage, IFilterBuilder<Enrollment>? filterBuilder = null)
        {
            if (itemsPerPage <= 0)
            {
                return 1;
            }

            int pageCount = 0;

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                int enrollmentCount = 0;
                if (filterBuilder == null)
                {
                    enrollmentCount = unitOfWork.EnrollmentRepository.Count();
                }
                else
                {
                    var filter = filterBuilder.BuildFilter();
                    enrollmentCount = unitOfWork.EnrollmentRepository.Count(filter);
                }

                pageCount = (int)Math.Ceiling((double)enrollmentCount / itemsPerPage);
            }

            return pageCount;
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