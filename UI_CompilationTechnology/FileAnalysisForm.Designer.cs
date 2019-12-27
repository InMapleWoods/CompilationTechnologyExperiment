namespace UI_CompilationTechnology
{
    partial class FileAnalysisForm
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
            this.sourceCodeTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sourceCodeTextBox
            // 
            this.sourceCodeTextBox.AcceptsTab = true;
            this.sourceCodeTextBox.BackColor = System.Drawing.Color.PaleGreen;
            this.sourceCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceCodeTextBox.Font = new System.Drawing.Font("Cascadia Code", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceCodeTextBox.Location = new System.Drawing.Point(0, 0);
            this.sourceCodeTextBox.Multiline = true;
            this.sourceCodeTextBox.Name = "sourceCodeTextBox";
            this.sourceCodeTextBox.ReadOnly = true;
            this.sourceCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sourceCodeTextBox.Size = new System.Drawing.Size(800, 450);
            this.sourceCodeTextBox.TabIndex = 0;
            // 
            // FileAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sourceCodeTextBox);
            this.Name = "FileAnalysisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FileAnalysisForm_FormClosing);
            this.Load += new System.EventHandler(this.FileAnalysisForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox sourceCodeTextBox;

    }
}