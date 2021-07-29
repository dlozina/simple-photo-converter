using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoConverterWebAppv2.App_Work
{
    public class FilePath
    {
        //Filename of the file
        public string Filename { get; set; }
        // Path of the file on the server 
        public string Path { get; set; }
        // Size of the file (bytes) 
        public long Length { get; set; }
        // True for filename in
        public bool IsDirectory { get; set; }
    }
}