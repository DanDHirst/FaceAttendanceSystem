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
            new Course{CourseCode=1050,CourseName="Computing"},
            new Course{CourseCode=2050,CourseName="Marketing"},
            new Course{CourseCode=3050,CourseName="Art"},

            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var students = new Student[]
            {
            new Student{StudentCode=12232,StudentName="Alexander"},
            new Student{StudentCode=22231,StudentName="John"},
            new Student{StudentCode=32233,StudentName="Paul"}


            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            var courseList = new CourseList[]
            {
            new CourseList{ StudentID=1, CourseID=1},
            new CourseList{ StudentID=2, CourseID=2},
            new CourseList{ StudentID=3, CourseID=3},


            };
            foreach (CourseList e in courseList)
            {
                context.CourseLists.Add(e);
            }
            context.SaveChanges();

            var Modules = new Module[]
            {
            new Module{ModuleCode="Comp3000",ModuleName="Computing final year"},
            new Module{ModuleCode="Comp3006",ModuleName="fullstack"},
            new Module{ModuleCode="Comp3005",ModuleName="project managment"},



            };
            foreach (Module e in Modules)
            {
                context.Modules.Add(e);
            }
            context.SaveChanges();

            var lecturers = new Lecturer[]
            {
            new Lecturer{LecturerCode=23, LecturerName="Steve"},
            new Lecturer{LecturerCode=24, LecturerName="Matt"},
            new Lecturer{LecturerCode=25, LecturerName="John"},




            };
            foreach (Lecturer e in lecturers)
            {
                context.Lecturers.Add(e);
            }
            context.SaveChanges();

            var ModuleList = new ModuleList[]
            {
            new ModuleList{ModuleID=1, CourseID=1, ModuleListID=1},
            new ModuleList{ModuleID=2, CourseID=2, ModuleListID=2},
            new ModuleList{ModuleID=3, CourseID=3, ModuleListID=3},





            };
            foreach (ModuleList e in ModuleList)
            {
                context.ModuleLists.Add(e);
            }
            context.SaveChanges();

            var Classes = new Class[]
            {
            new Class{ClassCode=1, ModuleID=1, LecturerID=1 , Room="bgb102" },
            new Class{ClassCode=2, ModuleID=2, LecturerID=2 , Room="Rolle102" },
            new Class{ClassCode=3, ModuleID=3, LecturerID=3 , Room="smb103" },






            };
            foreach (Class e in Classes)
            {
                context.Classes.Add(e);
            }
            context.SaveChanges();


            var RegisteredStudents = new RegisteredStudent[]
            {
            new RegisteredStudent{ StudentID = 1, ClassID = 1, RegisteredTime= DateTime.Now},
            new RegisteredStudent{ StudentID = 2, ClassID = 2, RegisteredTime= DateTime.Now},
            new RegisteredStudent{ StudentID = 3, ClassID = 3, RegisteredTime= DateTime.Now},






            };
            foreach (RegisteredStudent e in RegisteredStudents)
            {
                context.RegisteredStudents.Add(e);
            }
            context.SaveChanges();




        }
    }
}
