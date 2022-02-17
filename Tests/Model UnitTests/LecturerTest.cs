using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class LecturerTest
    {
        [Test]
        public void CheckID()
        {
            Lecturer c = new Lecturer
            {
                ID = 1
            };
            Assert.AreEqual(1, c.ID);

        }

        [Test]
        public void CheckLecturerCode()
        {
            Lecturer c = new Lecturer
            {
                LecturerCode = 123
            };
            Assert.AreEqual(123, c.LecturerCode);

        }
        [Test]
        public void CheckLecturerName()
        {
            Lecturer c = new Lecturer
            {
                LecturerName = "John"
            };
            Assert.AreEqual("John", c.LecturerName);

        }

    }
}
