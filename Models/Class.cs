using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class Class
    {
        public int ID { get; set; }
        public int ClassID { get; set; }
        public int ModuleID { get; set; }
        public int LecturerID { get; set; }

        public string Room { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public Lecturer Lecturer { get; set; }
        public Module Module { get; set; }

    }
}
