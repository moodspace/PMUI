namespace Postmodern_UI
{
    partial class Tile
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.iconBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).BeginInit();
            this.SuspendLayout();
            // 
            // iconBox
            // 
            this.iconBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.iconBox.BackColor = System.Drawing.Color.Transparent;
            this.iconBox.Location = new System.Drawing.Point(0, -15);
            this.iconBox.Name = "iconBox";
            this.iconBox.Size = new System.Drawing.Size(150, 165);
            this.iconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconBox.TabIndex = 0;
            this.iconBox.TabStop = false;
            this.iconBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.iconBox_MouseClick);
            this.iconBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.iconBox_MouseDown);
            this.iconBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.iconBox_MouseMove);
            this.iconBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.iconBox_MouseUp);
            // 
            // Tile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.iconBox);
            this.Name = "Tile";
            this.Load += new System.EventHandler(this.Tile_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.iconBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox iconBox;
    }
}
