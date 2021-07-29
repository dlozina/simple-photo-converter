using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoConverterWebAppv2.App_Work
{
    public class SaveDataApp
    {
        public List<string> fileNumber { get; set; }
        public List<string> filePathList { get; set; }

        public void Initsavedata()
        {
            fileNumber = new List<string>();
            filePathList = new List<string>();
        }
    }
}