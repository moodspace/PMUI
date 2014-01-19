namespace Postmodern_UI
{
    partial class PopupMenu
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
            this.menuItem1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // menuItem1
            // 
            this.menuItem1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.menuItem1.Location = new System.Drawing.Point(2, 5);
            this.menuItem1.Name = "menuItem1";
            this.menuItem1.Size = new System.Drawing.Size(166, 40);
            this.menuItem1.TabIndex = 0;
            this.menuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PopupMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.menuItem1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PopupMenu";
            this.Size = new System.Drawing.Size(170, 150);
            this.Load += new System.EventHandler(this.PopupMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label menuItem1;

    }
}
