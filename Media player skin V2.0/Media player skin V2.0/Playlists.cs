using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Media_player_skin_V2._0
{
    public class Playlists
    {
        public List<Playlist> pl;
        public int plIndex;
        public TreeView wpfTree;
        public ListView wpfListMedia;
        public TreeViewItem tmpNode;
        public Media currentMedia = null;
        public Playlist currentPlaylist = null;
        public MediaElement player;
        public bool openBoxRename;

        public void initPlaylists()
        {
            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
                    if (fs.Length > 0)
                        pl = (List<Playlist>)xml.Deserialize(fs);
                    else
                        pl = new List<Playlist>();
                }
                catch (Exception)
                {
                    MessageBox.Show("Le fichier de playist est corrompue. Un nouveau fichier a été créé.");
                    pl = new List<Playlist>();
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
                    XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
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
                    media.MouseDoubleClick += new MouseButtonEventHandler(playOrNotSong);
                    media.Header = pl[i].List[j].name;
                    tmp.Items.Add(media);
                }
                wpfTree.Items.Add(tmp);
            }
        }

        public void playOrNotSong(object sender, EventArgs e)
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
                                player.Stop();
                                player.SpeedRatio = 1.0;
                                currentMedia = pl[i].List[j];
                                currentPlaylist = pl[i];
                                player.Source = new Uri(currentMedia.path);
                                player.Play();
                                break;
                            }
                        }
                        break;
                    }
        }

        public void renamePlaylist(object sender, EventArgs e)
        {
            if (this.openBoxRename == true)
                return;
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
                    input.tmpManager = this;
                    this.openBoxRename = true;
                    input.Show();
                    break;
                }
        }

        public void AddMediaInPlaylist()
        {
              Media tmpMedia = (Media)wpfListMedia.SelectedItem;
               TreeViewItem tmpItem = (TreeViewItem)wpfTree.SelectedItem;
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
                           newSong.MouseDoubleClick += new MouseButtonEventHandler(playOrNotSong);
                           newSong.Header = tmpMedia.name;
                           newSong.Foreground = Brushes.White;
                           tmpItem.Items.Add(newSong);
                           using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
                           {
                               XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
                               xml.Serialize(fs, pl);
                           }
                           break;
                       }
               }
               else
                   MessageBox.Show("Selectionnez une playlist");
        }

        public void deleteMediaFromPlaylist()
        {
            TreeViewItem tmpMedia = (TreeViewItem)wpfTree.SelectedItem;
           if (tmpMedia.Parent.GetType().ToString() != "System.Windows.Controls.TreeView")
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
                                    XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
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

        public void addNewPlaylist()
        {
            bool stop = false;
            bool goAhead = false;
            for (int i = plIndex; stop != true; i++)
            {  
                for (int j = 0; j < pl.Count; j++)
                {
                    if (pl[j].Name == "Playlist" + i)
                    {       
                        goAhead = true;
                        break;
                    }
                }
                if (goAhead == false)
                {
                    stop = true;
                    plIndex = i;
                }
                goAhead = false;
           }
            pl.Add(new Playlist("Playlist" + plIndex));
            TreeViewItem Pltmp = new TreeViewItem();
            Pltmp.Header = "Playlist" + plIndex;
            Pltmp.MouseRightButtonDown += new MouseButtonEventHandler(renamePlaylist);
            plIndex++;
            Pltmp.Foreground = Brushes.White;
            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.OpenOrCreate))
            {
                XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
                xml.Serialize(fs, pl);
            }

            Stream Istream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlistNum", FileMode.OpenOrCreate);
            BinaryFormatter IFormatter = new BinaryFormatter();
            IFormatter.Serialize(Istream, plIndex);
            Istream.Close();
            wpfTree.Items.Add(Pltmp);
        }

        public void deletePlaylist()
        {
            tmpNode = (TreeViewItem)wpfTree.SelectedItem;
            for (int i = 0; i < pl.Count; i++)
            {
                if (pl[i].Name == tmpNode.Header.ToString())
                {
                    pl.Remove(pl[i]);
                    wpfTree.Items.Remove(tmpNode);
                    using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.Create))
                    {
                        XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
                        xml.Serialize(fs, pl);
                    }
                    return;
                }
            }
            MessageBox.Show("Selectionnez une Playlist");
        }
    }
}
