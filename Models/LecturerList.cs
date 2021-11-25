﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class LecturerList
    {
        public int ID { get; set; }
        public int LecturerListID { get; set; }

        public int ModuleID { get; set; }

        public int LecturerID { get; set; }

        public Module Module { get; set; }
        public Lecturer Lecturer { get; set; }
    }
}
