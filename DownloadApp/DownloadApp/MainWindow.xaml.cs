using System;
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
                FileNameProvided.Invoke(FileName.Text, DownloadedString);
                return;
            }
            var currentUrl = WebsiteUrl.Text;

            await Task.Run(async() =>
            {
                var webClient = new WebClient();
                var downloadedString = await webClient.DownloadStringTaskAsync(currentUrl);


                StringDownloaded.Invoke(downloadedString);

            });
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
    }
}
