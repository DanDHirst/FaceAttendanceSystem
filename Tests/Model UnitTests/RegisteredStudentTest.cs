using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class RegisteredStudentTest
    {
        [Test]
        public void CheckID()
        {
            RegisteredStudent s = new RegisteredStudent
            {
                ID = 1
            };
            Assert.AreEqual(1, s.ID);

        }
        [Test]
        public void CheckStudentID()
        {
            RegisteredStudent s = new RegisteredStudent
            {
                StudentID = 2
            };
            Assert.AreEqual(2, s.StudentID);

        }
        [Test]
        public void CheckClassID()
        {
            RegisteredStudent s = new RegisteredStudent
            {
                ClassID = 3
            };
            Assert.AreEqual(3, s.ClassID);

        }
        [Test]
        public void CheckRegisteredTime()
        {
            DateTime d = DateTime.Now;

            RegisteredStudent c = new RegisteredStudent
            {
                RegisteredTime = d
            };
            Assert.AreEqual(d, c.RegisteredTime);

        }

    }
}
