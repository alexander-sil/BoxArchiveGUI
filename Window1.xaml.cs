using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;

namespace BoxArchiveGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static List<string> filenames = new List<string>();
        public Window1()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Multiselect = true;
            dialog.Title = "Добавить файлы";
            
            dialog.InitialDirectory = Directory.GetCurrentDirectory();


            if (dialog.ShowDialog() == true)
            {
                string[] filenames = dialog.FileNames;

                foreach (string i in filenames)
                {
                    Window1.filenames.Add(i);
                    this.FileList.ItemsSource = filenames;
                }
                
            }
        }

        private void AcceptAllButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.AddExtension = true;
            saveDialog.Title = "Архивировать файлы";
            saveDialog.ValidateNames = true;
            saveDialog.Filter = "Файлы BOX (*.box)|*.BOX";
            saveDialog.OverwritePrompt = false;

            string filename = "C:\\pagefile.sys";

            saveDialog.InitialDirectory = Directory.GetCurrentDirectory();

            if (saveDialog.ShowDialog() == true)
            {
                filename = saveDialog.FileName;

                if (Directory.Exists("canned"))
                {
                    Directory.Delete("canned", true);
                    Directory.CreateDirectory("canned");

                    DirectoryInfo cannedInfo = new DirectoryInfo("canned");
                    cannedInfo.Attributes |= FileAttributes.Hidden;
                } else
                {
                    Directory.CreateDirectory("canned");

                    DirectoryInfo cannedInfo = new DirectoryInfo("canned");
                    cannedInfo.Attributes |= FileAttributes.Hidden;
                }

                if (Directory.Exists("input"))
                {
                    Directory.Delete("input", true);
                    Directory.CreateDirectory("input");

                    DirectoryInfo inputInfo = new DirectoryInfo("input");
                    inputInfo.Attributes |= FileAttributes.Hidden;
                }
                else
                {
                    Directory.CreateDirectory("input");

                    DirectoryInfo inputInfo = new DirectoryInfo("input");
                    inputInfo.Attributes |= FileAttributes.Hidden;
                }

                Logic.PackFilesToInt(filenames.ToArray(), 0);
                Logic.CanFiles();
                Logic.BoxCans(filename);
            }

            MessageBoxResult answer = MessageBox.Show("Открыть каталог, содержащий созданный архив?", "", MessageBoxButton.YesNo);

            if (answer == MessageBoxResult.Yes)
            {
                Process.Start("C:\\Windows\\explorer.exe", $"\"{new FileInfo(filename).Directory.FullName}\"");
                this.Close();
            } else
            {
                this.Close();
            }

        }

        private void AddFilesForm_Closed(object sender, EventArgs e)
        {
            if (Directory.Exists("unboxed")) { Directory.Delete("unboxed", true); }
            if (Directory.Exists("uncanned")) { Directory.Delete("uncanned", true); }

            if (Directory.Exists("input")) { Directory.Delete("input", true); }
            if (Directory.Exists("canned")) { Directory.Delete("canned", true); }
        }

    }
}
