using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using PhotoConverterUI.Model;


namespace PhotoConverterUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void Selectphotos_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select a picture";
            of.Filter = "Image Files(*.JPEG; *.JPG; *.BMP; *.GIF )| *.JPEG; *.JPG; *.BMP;  *.GIF | All files(*.*) | *.*";

            //Check selection photo 1
            if (App.photodata.photopath.Count == 0)
            {
                if (of.ShowDialog() == true)
                {
                    // Save path in a class
                    App.photodata.photopath.Add(of.FileName);
                    
                    BitmapImage firstphoto = new BitmapImage();
                    firstphoto.BeginInit();
                    firstphoto.CacheOption = BitmapCacheOption.OnLoad;
                    firstphoto.UriSource = new Uri(of.FileName);
                    firstphoto.EndInit();
                    ImgPhoto1.Source = firstphoto;
                }
                    
            }
            //Check selection photo 2
            else if (App.photodata.photopath.Count == 1)
            {
                if (of.ShowDialog() == true)
                {
                    // Save path in a class
                    App.photodata.photopath.Add(of.FileName);
                    BitmapImage secondphoto = new BitmapImage();
                    secondphoto.BeginInit();
                    secondphoto.CacheOption = BitmapCacheOption.OnLoad;
                    secondphoto.UriSource = new Uri(of.FileName);
                    secondphoto.EndInit();
                    ImgPhoto2.Source = secondphoto;
                }   
            }
            //Check selection photo 3
            else if (App.photodata.photopath.Count == 2)
            {
                if (of.ShowDialog() == true)
                {
                    // Save path in a class
                    App.photodata.photopath.Add(of.FileName);
                    BitmapImage thirdphoto = new BitmapImage();
                    thirdphoto.BeginInit();
                    thirdphoto.CacheOption = BitmapCacheOption.OnLoad;
                    thirdphoto.UriSource = new Uri(of.FileName);
                    thirdphoto.EndInit();
                    ImgPhoto3.Source = thirdphoto;

                }  
            }
            //Check selection photo 4
            else if (App.photodata.photopath.Count == 3)
            {
                if (of.ShowDialog() == true)
                {
                    // Save path in a class
                    App.photodata.photopath.Add(of.FileName);
                    BitmapImage fourthphoto = new BitmapImage();
                    fourthphoto.BeginInit();
                    fourthphoto.CacheOption = BitmapCacheOption.OnLoad;
                    fourthphoto.UriSource = new Uri(of.FileName);
                    fourthphoto.EndInit();
                    ImgPhoto4.Source = fourthphoto;
                }
            }
            else
            {
                // Check if needed
            }
        }

        private void Cancelselection_Click(object sender, RoutedEventArgs e)
        {
            // Write empty string
            App.photodata.photopath.Clear();
            // Update UI
            ImgPhoto1.Source = new BitmapImage();
            ImgPhoto2.Source = new BitmapImage();
            ImgPhoto3.Source = new BitmapImage();
            ImgPhoto4.Source = new BitmapImage();
        }

        // Test upload to server
        private void Convertphotos_Click(object sender, RoutedEventArgs e)
        {
            // Clear converted selection
            ImgPhoto1.Source = new BitmapImage();
            ImgPhoto2.Source = new BitmapImage();
            ImgPhoto3.Source = new BitmapImage();
            ImgPhoto4.Source = new BitmapImage();

            Boolean uploadStatus = false;
   
            foreach (String localFilename in App.photodata.photopath)
                {
                    string url = "http://localhost:52683/api/PhotoConvert";
                    string filePath = @"\";
                    Random rnd = new Random();
                    string uploadFileName = "Imag" + rnd.Next(9999).ToString();
                    uploadStatus = Upload(url, filePath, localFilename, uploadFileName);
                }

            if (uploadStatus)
            {
                // Change to status bar
                MessageBox.Show("File Uploaded");
            }
            else
            {
                // Change to status bar
                MessageBox.Show("File Not Uploaded");
            }
        }


        // filepath = @"Some\Folder\";  
        // url= "http://localhost:52683/api/PhotoConvert";  
        // localFilename = "c:\newProduct.jpg"   
        //uploadFileName="newFileName"  
        bool Upload(string url, string filePath, string localFilename, string uploadFileName)
        {
            Boolean isFileUploaded = false;

            try
            {
                HttpClient httpClient = new HttpClient();

                var fileStream = File.Open(localFilename, FileMode.Open);
                var fileInfo = new FileInfo(localFilename);
                Photoupload uploadResult = null;
                bool _fileUploaded = false;

                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.Add("filePath", filePath);
                content.Add(new StreamContent(fileStream), "\"file\"", string.Format("\"{0}\"", uploadFileName + fileInfo.Extension)
                        );

                Task taskUpload = httpClient.PostAsync(url, content).ContinueWith(task =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        var response = task.Result;

                        if (response.IsSuccessStatusCode)
                        {
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
                isFileUploaded = false;
            }

            return isFileUploaded;
        }
    }
}
