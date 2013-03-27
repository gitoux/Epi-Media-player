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
using System.ComponentModel;

namespace Media_player_skin_V2._0
{
    /// <summary>
    /// Interaction logic for LibraryControl.xaml
    /// </summary>
    public class directoryMedia
    {
        public string dir { get; set; }
        public string type { get; set; }

        public directoryMedia(string nDir, string nType)
        {
            dir = nDir;
            type = nType;
        }
    }

    public class DirMedia
    {
        public int countDir;
        public string type;
        public ObservableCollection<directoryMedia> directories = new ObservableCollection<directoryMedia>();

        public DirMedia(string typeMedia)
        {
            countDir = 0;
            type = typeMedia;
        }
    }

    public partial class LibraryControl : Window
    {
        public bool closeWindow { get; set; }
        public ObservableCollection<DirMedia> listDir { get; set; }

        public LibraryControl()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            PictureTree.ItemsSource = listDir[0].directories;
            VideoTree.ItemsSource = listDir[1].directories;
            MusicTree.ItemsSource = listDir[2].directories;
            closeWindow = false;
        }

        private void AddLibrary_Click(object sender, RoutedEventArgs e)
        {
            string choosenType;
            TreeViewItem tmp = LibraryPath.SelectedItem as TreeViewItem;
            //PictureTree.
            if (tmp != null)
                choosenType = tmp.Header as string;
            else
            {
                System.Windows.Forms.MessageBox.Show("Not a type of library folder choosen.");
                return;
            }
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();
            string filename = fbd.SelectedPath;
            if (filename != "")
            {
                try
                {
                    DirMedia dm = null;
                    //System.Windows.Forms.MessageBox.Show(choosenType);
                    Directory.GetFiles(filename, "*.*", SearchOption.AllDirectories);
                    foreach (DirMedia dmTmp in listDir)
                    {
                        if (dmTmp.type == choosenType)
                        {
                            dm = dmTmp;
                        }
                    }
                    if (dm != null && dm.directories.Where(dirPath => dirPath.dir == filename).Any() == false)
                    {
                        foreach (DirMedia md in listDir)
                        {
                            if (md.type == choosenType)
                            {
                                md.directories.Add(new directoryMedia(filename, md.type));
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Directory cannot be a library dir.");
                    return;
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void DeleteLibrary_Click(object sender, RoutedEventArgs e)
        {
            if (LibraryPath.SelectedItem != null)
            {
                Type toto = LibraryPath.SelectedItem.GetType();
                if (toto.ToString() == "Media_player_skin_V2._0.directoryMedia")
                {
                    directoryMedia tmp = LibraryPath.SelectedItem as directoryMedia;
                    foreach (DirMedia m in listDir)
                    {
                        if (m.type == tmp.type)
                        {
                            m.directories.Remove(tmp);
                        }
                    }
                }
                else
                    System.Windows.Forms.MessageBox.Show("Cannot delete a category !");
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!closeWindow)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
