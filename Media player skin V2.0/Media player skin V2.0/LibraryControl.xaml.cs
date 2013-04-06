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
    [Serializable]
    public class directoryMedia
    {
        public string dir { get; set; }
        public eMediaType type { get; set; }

        public directoryMedia()
        {
            dir = "";
            type = eMediaType.NONE;
        }
        public directoryMedia(string nDir, eMediaType nType)
        {
            dir = nDir;
            type = nType;
        }
    }

    [Serializable]
    public class DirMedia
    {
        public int countDir;
        public eMediaType type;
        public ObservableCollection<directoryMedia> directories = new ObservableCollection<directoryMedia>();


        public DirMedia()
        {
            countDir = 0;
            type = eMediaType.NONE;
        }
        public DirMedia(eMediaType typeMedia)
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
            eMediaType choosenType = eMediaType.NONE;
            TreeViewItem tmp = LibraryPath.SelectedItem as TreeViewItem;
            //PictureTree.
            if (tmp != null)
            {
                string str = tmp.Header as string;
                if (str == "Music")
                    choosenType = eMediaType.MUSIC;
                else if (str == "Video")
                    choosenType = eMediaType.VIDEO;
                else if (str == "Pictures")
                    choosenType = eMediaType.IMAGE;
            }
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
                DirMedia dm = null;
                    
                try
                {
                    Directory.GetFiles(filename, "*.*", SearchOption.AllDirectories);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Directory cannot be a library dir : " + ex.Message);
                    return;
                }
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
