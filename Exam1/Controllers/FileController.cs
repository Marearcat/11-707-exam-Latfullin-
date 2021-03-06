﻿using System;
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
                if(file.Key == null)
                {
                    FileModel newFile = new FileModel { Name = file.UploadedFile.FileName, Path = path, ShortDesc = file.ShortDesc, Desc = file.Desc, Type = file.UploadedFile.ContentType };
                    context.Files.Add(newFile);
                }
                else
                {
                    KeyFileModel newFile = new KeyFileModel { Name = file.UploadedFile.FileName, Path = path, ShortDesc = file.ShortDesc, Desc = file.Desc, Type = file.UploadedFile.ContentType, Key = file.Key };
                }
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<FileIndexViewModel> model = new List<FileIndexViewModel>();
            foreach (var file in context.Files)
                model.Add(new FileIndexViewModel { Id = file.Id, Name = file.Name, ShortDesc = file.ShortDesc });
            
            return View(model.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Info(int id)
        {
            if (context.Files.Any(x => x.Id == id))
                return View(context.Files.First(x => x.Id == id));
            if (context.KeyFiles.Any(x => x.Id == id))
                return RedirectPermanent("~/File/KeyInfo?id=" + id);

            return RedirectToAction("Index", "File");
        }

        [HttpGet]
        public IActionResult KeyInfo(int id)
        {
            if (context.KeyFiles.Any(x => x.Id == id))
                return View(context.KeyFiles.First(x => x.Id == id));
            return RedirectToAction("Index", "File");
        }

        [HttpGet]
        public IActionResult Pull(int id)
        {
            var file = context.Files.First(x => x.Id == id);
            return PhysicalFile(_appEnvironment.WebRootPath + file.Path, file.Type, file.Name);
        }

        [HttpGet]
        public IActionResult KeyPull(int id, string key)
        {
            var file = context.KeyFiles.First(x => x.Id == id);
            if(file.Key == key)
                return PhysicalFile(_appEnvironment.WebRootPath + file.Path, file.Type, file.Name);
            return RedirectPermanent("~/File/KeyInfo?id=" + id);
        }
    }
}