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
        public string type { get; set; }
        public string title = "";
        public string performer = "";
        public string album = "";
        public string albumAuthor = "";
        public string genre = "";
        TimeSpan length = TimeSpan.MinValue;

        public Media(string newPath, string newType)
        {
            path = newPath;
            type = newType;
            name = System.IO.Path.GetFileName(path);
            if (System.IO.Path.GetExtension(name) == ".mkv")
                MessageBox.Show("Mkv is it!");
            else
                getInfoMedia();
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
            catch (TagLib.UnsupportedFormatException/*Exception*/)
            {
                return;
            }
            title = file.Tag.Title;
            if (type == "Music")
            {
                if (file.Tag.Performers.Length > 0)
                    performer = file.Tag.Performers[0];
                album = file.Tag.Album;
                if (file.Tag.AlbumArtists.Length > 0)
                    albumAuthor = file.Tag.AlbumArtists[0];
            }
            if (file.Tag.Genres.Length > 0)
                genre = file.Tag.Genres[0];
            length = file.Properties.Duration;
            //MessageBox.Show("\nTitle: " + file.Tag.Title +
            //    "\nPerformer: " + (file.Tag.Performers.Length > 0 ? file.Tag.Performers[0]: "") +
            //    "\nAlbum: " + file.Tag.Album +
            //    "\nAlbumArtist: " + (file.Tag.AlbumArtists.Length > 0 ? file.Tag.AlbumArtists[0]: "") +
            //    "\nGenre: " + (file.Tag.Genres.Length>0 ? file.Tag.Genres[0]: "") +
            //    "Duration: " + file.Properties.Duration);      
            //TagLib.IPicture[] pictures = file.Tag.Pictures;
            //MessageBox.Show("Embedded Pictures: " + pictures.Length);
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
                        listMedia.Add(new Media(pathToFile, "Images"));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
            }
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
                        listMedia.Add(new Media(pathToFile, "Video"));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
            }
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
                        listMedia.Add(new Media(pathToFile, "Music"));
                    }
                }
                tmp.countDir = tmp.directories.Count;
            }
            else if (tmp.countDir > tmp.directories.Count)
            {
                refreshLibrary();
                tmp.countDir = tmp.directories.Count;
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
                        listMedia.Add(new Media(pathToFile, "Images"));
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
                        listMedia.Add(new Media(pathToFile, "Video"));
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
                        listMedia.Add(new Media(pathToFile, "Music"));
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
