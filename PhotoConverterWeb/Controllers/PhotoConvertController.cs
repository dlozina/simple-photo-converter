using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PhotoConverterWebAppv2.App_Work;
using PhotoConverterWebAppv2.Models;


namespace PhotoConverterWebAppv2.Controllers
{
    public class PhotoConvertController : ApiController
    {
        private appDBContext appDbContext;

        public PhotoConvertController()
        {
            // Instance context
            appDbContext = new appDBContext();
        }

        // File info class
        public class FilePath
        {
            // Filename of the file 
            public string Filename { get; set; }
            // Path of the file on the server 
            public string Path { get; set; }
            // Size of the file (bytes) 
            public long Length { get; set; }
            // True for filename in
            public bool IsDirectory { get; set; } 
        }

        public string ConvertAndClearData(string photouploadpath, bool domultipage)
        {
            // Path to APP data
            WebApiApplication.saveDataApp.filePathList.Add(photouploadpath);
            string[] filePathArray = WebApiApplication.saveDataApp.filePathList.ToArray();
            // Convertimages
            string[] convertedPhoto =ConvertPhotos.ConvertPhotoToTiff(filePathArray, domultipage);
            // Empty save data
            WebApiApplication.saveDataApp.filePathList.Clear();
            // DeleteUploaded files
            foreach (string photo in filePathArray)
            {
                File.Delete(photo);
            }
            // Write to DB
            WritetoDatabase(convertedPhoto.First());

            return convertedPhoto.First();
        }

        public void WritetoDatabase(string convertedphotopath)
        {
            // Prepare Data for DB
            var ConvertedPhotoInfo = new ConvertedPhoto()
            {
                DateConverted = DateTime.Now,
                FilePath = convertedphotopath,
                FileName = Path.GetFileName(convertedphotopath),
            };

            // Write to DB
            appDbContext.ConvertedPho.Add(ConvertedPhotoInfo);
            // Save to DB
            appDbContext.SaveChanges();
        }

        // Get all Folder Details
        public void getAllSubfolderFiles(DirectoryInfo dirInfo, List<FilePath> files)
        {
            foreach (DirectoryInfo dInfo in dirInfo.GetDirectories())
            {
                foreach (FileInfo fInfo in dInfo.GetFiles())
                {
                    files.Add(new FilePath() { Path = fInfo.DirectoryName, Filename = fInfo.Name, Length = fInfo.Length, IsDirectory = File.GetAttributes(fInfo.DirectoryName).HasFlag(FileAttributes.Directory) });
                    getAllSubfolderFiles(dInfo, files);
                }
            }
        }

        // Upload the File 
        [MimeMultipart]
        public async Task<UploadPhoto> Post()
        {
            string _convertedfilePath ="";
            var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");

            if (Request.Content.IsMimeMultipartContent())
            {

                var filePath = Request.Headers.GetValues("filePath").ToList();
                // Filenumbers to be send
                WebApiApplication.saveDataApp.fileNumber = Request.Headers.GetValues("fileNumber").ToList();
                string filepathfromclient = "";

                if (filePath != null)
                {
                    filepathfromclient = filePath[0];
                    uploadPath = uploadPath + filepathfromclient;
                }         
            }

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
                
            var multipartFormDataStreamProvider = new UploadFileMultiparProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);
            // File name and path
            string _localFileName = multipartFormDataStreamProvider.FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();
            
            // Convert one file
            if (int.Parse(WebApiApplication.saveDataApp.fileNumber[0]) == 1)
            {
                _convertedfilePath = ConvertAndClearData(_localFileName, false);
            }

            // Create multipage from 2 files
            else if (int.Parse(WebApiApplication.saveDataApp.fileNumber[0]) == 2)
            {
                if (WebApiApplication.saveDataApp.filePathList.Count == 1)
                {
                    _convertedfilePath = ConvertAndClearData(_localFileName, true);
                }
                else
                {
                    WebApiApplication.saveDataApp.filePathList.Add(_localFileName);
                }
            }

            // Create multipage from 3 files
            else if (int.Parse(WebApiApplication.saveDataApp.fileNumber[0]) == 3)
            {
                if (WebApiApplication.saveDataApp.filePathList.Count == 2)
                {
                    _convertedfilePath = ConvertAndClearData(_localFileName, true);
                }
                else
                {
                    WebApiApplication.saveDataApp.filePathList.Add(_localFileName);
                }
            }

            // Create multipage from 4 files
            else if (int.Parse(WebApiApplication.saveDataApp.fileNumber[0]) == 4)
            {
                if (WebApiApplication.saveDataApp.filePathList.Count == 3)
                {
                    _convertedfilePath = ConvertAndClearData(_localFileName, true);
                }
                else
                {
                    WebApiApplication.saveDataApp.filePathList.Add(_localFileName);
                }
            }
            else
            {
                // What to do in this case?
            }

            // Create response ----> Conversion Done, Return Path
            return new UploadPhoto
            {
                FilePath = _convertedfilePath,

                FileName = Path.GetFileName(_convertedfilePath),

                FileLength = new FileInfo(_convertedfilePath).Length
            };
        }

        // To Get File Details ----> Not using
        // api/ FileHandlingAPI/getFileInfo?Id=1
        [ActionName("get"), HttpGet]
        public IEnumerable<FilePath> getFileInfo(int Id)
        {
            List<FilePath> files = new List<FilePath>();
            var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");

            DirectoryInfo dirInfo = new DirectoryInfo(uploadPath);

            foreach (FileInfo fInfo in dirInfo.GetFiles())
            {
                files.Add(new FilePath() { Path = uploadPath, Filename = fInfo.Name, Length = fInfo.Length, IsDirectory = File.GetAttributes(uploadPath).HasFlag(FileAttributes.Directory) });
            }

            getAllSubfolderFiles(dirInfo, files);
            return files.ToList();
        }
    }
}
