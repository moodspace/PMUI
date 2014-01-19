using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Postmodern_UI
{
    public partial class InputBox : UserControl
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private String f1_, f2_, f3_;
        private Control dataControl;
        private String missionType;

        public InputBox(String Field1, String Field2, String Field3, String f1_preload, String f2_preload, Control control, String type)
        {
            InitializeComponent();
            label1.Text = Field1;
            label2.Text = Field2;
            label3.Text = Field3;

            textField1.Text = f1_preload;
            textField2.Text = f2_preload;

            dataControl = control;
            missionType = type;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (f1_ == String.Empty || f2_ == String.Empty || comboBox1.Text == String.Empty)
            {
                textField1.BackColor = textField2.BackColor = Color.LightCoral;
            }
            else
            {
                f1_ = textField1.Text;
                f2_ = textField2.Text;
                f3_ = comboBox1.Text;
                textField1.BackColor = textField2.BackColor = Color.White;
                this.Hide();

                doAction();
            }
        }

        private void doAction()
        {
            switch (missionType)
            {
                case "TileWebsite":
                    {
                        TileWebsite tileWeb = (TileWebsite)dataControl;
                        tileWeb.IconURL = f1_;
                        tileWeb.getIconFromWeb();
                        tileWeb.WebsiteURL = f2_;
                        tileWeb.TSize = Settings.getTSize(f3_);
                        tileWeb.alignmanager.TryAdd(new Point(0, 0), tileWeb);
                        break;
                    }
            }

        }

        internal String[] getFieldContent()
        {
            return new String[] { f1_, f2_ };
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
        }
    }
}
