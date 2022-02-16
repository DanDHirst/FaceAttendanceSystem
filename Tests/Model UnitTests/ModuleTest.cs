using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class ModuleTest
    {
        [Test]
        public void CheckID()
        {

            Module m = new Module
            {
                ID = 1
            };

            Assert.AreEqual(1, m.ID);
        }
        [Test]
        public void CheckModuleName()
        {

            Module m = new Module
            {
                ModuleName = "Comp"
            };

            Assert.AreEqual("Comp",m.ModuleName);
        }
        [Test]
        public void CheckModuleCode()
        {

            Module m = new Module
            {
                ModuleCode = "Comp 3000"
            };

            Assert.AreEqual("Comp 3000", m.ModuleCode);
        }
    }
}
