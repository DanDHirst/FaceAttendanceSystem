using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests
{
    [TestFixture]
    public class CourseTest
    {
        [Test]
        public void CheckID()
        {
            Course c = new Course
            {
                ID = 1
            };
            Assert.AreEqual(1,c.ID);

        }

        [Test]
        public void CheckCourseCode()
        {
            Course c = new Course
            {
                CourseCode = 123
            };
            Assert.AreEqual(123, c.CourseCode);
        }
        [Test]
        public void CheckCourseName()
        {
            Course c = new Course
            {
                CourseName = "computing"
            };
            Assert.AreEqual("computing", c.CourseName);
        }
    }
}
