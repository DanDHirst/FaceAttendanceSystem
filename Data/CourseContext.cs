using FaceAttendance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Data
{
    public class CourseContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public CourseContext(DbContextOptions<CourseContext> options) : base(options)
        {
        }
        public DbSet<CourseList> CourseLists { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }


        public DbSet<ModuleList> ModuleLists { get; set; }
        public DbSet<Class> Classes { get; set; }

        public DbSet<RegisteredStudent> RegisteredStudents { get; set; }



    }
}
