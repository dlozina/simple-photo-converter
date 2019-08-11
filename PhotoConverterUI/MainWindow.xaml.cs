using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            // Enable after conversion
            Opensaved.IsEnabled = false;
            // Enable after selection
            Cancelselection.IsEnabled = false;
            Convertphotos.IsEnabled = false;
        }

        private Photoupload uploadResult = null;

        private void Selectphotos_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "Select a picture";
            of.Filter = "Image Files(*.JPEG; *.JPG; *.BMP; *.GIF; )| *.JPEG; *.JPG; *.BMP; *.GIF; | All files(*.*) | *.*";

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
            // Enable Convert Button
            Cancelselection.IsEnabled = true;
            Convertphotos.IsEnabled = true;
        }

        private void Cancelselection_Click(object sender, RoutedEventArgs e)
        {
            // Disable convert photos
            Cancelselection.IsEnabled = false;
            Convertphotos.IsEnabled = false;
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
            // Number of files to be send
            int fileNumber = App.photodata.photopath.Count();
   
            foreach (String localFilename in App.photodata.photopath)
            {
                string url = "http://localhost:52683/api/PhotoConvert";
                string filePath = @"\";
                Random rnd = new Random();
                string uploadFileName = "Imag" + rnd.Next(9999).ToString();
                uploadStatus = Upload(url, filePath, localFilename, uploadFileName, fileNumber);
            }

            if (uploadStatus)
            {
                // Write empty string to reset choice
                App.photodata.photopath.Clear();
                // Change to status bar
                sbStatus.Text = "Status: File Converted";
                sbPathDisplay.Text = "Converted file saved od disk! (To access photo press button open saved location)";
                Opensaved.IsEnabled = true;
            }
            else
            {
                // Write empty string to reset choice
                App.photodata.photopath.Clear();
                // Change to status bar
                sbStatus.Text = "Status: File not Converted";
                sbPathDisplay.Text = "None of the files is saved on a disk!";
            }

            Cancelselection.IsEnabled = false;
            Convertphotos.IsEnabled = false;
        }

        // filepath = @"Some\Folder\";  
        // url= "http://localhost:52683/api/PhotoConvert";  
        // localFilename = "c:\newProduct.jpg"   
        //uploadFileName="newFileName"  
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

        private void Opensaved_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "/select," + uploadResult.FilePath);
        }
    }
}
