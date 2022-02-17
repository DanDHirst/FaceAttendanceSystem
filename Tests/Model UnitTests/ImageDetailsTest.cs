using FaceAttendance.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Tests.Model_UnitTests
{
    [TestFixture]
    public class ImageDetailsTest
    {
        [Test]
        public void CheckID()
        {
            ImageDetails i = new ImageDetails
            {
                ID = 1
            };
            Assert.AreEqual(1, i.ID);

        }
        [Test]
        public void CheckURL()
        {
            ImageDetails i = new ImageDetails
            {
                url = "www.google.com"
            };
            Assert.AreEqual("www.google.com", i.url);

        }
        [Test]
        public void CheckConfidence()
        {
            ImageDetails i = new ImageDetails
            {
                confidence = 0.1
            };
            Assert.AreEqual(0.1, i.confidence);

        }
        [Test]
        public void CheckUsername()
        {
            ImageDetails i = new ImageDetails
            {
                username = "Dan"
            };
            Assert.AreEqual("Dan", i.username);

        }
        [Test]
        public void CheckStudentCode()
        {
            ImageDetails i = new ImageDetails
            {
                studentCode = 3
            };
            Assert.AreEqual(3, i.studentCode);

        }
        [Test]
        public void CheckActive()
        {
            ImageDetails i = new ImageDetails
            {
                active = true
            };
            Assert.AreEqual(true, i.active);

        }
        [Test]
        public void CheckFound()
        {
            ImageDetails i = new ImageDetails
            {
                notFound = true
            };
            Assert.AreEqual(true, i.notFound);

        }

    }
}
