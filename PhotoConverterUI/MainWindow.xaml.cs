using System;
using System.Collections.Generic;
using System.Linq;
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
            if (String.IsNullOrEmpty(App.photodata.image1path))
            {
                if (of.ShowDialog() == true)
                {
                    App.photodata.image1path = of.FileName;
                    // Save path in a class
                    ImgPhoto1.Source = new BitmapImage(new Uri(of.FileName));
                }
                    
            }
            //Check selection photo 2
            else if (String.IsNullOrEmpty(App.photodata.image2path))
            {
                if (of.ShowDialog() == true)
                {
                    App.photodata.image2path = of.FileName;
                    // Save path in a class
                    ImgPhoto2.Source = new BitmapImage(new Uri(of.FileName));
                }   
            }
            //Check selection photo 3
            else if (String.IsNullOrEmpty(App.photodata.image3path))
            {
                if (of.ShowDialog() == true)
                {
                    App.photodata.image3path = of.FileName;
                    // Save path in a class
                    ImgPhoto3.Source = new BitmapImage(new Uri(of.FileName));
                }  
            }
            //Check selection photo 4
            else if (String.IsNullOrEmpty(App.photodata.image4path))
            {
                if (of.ShowDialog() == true)
                {
                        App.photodata.image4path = of.FileName;
                    // Save path in a class
                    ImgPhoto4.Source = new BitmapImage(new Uri(of.FileName));
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
            App.photodata.image1path = "";
            App.photodata.image2path = "";
            App.photodata.image3path = "";
            App.photodata.image4path = "";
            // Update UI
            ImgPhoto1.Source = new BitmapImage();
            ImgPhoto2.Source = new BitmapImage();
            ImgPhoto3.Source = new BitmapImage();
            ImgPhoto4.Source = new BitmapImage();
        }
    }
}
