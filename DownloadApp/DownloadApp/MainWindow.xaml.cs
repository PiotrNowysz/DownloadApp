﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace DownloadApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event Action<string> StringDownloaded = (x) => { };
        public event Action<string> FileNameProvided = (x) => { };
        public MainWindow()
        {
            InitializeComponent();

             StringDownloaded += (x) => SetVisibilityAfterDownload();
            StringDownloaded += SaveToFile;
           

            FileName.Visibility = Visibility.Hidden;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
           var currentUrl = WebsiteUrl.Text;

            await Task.Run(() =>
            {
                var webClient = new WebClient();
                var downloadedString = webClient.DownloadString(currentUrl);


                StringDownloaded.Invoke(downloadedString);
                
            }); 
        }
        private void SetVisibilityAfterDownload()
        {
            WebsiteUrl.Visibility = Visibility.Hidden;
            FileName.Visibility = Visibility.Visible;
        }
        private void SaveToFile(string downloadedString)
        {

        }
    }
}
