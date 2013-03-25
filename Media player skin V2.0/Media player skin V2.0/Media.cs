using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Specialized;
using System.IO;

namespace Media_player_skin_V2._0
{
    class Media
    {
        public string path { get; set; }
        public string name { get; set; }
        public string type { get; set; }

        public Media(string newPath, string newType)
        {
            path = newPath;
            type = newType;
            name = System.IO.Path.GetFileName(path);
        }
    }
    public partial class MainWindow : Window
    {
        private string[] extensionImg = { "*.jpg", "*.bmp", "*.png" };
        private string[] extensionVideo = { "*.mp4", "*.avi", "*.wmv", "*.mkv" };
        private string[] extensionMusic = { "*.mp3", "*.ogg" };
        private ObservableCollection<Media> listMedia = new ObservableCollection<Media>();
        private Media currentMedia;
        private void DirListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string path = this.dir[this.dir.Count - 1];

            foreach (string s in this.extensionImg)
            {
                string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                foreach (string pathToFile in filepath)
                {
                    listMedia.Add(new Media(pathToFile, "Images"));
                }
            }
            foreach (string s in this.extensionVideo)
            {
                string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                foreach (string pathToFile in filepath)
                {
                    listMedia.Add(new Media(pathToFile, "Video"));
                }
            }
            foreach (string s in this.extensionMusic)
            {
                string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                foreach (string pathToFile in filepath)
                {
                    listMedia.Add(new Media(pathToFile, "Music"));
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            this.listMedia.Clear();
            foreach (string path in this.dir)
            {
                foreach (string s in this.extensionImg)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, "Images"));
                    }
                }
                foreach (string s in this.extensionVideo)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, "Video"));
                    }
                }
                foreach (string s in this.extensionMusic)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, "Music"));
                    }
                }
            }
        }
    }
}
