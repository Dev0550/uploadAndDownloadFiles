using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using uploaderTask.Models;

namespace uploaderTask.Controllers
{
    public class MediaController : Controller
    {
        // GET: Media
        public ActionResult Index()
        {
            List<ObjFile> ObjFiles = new List<ObjFile>();

            foreach (string strfile in Directory.GetFiles(Server.MapPath("~/Files")))
            {
                FileInfo fi = new FileInfo(strfile);
                ObjFile obj = new ObjFile();
                var dt = new SQL().GetInfo(fi.Name);
                obj.Name = fi.Name;
                obj.Size = fi.Length;
                obj.Date = dt.Date;
                obj.Type = GetFileType(fi.Extension);
                ObjFiles.Add(obj);
            }
            return View(ObjFiles);
        }
        public FileResult DownloadFile(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult DeleteFile(string fileName)
        {
            string fullPath = Server.MapPath("~/Files/" + fileName);
            FileInfo file = new FileInfo(fullPath);
            if (file.Exists)
            {
                file.Delete();
                new SQL().DeleteRecord(fileName);
                TempData["Message"] = "file deleted successfully";
            }
                return RedirectToAction("Index");
        }
        private string GetFileType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                default:
                    return "Unknown";
            }
        }
        [HttpPost]
        public ActionResult Index(ObjFile upl)
        {
            foreach (var file in upl.Files)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                    file.SaveAs(filePath);
                    new SQL().NewRecord(file.FileName, file.ContentLength, file.ContentType, DateTime.Now);
                }
            }
            TempData["Message"] = "files uploaded successfully";
            return RedirectToAction("Index");
        }
    }
}
public class ObjFile
{
    public IEnumerable<HttpPostedFileBase> Files { get; set; }
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }
}

