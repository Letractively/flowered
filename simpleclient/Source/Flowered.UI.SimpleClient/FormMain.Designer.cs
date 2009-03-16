namespace Flowered.UI.SimpleClient
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.transparentPanel = new Flowered.UI.Controls.TransparentPanel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miView,
            this.miHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(555, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetUrl,
            this.toolStripSeparator2,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(35, 20);
            this.miFile.Text = "&File";
            // 
            // miSetUrl
            // 
            this.miSetUrl.Image = global::Flowered.App.Standalone.Properties.Resources.world_edit;
            this.miSetUrl.Name = "miSetUrl";
            this.miSetUrl.Size = new System.Drawing.Size(135, 22);
            this.miSetUrl.Text = "Set URL...";
            this.miSetUrl.Click += new System.EventHandler(this.miSetUrl_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(132, 6);
            // 
            // miExit
            // 
            this.miExit.Image = global::Flowered.App.Standalone.Properties.Resources.door_out;
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(135, 22);
            this.miExit.Text = "&Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miView
            // 
            this.miView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miRefresh,
            this.toolStripSeparator1,
            this.miFullscreen});
            this.miView.Name = "miView";
            this.miView.Size = new System.Drawing.Size(41, 20);
            this.miView.Text = "&View";
            // 
            // miRefresh
            // 
            this.miRefresh.Image = global::Flowered.App.Standalone.Properties.Resources.arrow_refresh;
            this.miRefresh.Name = "miRefresh";
            this.miRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.miRefresh.Size = new System.Drawing.Size(158, 22);
            this.miRefresh.Text = "&Refresh";
            this.miRefresh.Click += new System.EventHandler(this.miRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // miFullscreen
            // 
            this.miFullscreen.Image = global::Flowered.App.Standalone.Properties.Resources.application;
            this.miFullscreen.Name = "miFullscreen";
            this.miFullscreen.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.miFullscreen.Size = new System.Drawing.Size(158, 22);
            this.miFullscreen.Text = "&Fullscreen";
            this.miFullscreen.Click += new System.EventHandler(this.miFullscreen_Click);
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.Size = new System.Drawing.Size(24, 20);
            this.miHelp.Text = "&?";
            // 
            // miAbout
            // 
            this.miAbout.Image = global::Flowered.App.Standalone.Properties.Resources.information;
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(126, 22);
            this.miAbout.Text = "&About...";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.AllowWebBrowserDrop = false;
            this.webBrowser.CausesValidation = false;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser.Location = new System.Drawing.Point(0, 24);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.ScriptErrorsSuppressed = true;
            this.webBrowser.ScrollBarsEnabled = false;
            this.webBrowser.Size = new System.Drawing.Size(555, 388);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.TabStop = false;
            this.webBrowser.Url = new System.Uri("", System.UriKind.Relative);
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // transparentPanel
            // 
            this.transparentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transparentPanel.Location = new System.Drawing.Point(0, 24);
            this.transparentPanel.Name = "transparentPanel";
            this.transparentPanel.Size = new System.Drawing.Size(555, 388);
            this.transparentPanel.TabIndex = 1;
            this.transparentPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.transparentPanel_MouseMove);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 412);
            this.Controls.Add(this.transparentPanel);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.Text = "Flowered";
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miHelp;
        private System.Windows.Forms.ToolStripMenuItem miExit;
        private System.Windows.Forms.ToolStripMenuItem miAbout;
        private System.Windows.Forms.ToolStripMenuItem miView;
        private System.Windows.Forms.ToolStripMenuItem miFullscreen;
        private System.Windows.Forms.ToolStripMenuItem miRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.ToolStripMenuItem miSetUrl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Flowered.UI.Controls.TransparentPanel transparentPanel;
    }
}

