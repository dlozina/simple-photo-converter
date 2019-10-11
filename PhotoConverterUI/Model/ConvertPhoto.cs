using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PhotoConverterUI.Model
{
    public class ConvertPhoto 
    {
        private Photoupload uploadResult;

        public bool ConvertFiles(int fileNumber, List<string> filespath)
        {
            Boolean uploadStatus = false;
            foreach (String localFilename in filespath)
            {
                string url = "http://localhost:52683/api/PhotoConvert";
                string filePath = @"\";
                Random rnd = new Random();
                string uploadFileName = "Imag" + rnd.Next(9999).ToString();
                uploadStatus = Upload(url, filePath, localFilename, uploadFileName, fileNumber);
            }

            return uploadStatus;   
        }

        bool Upload(string url, string filePath, string localFilename, string uploadFileName, int fileNumber)
        {
            Boolean isFileUploaded = false;

            try
            {
                HttpClient httpClient = new HttpClient();

                var fileStream = File.Open(localFilename, FileMode.Open);
                var fileInfo = new FileInfo(localFilename);
                bool _fileUploaded = false;

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.Add("filePath", filePath);
                content.Headers.Add("fileNumber", fileNumber.ToString());
                content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", uploadFileName + fileInfo.Extension));
                // My Post
                Task taskUpload = httpClient.PostAsync(url, content).ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        var response = task.Result;

                        if (response.IsSuccessStatusCode)
                        {
                            // My Response
                            uploadResult = response.Content.ReadAsAsync<Photoupload>().Result;
                            if (uploadResult != null)
                            {
                                _fileUploaded = true;
                            }
                        }
                    }

                    fileStream.Dispose();
                });

                taskUpload.Wait();
                if (_fileUploaded)
                {
                    isFileUploaded = true;
                }
                httpClient.Dispose();
            }
            catch (Exception ex)
            {
                // Write empty string
                isFileUploaded = false;
            }

            return isFileUploaded;
        }
    }
}
