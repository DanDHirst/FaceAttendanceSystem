using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class RegisteredStudent
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int ClassID { get; set; }
        public DateTime RegisteredTime { get; set; }
        public Class Class { get; set; }
        public Student Student { get; set; }
    }
}
