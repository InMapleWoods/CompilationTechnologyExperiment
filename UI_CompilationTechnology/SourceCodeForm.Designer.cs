namespace UI_CompilationTechnology
{
    partial class SourceCodeForm
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
            this.sourceCodeTextBox = new System.Windows.Forms.TextBox();
            this.sourceContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.FileScannerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SyntaxAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceCodeTextBox
            // 
            this.sourceCodeTextBox.AcceptsTab = true;
            this.sourceCodeTextBox.BackColor = System.Drawing.Color.PowderBlue;
            this.sourceCodeTextBox.ContextMenuStrip = this.sourceContextMenuStrip;
            this.sourceCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTextBox.Font = new System.Drawing.Font("Cascadia Code", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceCodeTextBox.Location = new System.Drawing.Point(0, 0);
            this.sourceCodeTextBox.Multiline = true;
            this.sourceCodeTextBox.Name = "sourceCodeTextBox";
            this.sourceCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sourceCodeTextBox.Size = new System.Drawing.Size(800, 450);
            this.sourceCodeTextBox.TabIndex = 0;
            // 
            // sourceContextMenuStrip
            // 
            this.sourceContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sourceContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileScannerToolStripMenuItem,
            this.SyntaxAnalysisToolStripMenuItem,
            this.GenerateCodeToolStripMenuItem});
            this.sourceContextMenuStrip.Name = "sourceContextMenuStrip";
            this.sourceContextMenuStrip.Size = new System.Drawing.Size(211, 104);
            // 
            // FileScannerToolStripMenuItem
            // 
            this.FileScannerToolStripMenuItem.Name = "FileScannerToolStripMenuItem";
            this.FileScannerToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.FileScannerToolStripMenuItem.Text = "词法分析";
            this.FileScannerToolStripMenuItem.Click += new System.EventHandler(this.FileScannerToolStripMenuItem_Click);
            // 
            // SyntaxAnalysisToolStripMenuItem
            // 
            this.SyntaxAnalysisToolStripMenuItem.Name = "SyntaxAnalysisToolStripMenuItem";
            this.SyntaxAnalysisToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.SyntaxAnalysisToolStripMenuItem.Text = "语法语义分析";
            this.SyntaxAnalysisToolStripMenuItem.Click += new System.EventHandler(this.SyntaxAnalysisToolStripMenuItem_Click);
            // 
            // GenerateCodeToolStripMenuItem
            // 
            this.GenerateCodeToolStripMenuItem.Name = "GenerateCodeToolStripMenuItem";
            this.GenerateCodeToolStripMenuItem.Size = new System.Drawing.Size(210, 24);
            this.GenerateCodeToolStripMenuItem.Text = "目标代码生成";
            this.GenerateCodeToolStripMenuItem.Click += new System.EventHandler(this.GenerateCodeToolStripMenuItem_Click);
            // 
            // SourceCodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sourceCodeTextBox);
            this.Name = "SourceCodeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SourceCodeForm_FormClosing);
            this.Load += new System.EventHandler(this.SourceCodeForm_Load);
            this.sourceContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sourceCodeTextBox;
        private System.Windows.Forms.ContextMenuStrip sourceContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileScannerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SyntaxAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem GenerateCodeToolStripMenuItem;
    }
}