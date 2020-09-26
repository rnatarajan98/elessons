using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using E_Lessons.Models;

namespace E_Lessons.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string path = Server.MapPath("~/UploadedFiles/");
            IEnumerable<string> _Directories= Directory.EnumerateDirectories(path);

            List<DirectoryModel> directories = new List<DirectoryModel>();
            foreach (string Directory in _Directories)
            {
                directories.Add(new DirectoryModel { DirectoryName = Directory });
            }
            
            return View(directories);
        }



        public ActionResult Slides(string directoryName)
        {
            var dir = directoryName + "/Export/";
            var path = Path.Combine(dir, "slide_1.jpg");

            //return Content(path);
            return base.File(path, "image/jpeg");
        }

        


    }
}