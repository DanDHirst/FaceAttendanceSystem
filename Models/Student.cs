using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class Student
    {
        public int ID { get; set; }
        public int StudentCode { get; set; }
        public string StudentName { get; set; }
        public string imageUrl { get; set; }
        public bool active { get; set; }
    }
}
