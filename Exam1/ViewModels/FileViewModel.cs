using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam1.ViewModels
{
    public class FileViewModel
    {
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        public string Path { get; set; }
        public IFormFile UploadedFile { get; set; }
    }
}
