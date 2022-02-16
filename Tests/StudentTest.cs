using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceAttendance.Models;
using NUnit.Framework;

namespace FaceAttendance.Tests
{
    [TestFixture]
    public class StudentTest
    {

        [Test]
        public void CheckID()
        {

            Student s = new Student
            {
                ID = 1
            };

            Assert.AreEqual(1, s.ID);
        }

        [Test]
        public void CheckStudentCode()
        {

            Student s = new Student
            {
                StudentCode = 2
            };

            Assert.AreEqual(2, s.StudentCode);
        }

        [Test]
        public void CheckStudentName()
        {

            Student s = new Student
            {
                StudentName = "steve"
            };

            Assert.AreEqual("steve", s.StudentName);
        }
        [Test]
        public void CheckImageURL()
        {

            Student s = new Student
            {
                imageUrl = "www.google.com"
            };

            Assert.AreEqual("www.google.com", s.imageUrl);
        }
        [Test]
        public void active()
        {

            Student s = new Student
            {
                active = true
            };

            Assert.AreEqual(true, s.active);
        }
    }
}
