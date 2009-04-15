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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miExit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetUrl = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miInteractive = new System.Windows.Forms.ToolStripMenuItem();
            this.miView = new System.Windows.Forms.ToolStripMenuItem();
            this.miRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miFullscreen = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrSnapshot = new System.Windows.Forms.Timer(this.components);
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.webBrowser = new Flowered.UI.Controls.BuriedWebBrowser();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
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
            this.miSnapshot,
            this.toolStripSeparator2,
            this.miExit});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(35, 20);
            this.miFile.Text = "&File";
            // 
            // miSnapshot
            // 
            this.miSnapshot.Name = "miSnapshot";
            this.miSnapshot.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this.miSnapshot.Size = new System.Drawing.Size(149, 22);
            this.miSnapshot.Text = "&Snapshot";
            this.miSnapshot.Click += new System.EventHandler(this.miSnapshot_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(146, 6);
            // 
            // miExit
            // 
            this.miExit.Image = global::Flowered.App.SimpleClient.Properties.Resources.door_out;
            this.miExit.Name = "miExit";
            this.miExit.Size = new System.Drawing.Size(149, 22);
            this.miExit.Text = "&Exit";
            this.miExit.Click += new System.EventHandler(this.miExit_Click);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSetUrl,
            this.toolStripSeparator3,
            this.miInteractive});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(37, 20);
            this.miEdit.Text = "&Edit";
            // 
            // miSetUrl
            // 
            this.miSetUrl.Image = global::Flowered.App.SimpleClient.Properties.Resources.world_edit;
            this.miSetUrl.Name = "miSetUrl";
            this.miSetUrl.Size = new System.Drawing.Size(152, 22);
            this.miSetUrl.Text = "Set &URL...";
            this.miSetUrl.Click += new System.EventHandler(this.miSetUrl_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(149, 6);
            // 
            // miInteractive
            // 
            this.miInteractive.Name = "miInteractive";
            this.miInteractive.Size = new System.Drawing.Size(152, 22);
            this.miInteractive.Text = "&Interactive";
            this.miInteractive.Click += new System.EventHandler(this.miInteractive_Click);
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
            this.miRefresh.Image = global::Flowered.App.SimpleClient.Properties.Resources.arrow_refresh;
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
            this.miFullscreen.Image = global::Flowered.App.SimpleClient.Properties.Resources.application;
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
            this.miAbout.Image = global::Flowered.App.SimpleClient.Properties.Resources.information;
            this.miAbout.Name = "miAbout";
            this.miAbout.Size = new System.Drawing.Size(126, 22);
            this.miAbout.Text = "&About...";
            this.miAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // tmrSnapshot
            // 
            this.tmrSnapshot.Tick += new System.EventHandler(this.tmrSnapshot_Tick);
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // webBrowser
            // 
            this.webBrowser.Buried = true;
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 24);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(555, 388);
            this.webBrowser.TabIndex = 1;
            this.webBrowser.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webBrowser.PreviewKeyDown += new Flowered.UI.Controls.PreviewKeyDownHandler(this.webBrowser_PreviewKeyDown);
            this.webBrowser.MouseMove += new System.Windows.Forms.MouseEventHandler(this.webBrowser_MouseMove);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 412);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMain";
            this.Text = "Flowered";
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
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
        private System.Windows.Forms.Timer tmrSnapshot;
        private System.Windows.Forms.Timer tmrRefresh;
        private Flowered.UI.Controls.BuriedWebBrowser webBrowser;
        private System.Windows.Forms.ToolStripMenuItem miEdit;
        private System.Windows.Forms.ToolStripMenuItem miSetUrl;
        private System.Windows.Forms.ToolStripMenuItem miSnapshot;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem miInteractive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

