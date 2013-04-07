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
        private Library lib = new Library();
        private LibraryControl libraryControlWindow = new LibraryControl();
        public ObservableCollection<Playlist> pl;
        public TreeViewItem tmpNode;
        private GridViewer grid = new GridViewer();
        Playlists playlistManager;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lib.initLibrary();
            this.libraryControlWindow.listDir = lib.dir;
            Sound_settings.ValueChanged += ChangeMediaVolume;
            playlistManager = new Playlists();
            playlistManager.wpfListMedia = listViewMedia;
            playlistManager.wpfTree = treePl;
            playlistManager.player = MediaPlayer;
            playlistManager.openBoxRename = false;
            playlistManager.initPlaylists();
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
                TimeSpan ts = TimeSpan.FromSeconds(10);
                Time_video.Maximum = ts.TotalSeconds;
                Time_video.SmallChange = 1;
            }
            PlayingMedia.Text = playlistManager.currentMedia.name;
            timer.Start();
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Close();
            Time_video.Value = 0.0;
            MediaPlayer.Position = TimeSpan.FromSeconds(Time_video.Value);
            if (playlistManager.currentPlaylist != null)
            {
                if (Loop == true && playlistManager.MediaNum + 1 == playlistManager.currentPlaylist.List.Count)
                    playlistManager.MediaNum = -1;
                if (playlistManager.MediaNum + 1 < playlistManager.currentPlaylist.List.Count)
                {
                    playlistManager.MediaNum += 1;
                    playlistManager.currentMedia = playlistManager.currentPlaylist.List[playlistManager.MediaNum];
                    MediaPlayer.Source = new Uri(playlistManager.currentMedia.path);
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
            if (MediaPlayer.SpeedRatio < 24)
                MediaPlayer.SpeedRatio += 2.0;
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

        private void Add_media_button_Click(object sender, RoutedEventArgs e)
        {
            if (Menu_library.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez un fichier.");
                return;
            }
            if (treePl.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez une playlist.");
                return;
            }
            else
                playlistManager.AddMediaInPlaylist(); 
        }

        private void Delete_media_button_Click(object sender, RoutedEventArgs e)
        {
            if (treePl.SelectedItem == null)
            {
                MessageBox.Show("Sélectionnez un média.");
                return;
            }
            else
                playlistManager.deleteMediaFromPlaylist();
        }

        private void Add_playlist_button_Click(object sender, RoutedEventArgs e)
        {
            playlistManager.addNewPlaylist();
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
                playlistManager.deletePlaylist();
            }
        }

        private void Next_button_Click(object sender, RoutedEventArgs e)
        {
            if (playlistManager.currentPlaylist != null && playlistManager.MediaNum + 1 < playlistManager.currentPlaylist.List.Count)
            {
                MediaPlayer.Stop();
                MediaPlayer.Close();
                playlistManager.MediaNum += 1;
                playlistManager.currentMedia = playlistManager.currentPlaylist.List[playlistManager.MediaNum];
                MediaPlayer.Source = new Uri(playlistManager.currentMedia.path);
                MediaPlayer.Play();
            }
            else if (playlistManager.currentPlaylist != null && playlistManager.MediaNum + 1 == playlistManager.currentPlaylist.List.Count && Loop)
            {
                playlistManager.MediaNum = 0;
                playlistManager.currentMedia = playlistManager.currentPlaylist.List[playlistManager.MediaNum];
                MediaPlayer.Source = new Uri(playlistManager.currentMedia.path);
                MediaPlayer.Play();
            }
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            if (playlistManager.currentPlaylist != null && playlistManager.MediaNum - 1 > -1)
            {
                MediaPlayer.Stop();
                MediaPlayer.Close();
                playlistManager.MediaNum -= 1;
                playlistManager.currentMedia = playlistManager.currentPlaylist.List[playlistManager.MediaNum];
                MediaPlayer.Source = new Uri(playlistManager.currentMedia.path);
                MediaPlayer.Play();
            }
        }

        private void PictureTree_Selected(object sender, RoutedEventArgs e)
        {
            listViewMedia.View = grid.gridPicture;
            listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia => typeMedia.type == eMediaType.IMAGE);
            Sort("title", ListSortDirection.Ascending);
            this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void MusicTree_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selected = Menu_library.SelectedItem as TreeViewItem;

            if ((String)selected.Header == "Music")
            {
                listViewMedia.View = grid.gridMusic;
                listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia => typeMedia.type == eMediaType.MUSIC);
                Sort("album", ListSortDirection.Ascending);
                this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
                if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                    MediaPlayer.Pause();
            }
        }

        private void VideoTree_Selected(object sender, RoutedEventArgs e)
        {
            listViewMedia.View = grid.gridVideo;
            listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia => typeMedia.type == eMediaType.VIDEO);
            Sort("title", ListSortDirection.Ascending);
            this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void ArtistTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listArtist = new List<String>();

            listViewMedia.View = grid.gridArtist;
            foreach (Media m in lib.listMedia)
            {
                if (!listArtist.Contains(m.artist))
                    listArtist.Add(m.artist);
            }
            listArtist.Sort();
            listViewMedia.ItemsSource = listArtist;
            this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void AlbumTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listAlbum = new List<String>();

            listViewMedia.View = grid.gridAlbum;
            foreach (Media m in lib.listMedia)
            {
                if (!listAlbum.Contains(m.album))
                    listAlbum.Add(m.album);
            }
            listAlbum.Sort();
            listViewMedia.ItemsSource = listAlbum;
            this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void GenreTree_Selected(object sender, RoutedEventArgs e)
        {
            List<String> listGenre = new List<String>();

            listViewMedia.View = grid.gridGenre;
            foreach (Media m in lib.listMedia)
            {
                if (!listGenre.Contains(m.genre))
                    listGenre.Add(m.genre);
            }
            listGenre.Sort();
            listViewMedia.ItemsSource = listGenre;
            this.ECMenu_open_BeginStoryboard.Storyboard.Begin();
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void listViewMedia_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewMedia.SelectedItem != null && listViewMedia.SelectedItem.GetType() == typeof(Media))
            {
                Media mediaSelected = listViewMedia.SelectedItem as Media;
                if (mediaSelected != null)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.SpeedRatio = 1.0;
                    playlistManager.currentMedia = mediaSelected;
                    playlistManager.currentPlaylist = null;
                    MediaPlayer.Source = new Uri(playlistManager.currentMedia.path);
                    MediaPlayer.Play();
                    this.ECMenu_close_BeginStoryboard3.Storyboard.Begin();
                }
            }
            else if (listViewMedia.SelectedItem != null)
            {
                String element = listViewMedia.SelectedItem.ToString();
                TreeViewItem tvi = Menu_library.SelectedItem as TreeViewItem;
                String property = tvi.Header as String;
                if (property == "Album")
                {
                    listViewMedia.View = grid.gridMusic;
                    listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia =>
                           typeMedia.type == eMediaType.MUSIC && typeMedia.album == element);
                    Sort("title", ListSortDirection.Ascending);
                }
                else if (property == "Artist")
                {
                    listViewMedia.View = grid.gridMusic;
                    listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia =>
                        typeMedia.type == eMediaType.MUSIC && typeMedia.artist == element);
                    Sort("title", ListSortDirection.Ascending);
                }
                else
                {
                    listViewMedia.View = grid.gridMusic;
                    listViewMedia.ItemsSource = lib.listMedia.Where(typeMedia =>
                        typeMedia.type == eMediaType.MUSIC && typeMedia.genre == element);
                    Sort("title", ListSortDirection.Ascending);
                }
            }
        }

        GridViewColumnHeader lastHeaderClicked = null;
        ListSortDirection lastDirection = ListSortDirection.Ascending;

        private void listViewMedia_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != lastHeaderClicked)
                        direction = ListSortDirection.Ascending;
                    else
                    {
                        if (lastDirection == ListSortDirection.Ascending)
                            direction = ListSortDirection.Descending;
                        else
                            direction = ListSortDirection.Ascending;
                    }
                    string header = headerClicked.Column.Header as string;
                    Sort(header.ToLower(), direction);
                    if (lastHeaderClicked != null && lastHeaderClicked != headerClicked)
                        lastHeaderClicked.Column.HeaderTemplate = null;
                    lastHeaderClicked = headerClicked;
                    lastDirection = direction;
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
            if (playlistManager.currentMedia != null && playlistManager.currentMedia.type != eMediaType.MUSIC)
                MediaPlayer.Pause();
        }

        private void Loop_disable_button_Click(object sender, RoutedEventArgs e)
        {
            Loop = false;
        }

        private void Loop_enable_button_Click(object sender, RoutedEventArgs e)
        {
            Loop = true;
        }

        private void MediaPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.ECMenu_close_BeginStoryboard3.Storyboard.Begin();
            MediaPlayer.Play();
        }
    }
}
