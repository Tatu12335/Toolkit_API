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
using System.Windows.Shapes;

namespace AvToolKitWPF.Login_Create
{
    /// <summary>
    /// Interaction logic for Create.xaml
    /// </summary>
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBox.Show("Account created successfully! Please log in.");
            this.Close();
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
       {
            
       
       }
    }
}
