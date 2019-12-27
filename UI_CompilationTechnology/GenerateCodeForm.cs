using System;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class GenerateCodeForm : Form
    {
        public GenerateCodeForm()
        {
            InitializeComponent();
        }
        public GenerateCodeForm(string code)
        {
            InitializeComponent();
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0, 0);
        }

        private void GenerateCodeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.isshowGenerateCodeForm = false;
        }

        private void GenerateCodeForm_Load(object sender, EventArgs e)
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