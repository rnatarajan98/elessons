using System;  
using System.Collections.Generic;
using System.Drawing;
using System.IO;  
using System.Linq;  
using System.Web;  
using System.Web.Mvc;
using Aspose.Slides;

namespace E_Lessons.Controllers
{ 
    public class UploadController : Controller
    {
        // GET: Upload  
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }


        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    //Create the directory and generate filename
                    string _folder = Server.MapPath("~/UploadedFiles/" + DateTime.Now.ToString("yyyymmddMMss"));

                    Directory.CreateDirectory(_folder);
                    Directory.CreateDirectory(_folder + "/Export");

                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(_folder, _FileName);
                    file.SaveAs(_path);

                    //PrintingCode
                    //string str = string.Join(";", audioSlides.ToArray());
                    //string[] data = { str };
                    //System.IO.File.WriteAllLines(_folder + "/Export/Text", data);
                    

                    Presentation pres = new Presentation(_path);

                    //Obtain audio files, placing them into a list of types and a list of streams.
                    IAudio[] audioData = pres.Audios.ToArray();
                    List<string> typeList = new List<string>();
                    List<Stream> audioStreams = new List<Stream>();
                    if (audioData.Length > 0)
                    {
                        foreach (IAudio audio in audioData)
                        {
                            Stream audioStream = audio.GetStream();
                            string audioType = audio.ContentType;

                            audioStreams.Add(audioStream);
                            typeList.Add(audioType);
                        }


                    }



                    //Initialise a list of slides which contain audio
                    List<string> audioSlides = new List<string>();

                    bool containsAudio;
                    foreach (ISlide sld in pres.Slides)
                    {


                        // Check if slide has audio
                        containsAudio = false;
                        //Generates list of components and checks for Audio
                        IShape[] shapeArray = sld.Shapes.ToArray();
                        foreach (IShape shp in shapeArray)
                        {
                            if (shp.ToString().Contains("AudioFrame"))
                            {
                                containsAudio = true;
                            }
                        }
                        if (containsAudio)
                        {
                            audioSlides.Add(sld.SlideNumber.ToString());
                        }
                        
                        


                        // Export slides as image
                        // Create a full scale image
                        Bitmap bmp = sld.GetThumbnail(1f, 1f);

                        // Save the image to disk in JPEG format
                        bmp.Save(string.Format(_folder + "/Export/Slide_{0}.jpg", sld.SlideNumber), System.Drawing.Imaging.ImageFormat.Jpeg);

                    }

                    if (audioStreams.Capacity > 0)
                    {
                        for (int i = 0; i < (audioStreams.Capacity - 1); i++)
                        {
                            SaveStreamAsFile(_folder + "/Export/", audioStreams[i], "Audio_" + audioSlides[i] + ".mp4");

                        }
                    }

                    
                    
                    
                   
                    ViewBag.Message = pres.Slides.Count.ToString();
                    return View();
                }

                    ViewBag.Message = "File Uploaded Successfully!!";
                    return View();

                
            }
            catch
            {
                ViewBag.Message = "The file upload failed!!";
                return View();
            }


            



        }

        public static void SaveStreamAsFile(string filePath, Stream inputStream, string fileName)
        {
            DirectoryInfo info = new DirectoryInfo(filePath);
            if (!info.Exists)
            {
                info.Create();
            }

            string path = Path.Combine(filePath, fileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                inputStream.CopyTo(outputFileStream);
            }
        }
    }
}