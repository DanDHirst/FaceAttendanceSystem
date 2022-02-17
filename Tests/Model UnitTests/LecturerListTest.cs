using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class LecturerListTest
    {
        [Test]
        public void CheckID()
        {
            LecturerList l = new LecturerList
            {
                ID = 1
            };
            Assert.AreEqual(1, l.ID);

        }
        [Test]
        public void CheckModuleID()
        {
            LecturerList l = new LecturerList
            {
                ModuleID = 2
            };
            Assert.AreEqual(2, l.ModuleID);

        }
        [Test]
        public void CheckLecturerID()
        {
            LecturerList l = new LecturerList
            {
                LecturerID = 3
            };
            Assert.AreEqual(3, l.LecturerID);

        }
    }
}
