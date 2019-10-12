using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoConverterWebAppv2.Models
{
    public class UploadPhoto
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        public int FileNumber { get; set; }
    }
}