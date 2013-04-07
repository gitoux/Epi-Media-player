using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace Media_player_skin_V2._0
{
    public partial class rename : Form
    {
        public Playlist tmpPl;
        public TreeViewItem tmpNode;
        public int selectedIndex;
        public List<Playlist> tmpOb;
        public rename()
        {
            InitializeComponent();
        }

        private void rename_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox2.Text.Trim();
            string sName = name.ToString();
            if (sName == tmpNode.Header.ToString())
            {
                MessageBox.Show("You put the same name");
                return;
            }

             for (int j = 0; j < tmpOb.Count; j++)
                 {
                     if (j != selectedIndex && sName == tmpOb[j].Name)
                     {
                         MessageBox.Show("There is already a playlist with this name");
                         return;
                     }              
                 }
             tmpPl.Name = sName;
             tmpNode.Header = sName;
             using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\playlist.xml", FileMode.Create))
             {
                 XmlSerializer xml = new XmlSerializer(typeof(List<Playlist>));
                 xml.Serialize(fs, tmpOb);
             }
             this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
