using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FaceAttendance.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Azure;
using PersonRecog;

namespace FaceAttendance.Controllers
{
    public class UsersController : Controller
    {
        const string IMAGE_BASE_URL = "https://facerecogimages.blob.core.windows.net/images/";
        private IHostingEnvironment Environment;

        public UsersController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        // GET: UsersController
        public ActionResult Index()
        {
            
            UserList users = new UserList();
            
            List<string> userList = BlobStorage.GetBlobs("images").ToList();
            foreach (var userName in userList)
            {
                User user = new User();
                user.Name = userName;
                user.Url = IMAGE_BASE_URL + userName;
                users.addUser(user);
            }
            return View(users);
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        
        public IActionResult Create(List<IFormFile> postedFiles)
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
                string fileName = Path.GetFileName(postedFile.FileName);
               
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
                BlobStorage.UploadAFile("images", postedFile.FileName);
                System.IO.File.Delete(("./wwwroot/image/" + postedFile.FileName));
            }
            

            return View();
        }
        // POST: UsersController/Create

        // GET: UsersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersController/Edit/5
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

        // GET: UsersController/Delete/5
        public ActionResult Delete(string id)
        {
            BlobStorage.DeleteAFile("images",id);
            return RedirectToAction(nameof(Index));
        }

        // POST: UsersController/Delete/5
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
