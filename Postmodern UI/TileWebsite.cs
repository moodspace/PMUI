using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class TileWebsite : Postmodern_UI.Tile
    {
        public TileWebsite(String[] textgroup, Settings.TSize TSize, Color tileColor, AlignManager am) :
            base(textgroup, TSize, tileColor, am)
        {
            InitializeComponent();
        }

        String url, icon_url;

        public String WebsiteURL { get { return url; } set { url = value; } }

        public String IconURL { get { return icon_url; } set { icon_url = value; } }

        public void getIconFromWeb()
        {
            this.icon = (Bitmap)Image.FromStream(WebRequest.Create(icon_url).GetResponse().GetResponseStream());
            this.refreshTile();
        }

        internal override void doAction()
        {
            System.Diagnostics.Process.Start(new System.Uri(this.url).ToString());
        }
    }
}
