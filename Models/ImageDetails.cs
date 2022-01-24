using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class ImageDetails
    {
        public int ID { get; set; }
        public string url { get; set; }
        public double confidence { get; set; }

        public string username { get; set; }
        public int studentCode { get; set; }

        public Boolean active { get; set; }
        public Boolean notFound { get; set; }


    }
}
