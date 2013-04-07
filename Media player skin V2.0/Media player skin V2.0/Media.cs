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
            length = TimeSpan.FromSeconds(0);
            if (System.IO.Path.GetExtension(path) != ".mkv")
                getInfoMedia();
            title = (title != null ? title : name);
            artist = (artist != null ? artist : "Unknown artist");
            album = (album != null ? album : "Unknown album");
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
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            lib.refreshLibrary();
        }
    }
}
