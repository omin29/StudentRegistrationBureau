using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class StudentRegistrationBureauContext : DbContext
    {
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public StudentRegistrationBureauContext()
        {
            Faculties = Set<Faculty>();
            Majors = Set<Major>();
            Courses = Set<Course>();
            Students = Set<Student>();
            Enrollments = Set<Enrollment>();
        }

        public StudentRegistrationBureauContext(DbContextOptions<StudentRegistrationBureauContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Student>()
                .HasAlternateKey(c => c.FacultyNumber)
                .HasName("UniqueFacultyNumber");

            modelBuilder.Entity<Enrollment>()
                .HasAlternateKey(c => new { c.CourseId, c.StudentId })
                .HasName("AlternativeKeyEnrollment");
        }
    }
}
