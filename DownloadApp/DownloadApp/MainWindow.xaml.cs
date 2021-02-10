using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
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
        public string DownloadedString { get; set; }
        /// <summary>
        /// Object contains all the data form user. Needs to be validated in order to use it
        /// </summary>
        public FileDownloadingData FileDownloadingData { get; set; } = new FileDownloadingData();
        /// <summary>
        /// Event that is fired when string from web has just been downloaded
        /// </summary>
        public event Action<string> StringDownloaded = (x) => { };
        /// <summary>
        /// Event that is fired when file name was provided by the user
        /// </summary>
        public event Action<string, string> FileNameProvided = (x, y) => { };
        /// <summary>
        /// Default constructor
        /// </summary>
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
        /// <summary>
        /// Fired when user clicks submit button
        /// </summary>
        /// <param name="sender">Button</param>
        /// <param name="e">Event args</param>
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
        /// <summary>
        /// Set controls state acording to download state
        /// </summary>
        private void SetControlStateAfterDownload()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                WebsiteUrl.Visibility = Visibility.Hidden;
                FileName.Visibility = Visibility.Visible;
                SubmitButton.Content = "Save";
            });


        }
        /// <summary>
        /// Saves downloaded website to file
        /// </summary>
        /// <param name="fileName">Name of the file to save to</param>
        /// <param name="downloadedString">String with HTML code of the web</param>
        private void SaveToFile(string fileName, string downloadedString)
        {
            File.WriteAllText(fileName, downloadedString);
        }
        /// <summary>
        /// Validates data from user
        /// </summary>
        /// <returns>True if validation suceeded, otherwise false</returns>
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
