namespace Autoreplace
{
    partial class AutoreplaceForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listReplaces = new System.Windows.Forms.ListBox();
            this.textSearchString = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listReplaces
            // 
            this.listReplaces.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listReplaces.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.listReplaces.FormattingEnabled = true;
            this.listReplaces.ItemHeight = 14;
            this.listReplaces.Location = new System.Drawing.Point(-2, 20);
            this.listReplaces.Name = "listReplaces";
            this.listReplaces.Size = new System.Drawing.Size(631, 354);
            this.listReplaces.TabIndex = 1;
            this.listReplaces.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listReplaces_KeyUp);
            this.listReplaces.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listReplaces_MouseDoubleClick);
            // 
            // textSearchString
            // 
            this.textSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSearchString.Location = new System.Drawing.Point(-2, 0);
            this.textSearchString.Name = "textSearchString";
            this.textSearchString.Size = new System.Drawing.Size(631, 20);
            this.textSearchString.TabIndex = 0;
            this.textSearchString.TextChanged += new System.EventHandler(this.textSearchString_TextChanged);
            this.textSearchString.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textSearchString_KeyUp);
            // 
            // AutoreplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(627, 369);
            this.ControlBox = false;
            this.Controls.Add(this.textSearchString);
            this.Controls.Add(this.listReplaces);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 100);
            this.Name = "AutoreplaceForm";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Deactivate += new System.EventHandler(this.AutoreplaceForm_Deactivate);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listReplaces;
        private System.Windows.Forms.TextBox textSearchString;
    }
}