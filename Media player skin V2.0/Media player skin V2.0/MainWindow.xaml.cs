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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace Media_player_skin_V2._0
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool fullScreen = false;
        private double currentPosition = 0.0;
        private DispatcherTimer timer;
        private bool isDragging = false;
        private bool Loop = false;
        private Thickness marginSave;
        private ObservableCollection<DirMedia> dir = null;
        private LibraryControl libraryControlWindow = new LibraryControl();
        public ObservableCollection<Playlist> pl;
        public TreeViewItem tmpNode;
        private GridViewer grid = new GridViewer();

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = MediaPlayer.NaturalDuration.TimeSpan;
                Time_video.Maximum = ts.TotalSeconds;
                Time_video.SmallChange = 1;
                Time_video.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            else
            {
                TimeSpan ts = TimeSpan.FromSeconds(5);
                Time_video.Maximum = ts.TotalSeconds;
                Time_video.SmallChange = 1;
            }
            PlayingMedia.Text = currentMedia.name;
            timer.Start();
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Close();
            Time_video.Value = 0.0;
            MediaPlayer.Position = TimeSpan.FromSeconds(Time_video.Value);
            if (currentPlaylist != null)
            {
                if (Loop == true && MediaNum + 1 == currentPlaylist.List.Count)
                    MediaNum = -1;
                if (MediaNum + 1 < currentPlaylist.List.Count)
                {
                    MediaNum += 1;
                    currentMedia = currentPlaylist.List[MediaNum];
                    MediaPlayer.Source = new Uri(currentMedia.path);
                    MediaPlayer.Play();
                }
            }
        }

        private void buttonPlayClick(object sender, RoutedEventArgs e)
        {
            if (MediaPlayer.SpeedRatio != 1.0)
                MediaPlayer.SpeedRatio = 1.0;
            MediaPlayer.Play();
        }
        private void buttonStopClick(object sender, RoutedEventArgs e)
        {
            MediaPlayer.SpeedRatio = 1.0;
            MediaPlayer.Stop();
            MediaPlayer.Close();
            Time_video.Value = 0.0;
            MediaPlayer.Position = TimeSpan.FromSeconds(Time_video.Value);
        }
        private void buttonPauseClick(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }
        private void buttonForwardClick(object sender, RoutedEventArgs e)
        {
            //MediaPlayer.
            if (MediaPlayer.SpeedRatio < 24)
                MediaPlayer.SpeedRatio += 2.0;
            //MessageBox.Show(""+MediaPlayer.SpeedRatio);
            MediaPlayer.Play();
        }
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            MediaPlayer.Volume = (double)Sound_settings.Value;
        }

        private void mediaPlayer_ButtonFullScreen(object sender, RoutedEventArgs e)
        {
            if (!fullScreen)
            {
                this.Background = new SolidColorBrush(Colors.Black);
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                Thickness margin = MediaPlayer.Margin;
                marginSave = MediaPlayer.Margin;
                margin.Bottom = 0;
                margin.Left = 0;
                margin.Right = 0;
                margin.Top = 0;
                MediaPlayer.Margin = margin;
                MediaPlayer.Position = TimeSpan.FromSeconds(currentPosition);
            }
            else
            {
                this.Background = new SolidColorBrush(Colors.White);
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                MediaPlayer.Margin = marginSave;
                MediaPlayer.Position = TimeSpan.FromSeconds(currentPosition);
            }
            fullScreen = !fullScreen;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
            {
                Time_video.Value = MediaPlayer.Position.TotalSeconds;
                currentPosition = MediaPlayer.Position.TotalSeconds;
            }
        }
        private void Time_video_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }
        private void Time_video_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            MediaPlayer.Position = TimeSpan.FromSeconds(Time_video.Value);
        }

        private void AddLibrary_Click(object sender, RoutedEventArgs e)
        {
            this.libraryControlWindow.Show();
            return;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
                            dir = xml.Deserialize(fs) as ObservableCollection<DirMedia>;
                            refreshLibrary();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
            if (dir == null)
            {
                dir = new ObservableCollection<DirMedia>();
                dir.Add(new DirMedia(eMediaType.IMAGE));
                dir.Add(new DirMedia(eMediaType.VIDEO));
                dir.Add(new DirMedia(eMediaType.MUSIC));
            }
            dir[0].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListPictureChanged);
            dir[1].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListVideoChanged);
            dir[2].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListMusicChanged);
            this.libraryControlWindow.listDir = dir;
            Sound_settings.ValueChanged += ChangeMediaVolume;

            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                    if (fs.Length > 0)
                        pl = (ObservableCollection<Playlist>)xml.Deserialize(fs);
                    else
                        pl = new ObservableCollection<Playlist>();
                }
                catch (Exception)
                {
                    MessageBox.Show("The file for playlist has been corrupted. New playlist file has been created");
                    pl = new ObservableCollection<Playlist>();
                }
            }
            Stream Istream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlistNum", FileMode.OpenOrCreate);
            BinaryFormatter IFormatter = new BinaryFormatter();
            if (Istream.Length > 0 && pl.Count > 0)
                plIndex = (int)IFormatter.Deserialize(Istream);
            else
                plIndex = 1;
            if (pl.Count == 0 || plIndex < 0 || plIndex > int.MaxValue - 1)
            {
                plIndex = 1;
                pl.Add(new Playlist("Playlist" + plIndex));
                plIndex++;
                using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.Create))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                    xml.Serialize(fs, pl);
                }
                IFormatter.Serialize(Istream, plIndex);
            }
            Istream.Close();
            for (int i = 0; i < pl.Count(); i++)
            {
                TreeViewItem tmp = new TreeViewItem();
                tmp.MouseRightButtonDown += new MouseButtonEventHandler(renamePlaylist);
                tmp.Foreground = Brushes.White;
                tmp.Header = pl[i].Name;
                for (int j = 0; j < pl[i].List.Count; j++)
                {
                    TreeViewItem media = new TreeViewItem();
                    media.Foreground = Brushes.White;
                    media.MouseDoubleClick += new MouseButtonEventHandler(PlaySong);
                    media.MouseRightButtonDown += new MouseButtonEventHandler(renamePlaylist);
                    media.Header = pl[i].List[j].name;
                    tmp.Items.Add(media);
                }
                treePl.Items.Add(tmp);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.libraryControlWindow.closeWindow = true;
            this.libraryControlWindow.Close();
        }

        private void Sound_off_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.IsMuted = true;
        }

        private void Sound_on_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.IsMuted = false;
        }

        private void renamePlaylist(object sender, EventArgs e)
        {
            TreeViewItem tmpItem = (TreeViewItem)sender;

            string sParent = tmpItem.Header.ToString();
            for (int i = 0; i < pl.Count; i++)
                if (pl[i].Name == sParent)
                {
                    rename input = new rename();
                    input.tmpNode = tmpItem;
                    input.tmpPl = pl[i];
                    input.selectedIndex = i;
                    input.tmpOb = pl;
                    input.Show();
                    break;
                }
        }

        private void PlaySong(object sender, RoutedEventArgs e)
        {
            TreeViewItem tmpItem = (TreeViewItem)sender;
            TreeViewItem parent = tmpItem.Parent as TreeViewItem;
            string sParent = parent.Header.ToString();
            for (int i = 0; i < pl.Count; i++)
                    if (pl[i].Name == sParent)
                    {
                        for (int j = 0; j < pl[i].List.Count; j++)
                        {
                            if (pl[i].List[j].name == tmpItem.Header.ToString())
                            {
                                MediaPlayer.Stop();
                                MediaPlayer.SpeedRatio = 1.0;
                                currentMedia = pl[i].List[j];
                                MediaNum = j;
                                currentPlaylist = pl[i];
                                MediaPlayer.Source = new Uri(currentMedia.path);
                                MediaPlayer.Play();
                                break;
                            }
                        }
                        break;
                    }
            }

        private void Add_media_button_Click(object sender, RoutedEventArgs e)
        {
            if (Menu_listbox.SelectedItem == null)
            {
                MessageBox.Show("Selectionnez un fichier");
                return;
            }
            if (treePl.SelectedItem == null)
            {
                MessageBox.Show("Selectionnez une playlist");
                return;
            }
            else
            {
                Media tmpMedia = (Media)listViewMedia.SelectedItem;
                TreeViewItem tmpItem = (TreeViewItem)treePl.SelectedItem;
                if (tmpMedia == null)
                {
                    MessageBox.Show("Sélectionnez un média à ajouter à la playlist.");
                    return;
                }
                if (tmpItem.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                {
                    Playlist tmpPl;
                    for (int i = 0; i < pl.Count; i++)
                        if (pl[i].Name == tmpItem.Header.ToString())
                        {
                            tmpPl = pl[i];
                            tmpPl.List.Add(tmpMedia);
                            TreeViewItem newSong = new TreeViewItem();
                            newSong.MouseDoubleClick += new MouseButtonEventHandler(PlaySong);
                            newSong.Header = tmpMedia.name;
                            newSong.Foreground = Brushes.White;
                            tmpItem.Items.Add(newSong);
                            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
                            {
                                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                                xml.Serialize(fs, pl);
                            }
                            break;
                        }
                }
                else
                    MessageBox.Show("Selectionnez une playlist");
            }
        }

        private void Delete_media_button_Click(object sender, RoutedEventArgs e)
        {
            if (treePl.SelectedItem == null)
            {
                MessageBox.Show("Selectionnez un media");
                return;
            }
            else
            {
                TreeViewItem tmpMedia = (TreeViewItem)treePl.SelectedItem;
                if (tmpMedia.Items.Count == 0)
                {
                    TreeViewItem tmpPl = tmpMedia.Parent as TreeViewItem;
                    for (int i = 0; i < pl.Count; i++)
                        if (pl[i].Name == tmpPl.Header.ToString())
                        {
                            Playlist curPlaylist;
                            curPlaylist = pl[i];
                            for (int j = 0; j < curPlaylist.List.Count; j++)
                                if (curPlaylist.List[j].name == tmpMedia.Header.ToString())
                                {
                                    curPlaylist.List.RemoveAt(j);
                                    tmpPl.Items.Remove(tmpMedia);
                                    using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.Create))
                                    {
                                        XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                                        xml.Serialize(fs, pl);
                                    }
                                    break;
                                }
                            break;
                        }
                }
                else
                    MessageBox.Show("Select a song please");
            }
        }

        private void Add_playlist_button_Click(object sender, RoutedEventArgs e)
        {
            pl.Add(new Playlist("Playlist" + plIndex));
            TreeViewItem Pltmp = new TreeViewItem();
            Pltmp.MouseRightButtonDown += new MouseButtonEventHandler(renamePlaylist);
            Pltmp.Foreground = Brushes.White;
            Pltmp.Header = "Playlist" + plIndex;
            plIndex++;

            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                xml.Serialize(fs, pl);
            }

            Stream Istream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlistNum", FileMode.OpenOrCreate);
            BinaryFormatter IFormatter = new BinaryFormatter();
            IFormatter.Serialize(Istream, plIndex);
            Istream.Close();
            treePl.Items.Add(Pltmp);
        }

        private void Delete_playlist_button_Click(object sender, RoutedEventArgs e)
        {
            if (treePl.SelectedItem == null)
            {
                MessageBox.Show("Selectionnez une Playlist");
                return;
            }
            else
            {
                tmpNode = (TreeViewItem)treePl.SelectedItem;
                for (int i = 0; i < pl.Count; i++)
                {
                    if (pl[i].Name == tmpNode.Header.ToString())
                    {
                        pl.Remove(pl[i]);
                        treePl.Items.Remove(tmpNode);
                        using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.Create))
                        {
                            XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                            xml.Serialize(fs, pl);
                        }
                        return;
                    }
                }
                MessageBox.Show("Selectionnez une Playlist");
            }
        }

        private void Loop_button_Click(object sender, RoutedEventArgs e)
        {
            Loop = !Loop;
            MessageBox.Show((Loop ? "Loop is true" : "Loop is false"));
        }

        private void Next_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlaylist != null && MediaNum + 1 < currentPlaylist.List.Count)
            {
                MediaPlayer.Stop();
                MediaPlayer.Close();
                MediaNum += 1;
                currentMedia = currentPlaylist.List[MediaNum];
                MediaPlayer.Source = new Uri(currentMedia.path);
                MediaPlayer.Play();
            }
            else if (currentPlaylist != null && MediaNum + 1 == currentPlaylist.List.Count && Loop)
            {
                MediaNum = 0;
                currentMedia = currentPlaylist.List[MediaNum];
                MediaPlayer.Source = new Uri(currentMedia.path);
                MediaPlayer.Play();
            }
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlaylist != null && MediaNum - 1 > -1)
            {
                MediaPlayer.Stop();
                MediaPlayer.Close();
                MediaNum -= 1;
                currentMedia = currentPlaylist.List[MediaNum];
                MediaPlayer.Source = new Uri(currentMedia.path);
                MediaPlayer.Play();
            }
        }

        private void PictureTree_Selected(object sender, RoutedEventArgs e)
        {
            listViewMedia.View = grid.gridPicture;
            ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia => typeMedia.type == eMediaType.IMAGE));
            view.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
            listViewMedia.ItemsSource = view;
        }

        private void MusicTree_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = Menu_listbox.SelectedItem as TreeViewItem;

            if ((String)selected.Header == "Music")
            {
                listViewMedia.View = grid.gridMusic;
                ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia => typeMedia.type == eMediaType.MUSIC));
                view.SortDescriptions.Add(new SortDescription("album", ListSortDirection.Ascending));
                listViewMedia.ItemsSource = view;
            }
        }

        private void VideoTree_Selected(object sender, RoutedEventArgs e)
        {
            listViewMedia.View = grid.gridVideo;
            ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia => typeMedia.type == eMediaType.VIDEO));
            view.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
            listViewMedia.ItemsSource = view;
        }

        private void ArtistTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listArtist = new List<String>();

            listViewMedia.View = grid.gridArtist;
            foreach (Media m in listMedia)
            {
                if (!listArtist.Contains(m.artist))
                    listArtist.Add(m.artist);
            }
            listArtist.Sort();
            listViewMedia.ItemsSource = listArtist;
        }

        private void AlbumTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listAlbum = new List<String>();

            listViewMedia.View = grid.gridAlbum;
            foreach (Media m in listMedia)
            {
                if (!listAlbum.Contains(m.album))
                    listAlbum.Add(m.album);
            }
            listAlbum.Sort();
            listViewMedia.ItemsSource = listAlbum;
        }

        private void GenreTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listGenre = new List<String>();

            listViewMedia.View = grid.gridGenre;
            foreach (Media m in listMedia)
            {
                if (!listGenre.Contains(m.genre))
                    listGenre.Add(m.genre);
            }
            listGenre.Sort();
            listViewMedia.ItemsSource = listGenre;
        }

        private void listViewMedia_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewMedia.SelectedItem != null && listViewMedia.SelectedItem.ToString() == "Media_player_skin_V2._0.Media")
            {
                Media mediaSelected = listViewMedia.SelectedItem as Media;
                if (mediaSelected != null)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.SpeedRatio = 1.0;
                    currentMedia = mediaSelected;
                    currentPlaylist = null;
                    MediaPlayer.Source = new Uri(currentMedia.path);
                    MediaPlayer.Play();
                }
            }
            else if (listViewMedia.SelectedItem != null)
            {
                String element = listViewMedia.SelectedItem.ToString();
                TreeViewItem tvi = Menu_listbox.SelectedItem as TreeViewItem;
                String property = tvi.Header as String;
                if (property == "Album")
                {
                    listViewMedia.View = grid.gridMusic;
                    ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia =>
                        typeMedia.type == eMediaType.MUSIC && typeMedia.album == element));
                    view.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
                    listViewMedia.ItemsSource = view;
                }
                else if (property == "Artist")
                {
                    listViewMedia.View = grid.gridMusic;
                    ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia =>
                        typeMedia.type == eMediaType.MUSIC && typeMedia.artist == element));
                    view.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
                    listViewMedia.ItemsSource = view;
                }
                else
                {
                    listViewMedia.View = grid.gridMusic;
                    ICollectionView view = CollectionViewSource.GetDefaultView(listMedia.Where(typeMedia =>
                        typeMedia.type == eMediaType.MUSIC && typeMedia.genre == element));
                    view.SortDescriptions.Add(new SortDescription("title", ListSortDirection.Ascending));
                    listViewMedia.ItemsSource = view;
                }
            }
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void listViewMedia_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }
                    string header = headerClicked.Column.Header as string;
                    Sort(header.ToLower(), direction);
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }
                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(listViewMedia.ItemsSource);

            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        private void CMenu_close_button_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Play();
        }

        private void CMenu_open_button_Click(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Pause();
        }
    }
}
