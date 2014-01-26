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
        Object[] data;

        public TileWebsite(Object[] data, AlignManager am) :
            base(data, am)
        {
            InitializeComponent();
            this.data = data;
        }

        String url, icon_url;

        public String WebsiteURL { get { return url; } set { url = value; } }

        public String IconURL { get { return icon_url; } set { icon_url = value; } }

        public void getIconFromWeb()
        {
            Bitmap icon = (Bitmap)Image.FromStream(WebRequest.Create(icon_url).GetResponse().GetResponseStream());
            if (data[1] == null || data[1] is Bitmap)
            {
                data[1] = icon;
            }
            else
            {
                ((Object[])data[1])[0] = icon;
            }
            
            this.refreshTile();
        }

        internal override void doAction()
        {
            System.Diagnostics.Process.Start(new System.Uri(this.url).ToString());
        }
    }
}
