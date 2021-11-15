using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TFI_PAD.Models
{
    public class FileDescriptionShort
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public ICollection<IFormFile> File { get; set; }
    }
}