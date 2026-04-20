using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace AvToolKitWPF.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// NOTE : didn't do mvvm for this one, since it's a simple app and I wanted to focus on the api calls and the file scanning logic, but in a real application I would definitely use mvvm for better separation of concerns and maintainability.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly string _token;

        public MainWindow(string token)
        {
            InitializeComponent();
            _token = token;


        }

        private async void ButtonScan_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            var selected = dialog.ShowDialog().Value;
            if (!selected)
            {
                MessageBox.Show("Please select a file to scan.", "No File Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var filePath = dialog.FileName;
            MessageBox.Show($"File selected: {filePath}", "File Selected", MessageBoxButton.OK, MessageBoxImage.Information);

            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(new { filePath });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await client.PostAsync("https://localhost:7023/FileOps/Scan", content);


                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Scan failed: {responseContent}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    ListBoxResults.Items.Add($"Scan successful: {responseContent}");
                }
            }
            catch (HttpIOException ioex)
            {
                MessageBox.Show($"Error scanning file: {ioex.Message}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonSubscribe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
