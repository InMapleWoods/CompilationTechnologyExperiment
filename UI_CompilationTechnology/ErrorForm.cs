using System;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class ErrorForm : Form
    {
        public ErrorForm()
        {
            InitializeComponent();
        }
        public ErrorForm(string code)
        {
            InitializeComponent();
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0, 0);
        }

        private void ErrorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.isshowGenerateCodeForm = false;
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            MainForm.isshowGenerateCodeForm = true;
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