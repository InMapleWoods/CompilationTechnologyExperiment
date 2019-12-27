namespace UI_CompilationTechnology
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MainStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFileStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveSourceStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MaintoolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileScannerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AnalysisStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileScannerStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ShowStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyntaxAnalysisStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveResultStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyntaxToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.ShowSyntaxtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateAssemblyCodeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveCodetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CodetoolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.GenerateCodetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSourceFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.mainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MainStripMenuItem,
            this.FileScannerStripMenuItem,
            this.SyntaxAnalysisStripMenuItem,
            this.GenerateAssemblyCodeStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(800, 28);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // MainStripMenuItem
            // 
            this.MainStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewFileStripMenuItem,
            this.OpenStripMenuItem,
            this.SaveSourceStripMenuItem,
            this.MaintoolStripSeparator1,
            this.ExitStripMenuItem});
            this.MainStripMenuItem.Name = "MainStripMenuItem";
            this.MainStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.MainStripMenuItem.Text = "文件";
            // 
            // NewFileStripMenuItem
            // 
            this.NewFileStripMenuItem.Name = "NewFileStripMenuItem";
            this.NewFileStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.NewFileStripMenuItem.Text = "新建";
            this.NewFileStripMenuItem.Click += new System.EventHandler(this.NewFileStripMenuItem_Click);
            // 
            // OpenStripMenuItem
            // 
            this.OpenStripMenuItem.Name = "OpenStripMenuItem";
            this.OpenStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.OpenStripMenuItem.Text = "打开";
            this.OpenStripMenuItem.Click += new System.EventHandler(this.OpenStripMenuItem_Click);
            // 
            // SaveSourceStripMenuItem
            // 
            this.SaveSourceStripMenuItem.Name = "SaveSourceStripMenuItem";
            this.SaveSourceStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.SaveSourceStripMenuItem.Text = "保存";
            this.SaveSourceStripMenuItem.Click += new System.EventHandler(this.SaveSourceStripMenuItem_Click);
            // 
            // MaintoolStripSeparator1
            // 
            this.MaintoolStripSeparator1.Name = "MaintoolStripSeparator1";
            this.MaintoolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // ExitStripMenuItem
            // 
            this.ExitStripMenuItem.Name = "ExitStripMenuItem";
            this.ExitStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.ExitStripMenuItem.Text = "退出";
            this.ExitStripMenuItem.Click += new System.EventHandler(this.ExitStripMenuItem_Click);
            // 
            // FileScannerStripMenuItem
            // 
            this.FileScannerStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AnalysisStripMenuItem,
            this.FileScannerStripSeparator,
            this.ShowStripMenuItem});
            this.FileScannerStripMenuItem.Name = "FileScannerStripMenuItem";
            this.FileScannerStripMenuItem.Size = new System.Drawing.Size(83, 24);
            this.FileScannerStripMenuItem.Text = "词法分析";
            // 
            // AnalysisStripMenuItem
            // 
            this.AnalysisStripMenuItem.Name = "AnalysisStripMenuItem";
            this.AnalysisStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.AnalysisStripMenuItem.Text = "保存分析结果";
            this.AnalysisStripMenuItem.Click += new System.EventHandler(this.AnalysisStripMenuItem_Click);
            // 
            // FileScannerStripSeparator
            // 
            this.FileScannerStripSeparator.Name = "FileScannerStripSeparator";
            this.FileScannerStripSeparator.Size = new System.Drawing.Size(209, 6);
            // 
            // ShowStripMenuItem
            // 
            this.ShowStripMenuItem.Name = "ShowStripMenuItem";
            this.ShowStripMenuItem.Size = new System.Drawing.Size(212, 26);
            this.ShowStripMenuItem.Text = "显示词法分析结果";
            this.ShowStripMenuItem.Click += new System.EventHandler(this.ShowStripMenuItem_Click);
            // 
            // SyntaxAnalysisStripMenuItem
            // 
            this.SyntaxAnalysisStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveResultStripMenuItem,
            this.SyntaxToolStripSeparator,
            this.ShowSyntaxtoolStripMenuItem});
            this.SyntaxAnalysisStripMenuItem.Name = "SyntaxAnalysisStripMenuItem";
            this.SyntaxAnalysisStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.SyntaxAnalysisStripMenuItem.Text = "语法语义分析";
            // 
            // SaveResultStripMenuItem
            // 
            this.SaveResultStripMenuItem.Name = "SaveResultStripMenuItem";
            this.SaveResultStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.SaveResultStripMenuItem.Text = "保存分析结果";
            this.SaveResultStripMenuItem.Click += new System.EventHandler(this.SaveResultStripMenuItem_Click);
            // 
            // SyntaxToolStripSeparator
            // 
            this.SyntaxToolStripSeparator.Name = "SyntaxToolStripSeparator";
            this.SyntaxToolStripSeparator.Size = new System.Drawing.Size(221, 6);
            // 
            // ShowSyntaxtoolStripMenuItem
            // 
            this.ShowSyntaxtoolStripMenuItem.Name = "ShowSyntaxtoolStripMenuItem";
            this.ShowSyntaxtoolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.ShowSyntaxtoolStripMenuItem.Text = "显示语义分析结果";
            this.ShowSyntaxtoolStripMenuItem.Click += new System.EventHandler(this.ShowSyntaxtoolStripMenuItem_Click);
            // 
            // GenerateAssemblyCodeStripMenuItem
            // 
            this.GenerateAssemblyCodeStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveCodetoolStripMenuItem,
            this.CodetoolStripSeparator,
            this.GenerateCodetoolStripMenuItem});
            this.GenerateAssemblyCodeStripMenuItem.Name = "GenerateAssemblyCodeStripMenuItem";
            this.GenerateAssemblyCodeStripMenuItem.Size = new System.Drawing.Size(113, 24);
            this.GenerateAssemblyCodeStripMenuItem.Text = "目标代码生成";
            // 
            // SaveCodetoolStripMenuItem
            // 
            this.SaveCodetoolStripMenuItem.Name = "SaveCodetoolStripMenuItem";
            this.SaveCodetoolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.SaveCodetoolStripMenuItem.Text = "保存目标代码";
            this.SaveCodetoolStripMenuItem.Click += new System.EventHandler(this.SaveCodetoolStripMenuItem_Click);
            // 
            // CodetoolStripSeparator
            // 
            this.CodetoolStripSeparator.Name = "CodetoolStripSeparator";
            this.CodetoolStripSeparator.Size = new System.Drawing.Size(179, 6);
            // 
            // GenerateCodetoolStripMenuItem
            // 
            this.GenerateCodetoolStripMenuItem.Name = "GenerateCodetoolStripMenuItem";
            this.GenerateCodetoolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.GenerateCodetoolStripMenuItem.Text = "显示生成代码";
            this.GenerateCodetoolStripMenuItem.Click += new System.EventHandler(this.GenerateCodetoolStripMenuItem_Click);
            // 
            // openSourceFileDialog
            // 
            this.openSourceFileDialog.DefaultExt = "*.*|*.txt";
            this.openSourceFileDialog.FileName = "Source.txt";
            this.openSourceFileDialog.Title = "打开源文件";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "L语言编译器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MainStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator MaintoolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ExitStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem FileScannerStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AnalysisStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator FileScannerStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem ShowStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SyntaxAnalysisStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveResultStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator SyntaxToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem ShowSyntaxtoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateAssemblyCodeStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateCodetoolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator CodetoolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem SaveCodetoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewFileStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveSourceStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openSourceFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

