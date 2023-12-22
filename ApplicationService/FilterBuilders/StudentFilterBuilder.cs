using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationService.FilterBuilders
{
    public class StudentFilterBuilder : IFilterBuilder<Student>
    {
        public int FacultyId { get; set; } = 0;
        public int MajorId { get; set; } = 0;
        public string? FacultyNumber { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public Expression<Func<Student, bool>> BuildFilter()
        {
            Expression<Func<Student, bool>> filter =
                (student => 
                    (FacultyId != 0 ? student.FacultyId == FacultyId : true) &&
                    (MajorId != 0 ? student.MajorId == MajorId : true) &&
                    (!string.IsNullOrEmpty(FacultyNumber) ? student.FacultyNumber == FacultyNumber : true) &&
                    (!string.IsNullOrEmpty(FirstName) ? student.FirstName == FirstName : true) &&
                    (!string.IsNullOrEmpty(MiddleName) ? student.MiddleName == MiddleName : true) &&
                    (!string.IsNullOrEmpty(LastName) ? student.LastName == LastName : true)
                );

            return filter;
        }
    }
}
