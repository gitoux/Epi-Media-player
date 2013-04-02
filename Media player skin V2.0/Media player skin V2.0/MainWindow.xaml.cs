﻿using System;
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
        private Thickness marginSave;
        private ObservableCollection<DirMedia> dir = new ObservableCollection<DirMedia>();
        private LibraryControl libraryControlWindow = new LibraryControl();
        public ObservableCollection<Playlist> pl;
        public TreeViewItem tmpNode;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void plMouseRightClick(object sender, EventArgs e)
        {
            if (treePl.SelectedItem != null)
            {
                tmpNode = (TreeViewItem)treePl.SelectedItem;
                for (int i = 0; i < pl.Count; i++)
                {
                    if (pl[i].Name == tmpNode.Header.ToString())
                    {
                        rename input = new rename();
                        input.tmpNode = tmpNode;
                        input.tmpPl = pl[i];
                        input.selectedIndex = i;
                        input.tmpOb = pl;
                        input.Show();
                        break;
                    }
                }
            }
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
            timer.Start();
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Close();
            Time_video.Value = 0.0;
            MediaPlayer.Position = TimeSpan.FromSeconds(Time_video.Value);
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
            Sound_settings.ValueChanged += ChangeMediaVolume;
            dir.Add(new DirMedia("Pictures"));
            dir.Add(new DirMedia("Video"));
            dir.Add(new DirMedia("Music"));
            dir[0].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListPictureChanged);
            dir[1].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListVideoChanged);
            dir[2].directories.CollectionChanged += new NotifyCollectionChangedEventHandler(DirListMusicChanged);
            this.libraryControlWindow.listDir = dir;
            ICollectionView view = CollectionViewSource.GetDefaultView(listMedia);
            view.GroupDescriptions.Add(new PropertyGroupDescription("type"));
            view.SortDescriptions.Add(new SortDescription("type", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("name", ListSortDirection.Ascending));
            Menu_listbox.ItemsSource = view;

            using (var fs = new FileStream("playlist.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                    if (fs.Length > 0)
                        pl = (ObservableCollection<Playlist>)xml.Deserialize(fs);
                    else
                        pl = new ObservableCollection<Playlist>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The file for playlist has been corrupted. New playlist file has been created");
                    pl = new ObservableCollection<Playlist>();
                }
            }
            Stream Istream = File.Open("playlistNum", FileMode.OpenOrCreate);
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
                using (var fs = new FileStream("playlist.xml", FileMode.Create))
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
                tmp.Header = pl[i].Name;
                for (int j = 0; j < pl[i].List.Count; j++)
                {
                    TreeViewItem media = new TreeViewItem();
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

        private void MenuBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Media select = this.Menu_listbox.SelectedItem as Media;
            if (select != null)
            {
                MediaPlayer.Stop();
                MediaPlayer.SpeedRatio = 1.0;
                //MessageBox.Show(select.name);
                currentMedia = select;
                MediaPlayer.Source = new Uri(currentMedia.path);
                MediaPlayer.Play();
            }
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
                Media tmpMedia = (Media)Menu_listbox.SelectedItem;
                TreeViewItem tmpItem = (TreeViewItem)treePl.SelectedItem;
                if (tmpItem.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                {
                    Playlist tmpPl;
                    for (int i = 0; i < pl.Count; i++)
                        if (pl[i].Name == tmpItem.Header.ToString())
                        {
                            tmpPl = pl[i];
                            tmpPl.List.Add(tmpMedia);
                            TreeViewItem newSong = new TreeViewItem();
                            newSong.Header = tmpMedia.name;
                            tmpItem.Items.Add(newSong);
                            using (var fs = new FileStream("playlist.xml", FileMode.OpenOrCreate))
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
                                    using (var fs = new FileStream("playlist.xml", FileMode.Create))
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
            Pltmp.Header = "Playlist" + plIndex;
            plIndex++;

            using (var fs = new FileStream("playlist.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<Playlist>));
                xml.Serialize(fs, pl);
            }

            Stream Istream = File.Open("playlistNum", FileMode.OpenOrCreate);
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
                        using (var fs = new FileStream("playlist.xml", FileMode.Create))
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
    }
}
