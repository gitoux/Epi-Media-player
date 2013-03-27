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
            MediaPlayer.SpeedRatio = 2.0;
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
                //Video_Grid.Children.Remove(MediaPlayer);
                //Menu_video_grid.Children.Remove(Video_Grid);
                //Global_grid.Children.Remove(Menu_video_grid);
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
                //this.Content = Global_grid;
                //Global_grid.Children.Add(Menu_video_grid);
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
                //MediaPlayer.Stop();
                MessageBox.Show(select.name);
                currentMedia = select;
                MediaPlayer.Source = new Uri(currentMedia.path);
                MediaPlayer.Play();
            }
        }
    }
}
