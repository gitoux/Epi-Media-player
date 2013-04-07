using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

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
}
