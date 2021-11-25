using FaceAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Data
{
    public class DbInitializer
    {
        public static void Initialize(CourseContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            

            var courses = new Course[]
            {
            new Course{CourseID=1050,CourseName="Computing"},
            new Course{CourseID=2050,CourseName="Marketing"},
            new Course{CourseID=3050,CourseName="Art"},

            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var students = new Student[]
            {
            new Student{StudentID=12232,StudentName="Alexander"},
            new Student{StudentID=22231,StudentName="John"},
            new Student{StudentID=32233,StudentName="Paul"}


            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var courseList = new CourseList[]
            {
            new CourseList{CourseListID=1, StudentID=1, CourseID=1},
            new CourseList{CourseListID=2, StudentID=2, CourseID=2},
            new CourseList{CourseListID=3, StudentID=3, CourseID=3},


            };
            foreach (CourseList e in courseList)
            {
                context.CourseLists.Add(e);
            }
            context.SaveChanges();
        }
    }
}
