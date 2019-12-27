using System;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class SourceCodeForm : Form
    {
        public SourceCodeForm()
        {
            InitializeComponent();
        }
        public SourceCodeForm(string code)
        {
            InitializeComponent();
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0,0);
        }

        private void SourceCodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.isshowSourceCodeForm = false;
        }

        private void SourceCodeForm_Load(object sender, EventArgs e)
        {
            MainForm.isshowSourceCodeForm = true;
        }

        public void SetCode(string code)
        {
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0, 0);
        }
        public string GetCode()
        {
            return sourceCodeTextBox.Text;
        }

        private void FileScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is MainForm)
            {
                ((MainForm)MdiParent).FileScan();
            }
        }

        private void SyntaxAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is MainForm)
            {
                ((MainForm)MdiParent).AnalysisSyntax();
            }
        }

        private void GenerateCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MdiParent is MainForm)
            {
                ((MainForm)MdiParent).GenerateCode();
            }
        }
    }
}
