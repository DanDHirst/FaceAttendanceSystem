using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using PersonRecog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceAttendance.Models;
using Microsoft.AspNetCore.Hosting;
using FaceAttendance.Data;

namespace FaceAttendance.Controllers
{
    public class FaceRecognitionController : Controller
    {
        private IHostingEnvironment Environment;
        private readonly CourseContext _context;

        public FaceRecognitionController(IHostingEnvironment _environment, CourseContext context)
        {
            Environment = _environment;
            _context = context;

        }

        // GET: FaceRecognition
        public ActionResult Index()
        {

            return View();
        }

        // GET: FaceRecognition/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {

            const string IMAGE_BASE_URL = "https://facerecogimages.blob.core.windows.net/images/";
            const string IMAGE_NEW_URL = "https://facerecogimages.blob.core.windows.net/newimage/";

            const string SUBSCRIPTION_KEY = "6c52f188e2a646efaa63ab64e8bc319f";
            const string ENDPOINT = "https://uksouth.api.cognitive.microsoft.com/";
            const string RECOGNITION_MODEL3 = RecognitionModel.Recognition03;
            var filename = "newPhoto.jpg";

            BlobStorage.UploadAFile("newimage", filename);

            // Authenticate.
            var client = FaceRecognition.Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            var face = new FaceRecognition();
            // Find Similar - find a similar face from a list of faces.
            face.FindSimilar(client, IMAGE_BASE_URL, IMAGE_NEW_URL, RECOGNITION_MODEL3).Wait();
            BlobStorage.DeleteAFile("newimage", filename);

            ImageDetails matchedImage = new ImageDetails();
            if(matchedImage.url == null)
            {
                matchedImage.notFound = true;
                return View(matchedImage);
            }
            
            matchedImage.url = face.url;
            matchedImage.confidence = (face.confidence*100);
            int idWithoutExtension = int.Parse(Path.GetFileNameWithoutExtension(face.name));
            matchedImage.ID = idWithoutExtension;

            var student = await _context.Students.FindAsync(idWithoutExtension);
            matchedImage.studentCode = student.StudentCode;
            matchedImage.username = student.StudentName;
            matchedImage.active = student.active;

            System.IO.File.Delete(("./wwwroot/image/" + filename));
            return View(matchedImage);
        }


        
        public IActionResult UploadNewPhoto(List<IFormFile> postedFiles)
        {
            
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "image");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                //string fileName = Path.GetFileName(postedFile.FileName);
                string fileName = Path.GetFileName("newPhoto.jpg");
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

            return View();
        }

        // GET: FaceRecognition/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FaceRecognition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FaceRecognition/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FaceRecognition/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FaceRecognition/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FaceRecognition/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
