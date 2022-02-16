using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class ModuleListTest
    {
        [Test]
        public void CheckID()
        {
            ModuleList m = new ModuleList
            {
                ID = 1
            };
            Assert.AreEqual(1, m.ID);

        }
        [Test]
        public void CheckModuleListID()
        {
            ModuleList m = new ModuleList
            {
                ModuleListID = 2
            };
            Assert.AreEqual(2, m.ModuleListID);

        }
        [Test]
        public void CheckModuleID()
        {
            ModuleList m = new ModuleList
            {
                ModuleID = 2
            };
            Assert.AreEqual(2, m.ModuleID);

        }
        [Test]
        public void CheckCourseID()
        {
            ModuleList m = new ModuleList
            {
                CourseID = 4
            };
            Assert.AreEqual(4, m.CourseID);

        }
    }
}
