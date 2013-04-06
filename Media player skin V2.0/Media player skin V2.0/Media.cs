using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;

namespace Media_player_skin_V2._0
{
    [Serializable]
    public enum eMediaType
    { 
        VIDEO,
        MUSIC,
        IMAGE,
        NONE
    }

    [Serializable]
    public class Playlist
    {
        public string Name { get; set; }
        public ObservableCollection<Media> List { get; set; }
        public Playlist(string _name)
        {
            Name = _name;
            List = new ObservableCollection<Media>();
        }

        public Playlist()
        {
        }
    }

    [Serializable]
    public class Media
    {
        public string path { get; set; }
        public string name { get; set; }
        public eMediaType type { get; set; }
        public string title { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public string genre { get; set; }
        public TimeSpan length { get; set; }

        public Media(string newPath, eMediaType newType)
        {
            path = newPath;
            type = newType;
            name = System.IO.Path.GetFileNameWithoutExtension(path);
            artist = null;
            album = null;
            genre = null;
            length = TimeSpan.MinValue;
            if (System.IO.Path.GetExtension(name) == ".mkv")
                MessageBox.Show("Mkv is it!");
            else
                getInfoMedia();
            title = (title != null ? title : name);
            artist = (artist != null ? artist : "Unknown artist");
            album = (album != null ? artist : "Unknown album");
            genre = (genre != null ? genre : "Unknown genre");
        }
        
        public Media()
        {
        }

        private void getInfoMedia()
        {
            TagLib.File file = null;

            try
            {
                file = TagLib.File.Create(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            title = file.Tag.Title;
            if (type == eMediaType.MUSIC)
            {
                if (file.Tag.Performers.Length > 0)
                    artist = file.Tag.Performers[0];
                album = file.Tag.Album;
            }
            if (file.Tag.Genres.Length > 0)
                genre = file.Tag.Genres[0];
            if (type != eMediaType.IMAGE)
                length = file.Properties.Duration;
        }
    }

    public partial class MainWindow : Window
    {
        private string[] extensionImg = { "*.jpg", "*.bmp", "*.png" };
        private string[] extensionVideo = { "*.mp4", "*.avi", "*.wmv", "*.mkv" };
        private string[] extensionMusic = { "*.mp3", "*.ogg" };
        private ObservableCollection<Media> listMedia = new ObservableCollection<Media>();
        private Media currentMedia = null;
        private Playlist currentPlaylist = null;
        private int MediaNum = 0;
        int plIndex;

        private void DirListPictureChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DirMedia tmp = this.dir[0];

            if (tmp.countDir < tmp.directories.Count)
            {
                string path = tmp.directories[tmp.directories.Count - 1].dir;
                foreach (string s in this.extensionImg)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.IMAGE));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
            }
            SerializeLibrary();
        }

        private void DirListVideoChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DirMedia tmp = this.dir[1];

            if (tmp.countDir < tmp.directories.Count)
            {
                string path = tmp.directories[tmp.directories.Count - 1].dir;
                foreach (string s in this.extensionVideo)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.VIDEO));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
            }
            SerializeLibrary();
        }

        private void DirListMusicChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DirMedia tmp = this.dir[2];

            if (tmp.countDir < tmp.directories.Count)
            {
                string path = tmp.directories[tmp.directories.Count - 1].dir;
                foreach (string s in this.extensionMusic)
                {
                    string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.MUSIC));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
            }
            SerializeLibrary();
        }

        private void SerializeLibrary()
        {
            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\library.xml",
                    FileMode.Create))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<DirMedia>));
                xml.Serialize(fs, this.dir);
            }
        }

        private void refreshLibraryPicture()
        {
            DirMedia tmp = this.dir[0];

            foreach (directoryMedia dm in tmp.directories)
            {
                foreach (string s in this.extensionImg)
                {
                    string[] filepath = Directory.GetFiles(dm.dir, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.IMAGE));
                    }
                }
            }
        }

        private void refreshLibraryVideo()
        {
            DirMedia tmp = this.dir[1];

            foreach (directoryMedia dm in tmp.directories)
            {
                foreach (string s in this.extensionVideo)
                {
                    string[] filepath = Directory.GetFiles(dm.dir, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.VIDEO));
                    }
                }
            }
        }

        private void refreshLibraryMusic()
        {
            DirMedia tmp = this.dir[2];

            foreach (directoryMedia dm in tmp.directories)
            {
                foreach (string s in this.extensionMusic)
                {
                    string[] filepath = Directory.GetFiles(dm.dir, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, eMediaType.MUSIC));
                    }
                }
            }
        }

        private void refreshLibrary()
        {
            this.listMedia.Clear();
            refreshLibraryPicture();
            refreshLibraryVideo();
            refreshLibraryMusic();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            refreshLibrary();
        }
    }
}
