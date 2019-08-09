using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoConverterUI.Model
{
    public class Photoupload
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
        // File number to be send
        public int FileNumber { get; set; }
    }
}
