using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;

namespace Media_player_skin_V2._0
{
    /// <summary>
    /// Interaction logic for LibraryControl.xaml
    /// </summary>
    public partial class LibraryControl : Window
    {
        public ObservableCollection<string> listImgDir { get; set; }

        public LibraryControl()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LibraryPath.ItemsSource = listImgDir;
        }

        private void AddLibrary_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();
            string filename = fbd.SelectedPath;
            try
            {
                //System.Windows.Forms.MessageBox.Show(filename);
                Directory.GetFiles(filename, ".txt", SearchOption.AllDirectories);
                if (filename != "" && listImgDir.Where(dirPath => dirPath == filename).Any() == false)
                    listImgDir.Add(filename);
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Directory cannot be a library dir.");
                return;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
