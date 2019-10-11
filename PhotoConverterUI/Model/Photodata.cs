using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoConverterUI.Model
{
    public class Photodata
    {
        public List<string> photopath { get; set; }

        public void AddPhotoPath(string filepath  )
        {
            photopath.Add(filepath);
        }
    }
}
