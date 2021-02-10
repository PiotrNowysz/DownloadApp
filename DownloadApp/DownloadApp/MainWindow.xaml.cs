using System;
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
        public event Action<string> FileNameProvided = (x) => { };
        public MainWindow()
        {
            InitializeComponent();

             StringDownloaded += (x) => SetControlStateAfterDownload();
            StringDownloaded += SaveToFile;
           

            FileName.Visibility = Visibility.Hidden;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

           if(DownloadedString != null)
            {
                SaveToFile(DownloadedString);
                return;
            }
           var currentUrl = WebsiteUrl.Text;

            await Task.Run(() =>
            {
                var webClient = new WebClient();
                var downloadedString = webClient.DownloadString(currentUrl);


                StringDownloaded.Invoke(downloadedString);
                
            }); 
        }
        private void SetControlStateAfterDownload()
        {
            WebsiteUrl.Visibility = Visibility.Hidden;
            FileName.Visibility = Visibility.Visible;
            SubmitButton.Content = "Save";
        }
        private void SaveToFile(string downloadedString)
        {

        }
    }
}
