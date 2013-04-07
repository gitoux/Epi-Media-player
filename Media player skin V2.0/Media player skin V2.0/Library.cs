using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Specialized;

namespace Media_player_skin_V2._0
{
    public class Library
    {
        public ObservableCollection<DirMedia> dir;
        public ObservableCollection<Media> listMedia = new ObservableCollection<Media>();
        private string[] extensionImg = { "*.jpg", "*.bmp", "*.png" };
        private string[] extensionVideo = { "*.mp4", "*.avi", "*.wmv", "*.mkv" };
        private string[] extensionMusic = { "*.mp3", "*.ogg" };

        public Library()
        {
            dir = null;
        }

        public void initLibrary()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\library.xml"))
            {
                using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\library.xml", FileMode.OpenOrCreate))
                {
                    try
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<DirMedia>));
                        if (fs.Length > 0)
                        {
                            this.dir = xml.Deserialize(fs) as ObservableCollection<DirMedia>;
                            refreshLibrary();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }

            }
            if (dir == null)
            {
                this.dir = new ObservableCollection<DirMedia>();
                this.dir.Add(new DirMedia(eMediaType.IMAGE));
                this.dir.Add(new DirMedia(eMediaType.VIDEO));
                this.dir.Add(new DirMedia(eMediaType.MUSIC));
            }
            this.dir[0].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListPictureChanged);
            this.dir[1].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListVideoChanged);
            this.dir[2].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListMusicChanged);
        }

        private void DirListPictureChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            DirMedia tmp = this.dir[0];

            if (tmp.countDir < tmp.directories.Count)
            {
                string path = tmp.directories[tmp.directories.Count - 1].dir;
                findFilesSingleDir(extensionImg, path, eMediaType.IMAGE);
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
                findFilesSingleDir(extensionVideo, path, eMediaType.VIDEO);
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
                findFilesSingleDir(extensionMusic, path, eMediaType.MUSIC);
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

            findFilesInAllDir(this.extensionImg, tmp, eMediaType.IMAGE);
        }

        private void refreshLibraryVideo()
        {
            DirMedia tmp = this.dir[1];

            findFilesInAllDir(this.extensionVideo, tmp, eMediaType.VIDEO);
        }

        private void refreshLibraryMusic()
        {
            DirMedia tmp = this.dir[2];

            findFilesInAllDir(this.extensionMusic, tmp, eMediaType.MUSIC);
        }

        private void findFilesSingleDir(string[] extension, string path, eMediaType type)
        {
            foreach (string s in extension)
            {
                string[] filepath = Directory.GetFiles(path, s, SearchOption.AllDirectories);
                foreach (string pathToFile in filepath)
                {
                    listMedia.Add(new Media(pathToFile, type));
                }
            }
        }

        private void findFilesInAllDir(string[] extension, DirMedia dmedia, eMediaType type)
        {
            foreach (directoryMedia dm in dmedia.directories)
            {
                foreach (string s in extension)
                {
                    string[] filepath = Directory.GetFiles(dm.dir, s, SearchOption.AllDirectories);
                    foreach (string pathToFile in filepath)
                    {
                        listMedia.Add(new Media(pathToFile, type));
                    }
                }
            }
        }

        public void refreshLibrary()
        {
            listMedia.Clear();
            refreshLibraryPicture();
            refreshLibraryVideo();
            refreshLibraryMusic();
        }
    }
}
