using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JRSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Drawing.Drawing2D;

namespace JRSystem.Controllers
{
    public class FileController : Controller
    {

        private readonly ReferralDBContext _context;

        public FileController(ReferralDBContext _context)
        {
            this._context = _context;
        }
        public async Task<IActionResult> Index(string FileId)
        {
            
            var fileuploadView = await LoadAllFiles(FileId);
            ViewBag.Message = TempData["Message"];
            TempData["FileId"] = FileId;
            return View(fileuploadView);
        }

        public async Task<IActionResult> ShowFiles(string FileId)
        {

            var fileuploadViewResult = await LoadAllFiles(FileId);
            ViewBag.Message = TempData["Message"];
            TempData["FileId"] = FileId;
            return View(fileuploadViewResult);
        }

        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var fileModel = new FileOnDatabase
                {
                    CreatedOn = DateTime.Now,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    Description = description,
                    ReferralId = TempData["ReferralId"] as string,
                    UserId = HttpContext.Session.GetInt32("_AccountID"),
                    FileId = TempData["FileId"] as string
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                _context.FilesOnDatabase.Add(fileModel);
                TempData["ReferralId"] = fileModel.ReferralId;
                TempData["FileId"] = fileModel.FileId;
                _context.SaveChanges();
            }
            TempData["Message"] = "File successfully uploaded";
            return RedirectToAction("index", new {FileId = TempData["FileId"] });
        }

        private async Task<FileUploadView> LoadAllFiles(string FileId)
        {
            var viewModel = new FileUploadView();
            
            viewModel.FilesOnDatabase = await _context.FilesOnDatabase.Where(f => f.FileId == FileId).ToListAsync();
            
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
        }
        
        public async Task<IActionResult> done()
        {
            return View();
        }
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await _context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.FilesOnDatabase.Remove(file);
            _context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("Index",new {FileId = file.FileId});
        }


    }
}
