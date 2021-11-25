using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class CourseList
    {
        public int ID { get; set; }
        public int CourseListID { get; set; }
        
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
