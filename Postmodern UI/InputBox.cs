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

        private Label[] prompts;
        private TextBox[] textFields;
        private int operationType;
        private Control tile;

        // only allows textfields to be customized
        public InputBox(String[] fieldPrompt, String[] fieldPreload, Control tile, int operationType)
        {
            InitializeComponent();

            int fieldsCount = 0;

            prompts = new Label[fieldPrompt.Length];
            textFields = new TextBox[fieldPrompt.Length];

            switch (operationType)
            {
                case 1:
                    fieldsCount = 3; break;
            }

            for (int i = 0; i < fieldsCount; i++)
            {
                Label lbl = new Label();
                lbl.AutoSize = false;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Text = fieldPrompt[i];
                lbl.Left = 3;
                lbl.Top = 40 + i * 30;
                lbl.Width = label1.Width;
                lbl.Height = 24;
                this.Controls.Add(lbl);
                prompts[i] = lbl;

                TextBox txt = new TextBox();
                txt.Text = fieldPreload[i];
                txt.Left = comboBox1.Left;
                txt.Top = 40 + i * 30;
                txt.Width = comboBox1.Width;
                this.Controls.Add(txt);
                textFields[i] = txt;

                this.Height += txt.Height + 6;
            }

            this.operationType = operationType;
            this.tile = tile;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool invalidInputs = false;

            if (comboBox1.Text == "") //fail to input size
                invalidInputs = true;

            foreach (TextBox field in textFields)
            {
                if (field.Text == "") {
                    field.BackColor = Color.LightCoral;
                    invalidInputs = true;
                }
                else
                    field.BackColor = Color.White;
            }

            if (!invalidInputs)
            {
                this.Hide();
                doAction();
            }
            
        }

        private void doAction()
        {
            switch (operationType)
            {
                case 1:
                    {
                        TileWebsite tileWeb = (TileWebsite)tile;
                        tileWeb.setText(new String[] { textFields[0].Text });
                        tileWeb.IconURL = textFields[1].Text;
                        tileWeb.getIconFromWeb();
                        tileWeb.WebsiteURL = textFields[2].Text;
                        tileWeb.setTSize(Settings.getTSize(comboBox1.Text));
                        tileWeb.alignmanager.TryAdd(
                            tileWeb.alignmanager.getNextAvailable(new Point(0, 0), tileWeb.getTSize()), tileWeb, true);
                        break;
                    }
            }

        }

        private void InputBox_Load(object sender, EventArgs e)
        {
        }
    }
}
