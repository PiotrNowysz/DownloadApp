using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

namespace DownloadApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DownloadedString { get; set; }
        public FileDownloadingData FileDownloadingData { get; set; } = new FileDownloadingData();
        public event Action<string> StringDownloaded = (x) => { };
        public event Action<string, string> FileNameProvided = (x, y) => { };
        public MainWindow()
        {
            InitializeComponent();

            StringDownloaded += (x) => SetControlStateAfterDownload();
            StringDownloaded += (x) => DownloadedString = x;
            StringDownloaded += (x) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Label.Content = "Enter file's name:";

                });
            };
            FileNameProvided += SaveToFile;
            FileNameProvided += (x, y) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Saved");
                });
            };

            FileName.Visibility = Visibility.Hidden;

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (DownloadedString != null)
            {
                FileDownloadingData.FileName = FileName.Text;
                if (Validate()) FileNameProvided.Invoke(FileDownloadingData.FileName, DownloadedString);

                return;
            }
            FileDownloadingData.Url = WebsiteUrl.Text;
            if (Validate())
            {
                await Task.Run(async () =>
                {
                    var webClient = new WebClient();
                    var downloadedString = await webClient.DownloadStringTaskAsync(FileDownloadingData.Url);


                    StringDownloaded.Invoke(downloadedString);

                });
            }
        }
        private void SetControlStateAfterDownload()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WebsiteUrl.Visibility = Visibility.Hidden;
                FileName.Visibility = Visibility.Visible;
                SubmitButton.Content = "Save";
            });


        }
        private void SaveToFile(string fileName, string downloadedString)
        {
            File.WriteAllText(fileName, downloadedString);
        }
        private bool Validate()
        {
            var validationContext = new ValidationContext(FileDownloadingData);
            var results = new List<ValidationResult>();

            if (Validator.TryValidateObject(FileDownloadingData, validationContext, results, true)) return true;

            results.ForEach(x => MessageBox.Show(x.ErrorMessage));
            return false;
        }
    }
}
