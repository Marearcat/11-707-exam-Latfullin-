using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam1.Data;
using Exam1.Models;
using Exam1.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam1.Controllers
{
    public class FileController : Controller
    {
        ApplicationDbContext context;
        IHostingEnvironment _appEnvironment;

        public FileController(ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Push()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Push(FileViewModel file)
        {
            if (file.UploadedFile != null)
            {
                // путь к папке Files
                string path = "/Files/" + file.UploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await file.UploadedFile.CopyToAsync(fileStream);
                }
                FileModel newFile = new FileModel { Name = file.UploadedFile.FileName, Path = path, ShortDesc = file.ShortDesc, Desc = file.Desc };
                context.Files.Add(newFile);
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}