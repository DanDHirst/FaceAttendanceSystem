using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class Constants
    {
        public const string IMAGE_BASE_URL = "https://faceattendimages.blob.core.windows.net/images/";
        public const string IMAGE_NEW_URL = "https://faceattendimages.blob.core.windows.net/newimage/";
        public const string SUBSCRIPTION_KEY = "28ddc22f482a466eb600ecafb7962bbe";
        public const string ENDPOINT = "https://uksouth.api.cognitive.microsoft.com/";
        public static string _connectionString =
            "DefaultEndpointsProtocol=https;AccountName=faceattendimages;AccountKey=g1VnlmiQ8ChdJluUaPpoOAHtk1WD/CpcNjzTuBKS3P8PekttQvm46jhhRGyG/e3meS7fB/W4AP8+ftjcDNAq+A==;EndpointSuffix=core.windows.net";
        public static string AUTH = "1297";

        public static List<string> ROOMS = new List<string>{"","SMB0",
"SMB1",
"SMB2",
"SMB3",
"SMB4",
"SMB5",
"SMB6",
"SMB7",
"SMB8",
"SMB9", 
"BGB0",
"BGB1",
"BGB2",
"BGB3",
"BGB4",
"BGB5",
"BGB6",
"BGB7",
"BGB8",
"BGB9",
"BGB10","ROL0",
"ROL1",
"ROL2",
"ROL3",
"ROL4",
"ROL5",
"ROL6",
"ROL7",
"ROL8",
"ROL9",
"ROL10",};

    }
}
