using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Media_player_skin_V2._0
{
    class GridViewer
    {
        public GridView gridMusic = new GridView();
        public GridView gridVideo = new GridView();
        public GridView gridPicture = new GridView();
        public GridView gridAlbum = new GridView();
        public GridView gridArtist = new GridView();
        public GridView gridGenre = new GridView();

        public GridViewer()
        {
            initGridMusic();
            initGridPicture();
            initGridVideo();
            initGridAlbum();
            initGridArtist();
            initGridGenre();
        }

        private void initGridMusic()
        {
            gridMusic.AllowsColumnReorder = true;
            gridMusic.ColumnHeaderToolTip = "Music information";
            GridViewColumn AlbumColumn = new GridViewColumn();
            AlbumColumn.DisplayMemberBinding = new Binding("album");
            AlbumColumn.Header = "Album";
            AlbumColumn.Width = 150;
            gridMusic.Columns.Add(AlbumColumn);
            GridViewColumn TitleColumn = new GridViewColumn();
            TitleColumn.DisplayMemberBinding = new Binding("title");
            TitleColumn.Header = "Title";
            TitleColumn.Width = 150;
            gridMusic.Columns.Add(TitleColumn);
            GridViewColumn GenreColumn = new GridViewColumn();
            GenreColumn.DisplayMemberBinding = new Binding("genre");
            GenreColumn.Header = "Genre";
            GenreColumn.Width = 100;
            gridMusic.Columns.Add(GenreColumn);
            GridViewColumn LengthColumn = new GridViewColumn();
            LengthColumn.DisplayMemberBinding = new Binding("length");
            LengthColumn.Header = "Length";
            LengthColumn.Width = 57;
            gridMusic.Columns.Add(LengthColumn);
            GridViewColumn ArtistColumn = new GridViewColumn();
            ArtistColumn.DisplayMemberBinding = new Binding("artist");
            ArtistColumn.Header = "Artist";
            ArtistColumn.Width = 150;
            gridMusic.Columns.Add(ArtistColumn);
        }

        private void initGridVideo()
        {
            gridVideo.AllowsColumnReorder = true;
            gridVideo.ColumnHeaderToolTip = "Video information";
            GridViewColumn TitleColumn = new GridViewColumn();
            TitleColumn.DisplayMemberBinding = new Binding("title");
            TitleColumn.Header = "Title";
            TitleColumn.Width = 250;
            gridVideo.Columns.Add(TitleColumn);
            GridViewColumn GenreColumn = new GridViewColumn();
            GenreColumn.DisplayMemberBinding = new Binding("genre");
            GenreColumn.Header = "Genre";
            GenreColumn.Width = 100;
            gridVideo.Columns.Add(GenreColumn);
            GridViewColumn LengthColumn = new GridViewColumn();
            LengthColumn.DisplayMemberBinding = new Binding("length");
            LengthColumn.Header = "Length";
            LengthColumn.Width = 57;
            gridVideo.Columns.Add(LengthColumn);
        }

        private void initGridPicture()
        {
            gridPicture.AllowsColumnReorder = true;
            gridPicture.ColumnHeaderToolTip = "Picture information";
            GridViewColumn TitleColumn = new GridViewColumn();
            TitleColumn.DisplayMemberBinding = new Binding("title");
            TitleColumn.Header = "Title";
            TitleColumn.Width = 350;
            gridPicture.Columns.Add(TitleColumn);
        }

        private void initGridAlbum()
        {
            gridAlbum.ColumnHeaderToolTip = "Album information";
            GridViewColumn AlbumColumn = new GridViewColumn();
            AlbumColumn.Header = "Album";
            AlbumColumn.Width = 350;
            gridAlbum.Columns.Add(AlbumColumn);
        }

        private void initGridArtist()
        {
            gridArtist.ColumnHeaderToolTip = "Artist information";
            GridViewColumn ArtistColumn = new GridViewColumn();
            ArtistColumn.Header = "Artist";
            ArtistColumn.Width = 350;
            gridArtist.Columns.Add(ArtistColumn);
        }

        private void initGridGenre()
        {
            gridGenre.ColumnHeaderToolTip = "Genre information";
            GridViewColumn GenreColumn = new GridViewColumn();
            GenreColumn.Header = "Genre";
            GenreColumn.Width = 350;
            gridGenre.Columns.Add(GenreColumn);
        }
    }
}
