using FaceAttendance.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.ViewModel
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public List<Course> Courses { get; set; }
        public SelectList CoursesList { get; set; }
    }
}
