using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class ModuleList
    {
        public int ID { get; set; }
        public int ModuleListID { get; set; }
        public int ModuleID { get; set; }

        public int CourseID { get; set; }

        public Course Course { get; set; }
        public Module Module { get; set; }
    }
}
