using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using E_Lessons.Models;

namespace E_Lessons.Controllers
{
    public class SlideController : Controller
    {

        public ActionResult Index(string directoryName, int index)
        {
            var slide = new SlideModel { DirectoryName = directoryName, SlideNumber = index };

            //return Content(path);
            return View(slide);
        }

        public ActionResult Slide(string directoryName, int index)
        {
            var dir = directoryName + "/Export/";
            var path = Path.Combine(dir, "slide_" + index + ".jpg");

            //return Content(path);
            return base.File(path, "image/jpeg");
        }
    }
}