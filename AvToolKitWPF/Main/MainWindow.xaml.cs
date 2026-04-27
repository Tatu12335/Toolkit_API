using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using Toolkit_API.Domain.Entities.Files;

namespace AvToolKitWPF.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// NOTE : didn't do mvvm for this one, since it's a simple app and I wanted to focus on the api calls and the file scanning logic, but in a real application I would definitely use mvvm for better separation of concerns and maintainability.
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly string _token;
        private readonly string _role;

        public MainWindow(string token, string role)
        {
            InitializeComponent();
            _token = token;
            _role = role;

            if (string.Equals(_role, "Admin", StringComparison.OrdinalIgnoreCase))
                AdminPanelButton.Visibility = Visibility.Visible;
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
            var fileInfo = new FileInfo(filePath);
            MessageBox.Show($"File selected: {filePath}", "File Selected", MessageBoxButton.OK, MessageBoxImage.Information);




            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(new { filePath });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await client.PostAsync("https://localhost:7023/api/FileScan/Scan/File", content);


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
        public async Task<string> ScanFolder(string folder)
        {


            try
            {
                using (var client = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(new { filePath = folder });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    var response = await client.PostAsync("https://localhost:7023/api/FileScan/Scan/Folder", content);


                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultList = JsonConvert.DeserializeObject<FolderInfo>(responseContent);
                    foreach (var item in resultList.Files)
                    {
                        ListBoxResults.Items.Add(item);
                    }



                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Scan failed: {response}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return "";
                    }

                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occured " + ex.Message + ex.StackTrace + ex.InnerException);
            }
            return "";


        }

        private async void ButtonScanFolder_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog folderDialog = new OpenFolderDialog();
            var result = folderDialog.ShowDialog();

            if (result == true)
            {
                string folderPath = folderDialog.FolderName;
                MessageBox.Show($"Folder selected: {folderPath}", "Folder Selected", MessageBoxButton.OK, MessageBoxImage.Information);

                if (string.IsNullOrEmpty(folderPath))
                {
                    MessageBox.Show("Please select a valid folder.", "Invalid Folder", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                await ScanFolder(folderPath);




            }
        }
        private void ButtonSubscribe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ButtonAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            var admin = new AdminPanel.AdminPanel(_token);
            admin.Show();
        }
    }
}
