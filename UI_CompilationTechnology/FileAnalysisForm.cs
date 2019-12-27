using System;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class FileAnalysisForm : Form
    {
        public FileAnalysisForm()
        {
            InitializeComponent();
        }
        public FileAnalysisForm(string code)
        {
            InitializeComponent();
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0, 0);
        }

        private void FileAnalysisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.isshowFileAnalysisForm = false;
        }

        private void FileAnalysisForm_Load(object sender, EventArgs e)
        {
            MainForm.isshowFileAnalysisForm = true;
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
    }
}