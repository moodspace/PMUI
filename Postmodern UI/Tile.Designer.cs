﻿namespace Postmodern_UI
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
            this.SuspendLayout();
            // 
            // Tile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Font = new System.Drawing.Font("Segoe UI Light", 9.5F);
            this.Name = "Tile";
            this.Load += new System.EventHandler(this.Tile_Load);
            this.DoubleClick += new System.EventHandler(this.Tile_DoubleClick);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Tile_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
