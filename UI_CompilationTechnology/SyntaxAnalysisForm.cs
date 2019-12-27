using System;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class SyntaxAnalysisForm : Form
    {
        public SyntaxAnalysisForm()
        {
            InitializeComponent();
        }
        public SyntaxAnalysisForm(string code)
        {
            InitializeComponent();
            sourceCodeTextBox.Text = code;
            sourceCodeTextBox.Select(0, 0);
        }

        private void SyntaxAnalysisForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.isshowSyntaxAnalysisForm = false;
        }

        private void SyntaxAnalysisForm_Load(object sender, EventArgs e)
        {
            MainForm.isshowSyntaxAnalysisForm = true;
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