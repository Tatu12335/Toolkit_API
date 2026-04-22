using MahApps.Metro.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
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
        // This Probably isn't the most efficient way to scan a folder, since it sends a separate request for each file,
        // but it works for demonstration purposes. In a real application,
        // I would probably implement a batch scanning endpoint that accepts multiple file paths at once to reduce the number of requests and improve performance.
        private void ButtonScanFolder_Click(object sender, RoutedEventArgs e)
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

                Stack<string> stack = new Stack<string>();
                stack.Push(folderPath);
                while (stack.Count > 0)
                {
                    var currentPath = stack.Pop();
                    try
                    {
                        var files = Directory.GetFiles(currentPath);
                        foreach (var file in files)
                        {

                            using (var client = new HttpClient())
                            {
                                var json = JsonConvert.SerializeObject(new { filePath = file });

                                var content = new StringContent(json, Encoding.UTF8, "application/json");

                                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                                var response = client.PostAsync("https://localhost:7023/FileOps/Scan", content).Result;

                                var responseContent = response.Content.ReadAsStringAsync().Result;

                                if (!response.IsSuccessStatusCode)
                                {
                                    ListBoxResults.Items.Add($"Scan failed for {file}: {responseContent}");
                                }
                                else
                                {
                                    ListBoxResults.Items.Add($"Scan successful for {file}: {responseContent}");
                                }
                            }
                        }
                        var directories = Directory.GetDirectories(currentPath);
                        foreach (var dir in directories)
                        {
                            stack.Push(dir);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error accessing {currentPath}: {ex.Message}", "Access Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }




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
