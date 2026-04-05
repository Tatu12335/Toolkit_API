using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace AvToolKitWPF.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
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
                    var response = await client.PostAsync("https://localhost:7023/FileOps/Scan",content);
                    
                    if(response.IsSuccessStatusCode)
                        MessageBox.Show(filePath + " scanned successfully!", "Scan Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (HttpIOException ioex)
            {
                MessageBox.Show($"Error scanning file: {ioex.Message}", "Scan Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
