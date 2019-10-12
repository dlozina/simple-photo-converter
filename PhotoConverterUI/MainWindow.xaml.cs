using Microsoft.Win32;
using PhotoConverterUI.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PhotoConverterUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Photodata photodata;
        private OpenFileDialog of;
        private ConvertPhoto convertphoto;
        private BitmapImage firstphoto;
        private BitmapImage secondphoto;
        private BitmapImage thirdphoto;
        private BitmapImage fourthphoto;
        private bool UploadStatus;
        private int FileNumber;

        public MainWindow()
        {
            InitializeComponent();
            photodata = new Photodata();
            of = new OpenFileDialog();
            convertphoto = new ConvertPhoto();
            photodata.photopath = new List<string>();
            firstphoto = new BitmapImage();
            secondphoto = new BitmapImage();
            thirdphoto = new BitmapImage();
            fourthphoto = new BitmapImage();
            Opensaved.IsEnabled = false;
            Cancelselection.IsEnabled = false;
            Convertphotos.IsEnabled = false;
            of.Title = "Select a picture";
            of.Filter = "Image Files(*.JPEG; *.JPG; *.BMP; *.GIF; )| *.JPEG; *.JPG; *.BMP; *.GIF; | All files(*.*) | *.*";
        }

        private void Selectphotos_Click(object sender, RoutedEventArgs e)
        {
            //Check selection photo 1
            if (photodata.photopath.Count == 0)
            {
                if (of.ShowDialog() == true)
                {
                    photodata.AddPhotoPath(of.FileName);
                    ImgPhoto1.Source = DisplayPhoto(firstphoto);
                }
            }
            //Check selection photo 2
            else if (photodata.photopath.Count == 1)
            {
                if (of.ShowDialog() == true)
                {
                    photodata.AddPhotoPath(of.FileName);
                    ImgPhoto2.Source = DisplayPhoto(secondphoto);
                }
            }
            //Check selection photo 3
            else if (photodata.photopath.Count == 2)
            {
                if (of.ShowDialog() == true)
                {
                    photodata.AddPhotoPath(of.FileName);
                    ImgPhoto3.Source = DisplayPhoto(thirdphoto);
                }
            }
            //Check selection photo 4
            else if (photodata.photopath.Count == 3)
            {
                if (of.ShowDialog() == true)
                {
                    photodata.AddPhotoPath(of.FileName);
                    ImgPhoto4.Source = DisplayPhoto(fourthphoto);
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
            photodata.photopath.Clear();
            // Update UI (clear photo)
            ImgPhoto1.Source = null;
            ImgPhoto2.Source = null;
            ImgPhoto3.Source = null;
            ImgPhoto4.Source = null;
            // New instance created so we ca do new init
            firstphoto = new BitmapImage();
            secondphoto = new BitmapImage();
            thirdphoto = new BitmapImage();
            fourthphoto = new BitmapImage();
        }

        // Test upload to server
        private void Convertphotos_Click(object sender, RoutedEventArgs e)
        {
            // Clear converted selection
            ImgPhoto1.Source = null;
            ImgPhoto2.Source = null;
            ImgPhoto3.Source = null;
            ImgPhoto4.Source = null;
            // Number of files to be send
            FileNumber = photodata.photopath.Count();
            UploadStatus = convertphoto.ConvertFiles(FileNumber, photodata.photopath);
            if (UploadStatus)
            {
                // Change to status bar
                sbStatus.Text = "Status: File Converted";
                sbPathDisplay.Text = "Converted file saved od disk! (To access photo press button open saved location)";
                Opensaved.IsEnabled = true;
            }
            else
            {
                // Change to status bar
                sbStatus.Text = "Status: File not Converted";
                sbPathDisplay.Text = "None of the files is saved on a disk!";
            }
            Cancelselection.IsEnabled = false;
            Convertphotos.IsEnabled = false;
        }

        private BitmapImage DisplayPhoto(BitmapImage selectedphoto)
        {
            selectedphoto.BeginInit();
            selectedphoto.CacheOption = BitmapCacheOption.OnLoad;
            selectedphoto.UriSource = new Uri(of.FileName);
            selectedphoto.EndInit();
            return selectedphoto;
        }

        private void Opensaved_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "/select," + convertphoto.uploadResult.FilePath);
        }
    }
}