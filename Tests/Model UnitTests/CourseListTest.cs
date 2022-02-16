using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class CourseListTest
    {
        [Test]
        public void CheckID()
        {
            CourseList c = new CourseList
            {
                ID = 1
            };
            Assert.AreEqual(1, c.ID);

        }
        [Test]
        public void CheckStudentID()
        {
            CourseList c = new CourseList
            {
                StudentID = 2
            };
            Assert.AreEqual(2, c.StudentID);

        }
        [Test]
        public void CheckCourseID()
        {
            CourseList c = new CourseList
            {
                CourseID = 3
            };
            Assert.AreEqual(3, c.CourseID);

        }

        [Test]
        public void CheckStartDateTime()
        {
            DateTime d = DateTime.Now;

            CourseList c = new CourseList
            {
                StartDate = d
            };
            Assert.AreEqual(d, c.StartDate);

        }
        [Test]
        public void CheckEndDateTime()
        {
            DateTime d = DateTime.Now;

            CourseList c = new CourseList
            {
                EndDate = d
            };
            Assert.AreEqual(d, c.EndDate);

        }

    }
}
