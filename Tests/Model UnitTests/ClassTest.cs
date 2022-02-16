using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class ClassTest
    {
        [Test]
        public void CheckID()
        {
            Class c = new Class
            {
                ID = 1
            };
            Assert.AreEqual(1, c.ID);

        }

        [Test]
        public void CheckClassCode()
        {
            Class c = new Class
            {
                ClassCode = 10
            };
            Assert.AreEqual(10, c.ClassCode);

        }
        [Test]
        public void CheckModuleID()
        {
            Class c = new Class
            {
                ModuleID = 2
            };
            Assert.AreEqual(2, c.ModuleID);

        }
        [Test]
        public void CheckLecturerID()
        {
            Class c = new Class
            {
                LecturerID = 3
            };
            Assert.AreEqual(3, c.LecturerID);

        }
        [Test]
        public void CheckRoom()
        {
            Class c = new Class
            {
                Room = "room 1"
            };
            Assert.AreEqual("room 1", c.Room);

        }
        [Test]
        public void CheckStartDateTime()
        {
            DateTime d =  DateTime.Now;
            
            Class c = new Class
            {
                StartDateTime = d
            };
            Assert.AreEqual(d, c.StartDateTime);

        }
        [Test]
        public void CheckEndDateTime()
        {
            DateTime d = DateTime.Now;

            Class c = new Class
            {
                EndDateTime = d
            };
            Assert.AreEqual(d, c.EndDateTime);

        }


    }
}
