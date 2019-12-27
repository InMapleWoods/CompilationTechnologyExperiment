using CompilationTechnologyExperiment;
using System;
using System.IO;
using System.Windows.Forms;

namespace UI_CompilationTechnology
{
    public partial class MainForm : Form
    {
        public static bool isshowSourceCodeForm = false;
        public static bool isshowFileAnalysisForm = false;
        public static bool isshowSyntaxAnalysisForm = false;
        public static bool isshowGenerateCodeForm = false;
        public static bool isshowErrorForm = false;
        private SourceCodeForm sourceCodeForm = null;
        private FileAnalysisForm fileAnalysisForm = null;
        private SyntaxAnalysisForm syntaxAnalysisForm = null;
        private GenerateCodeForm generateCodeForm = null;
        private ErrorForm errorForm = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void OpenStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openSourceFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openSourceFileDialog.FileName;
                string sourceCode = Tools.GetFileContent(fileName);
                if (!isshowSourceCodeForm)
                {
                    sourceCodeForm = new SourceCodeForm(sourceCode);
                    sourceCodeForm.MdiParent = this;
                    sourceCodeForm.Show();
                }
                else
                {
                    sourceCodeForm.SetCode(sourceCode);
                }
            }
        }

        private void ExitStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewFileStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!isshowSourceCodeForm)
            {
                sourceCodeForm = new SourceCodeForm();
                sourceCodeForm.MdiParent = this;
                sourceCodeForm.Show();
            }
            else
            {
                sourceCodeForm.SetCode("");
            }
        }

        private void SaveSourceStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (isshowSourceCodeForm)
                {
                    File.WriteAllText(saveFileDialog.FileName, sourceCodeForm.GetCode());
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void AnalysisStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (isshowFileAnalysisForm)
                {
                    File.WriteAllText(saveFileDialog.FileName, fileAnalysisForm.GetCode());
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void SaveResultStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (isshowSyntaxAnalysisForm)
                {
                    File.WriteAllText(saveFileDialog.FileName, syntaxAnalysisForm.GetCode());
                    MessageBox.Show("保存成功");
                }
            }
        }
        private void SaveCodetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (isshowGenerateCodeForm)
                {
                    File.WriteAllText(saveFileDialog.FileName, generateCodeForm.GetCode());
                    MessageBox.Show("保存成功");
                }
            }

        }

        private void GenerateCodetoolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateCode();
        }

        public void GenerateCode()
        {
            if (isshowSourceCodeForm)
            {
                string sourceCode = sourceCodeForm.GetCode();
                FileScanner fileScanner = new FileScanner();
                var values = fileScanner.GetContentKeyValues(fileScanner.ProcessContent(sourceCode));
                fileScanner.GetTokens(values);
                fileScanner.GetSymbols();
                SyntaxAnalysis syntaxAnalysis = new SyntaxAnalysis();
                syntaxAnalysis.AnalysisResult(fileScanner.tokens, fileScanner.symbols);
                GenerateAssemblyCode generateAssemblyCode = new GenerateAssemblyCode();
                var res= generateAssemblyCode.GenerateCode(syntaxAnalysis.formulas, syntaxAnalysis.symbols, syntaxAnalysis.basicBlock);
                if (res)
                {
                    var generateCodeResult = generateAssemblyCode.GetAssembly();
                    if (!isshowGenerateCodeForm)
                    {
                        generateCodeForm = new GenerateCodeForm(generateCodeResult);
                        generateCodeForm.MdiParent = this;
                        generateCodeForm.Show();
                    }
                    else
                    {
                        generateCodeForm.SetCode(generateCodeResult);
                    }
                }
                else
                {
                    MessageBox.Show("目标代码生成发生错误");
                    if (!isshowErrorForm)
                    {
                        errorForm = new ErrorForm(generateAssemblyCode.error);
                        errorForm.MdiParent = this;
                        errorForm.Show();
                    }
                    else
                    {
                        errorForm.SetCode(generateAssemblyCode.error);
                    }
                }
            }
        }

        private void ShowStripMenuItem_Click(object sender, EventArgs e)
        {
            FileScan();
        }

        public void FileScan()
        {
            if (isshowSourceCodeForm)
            {
                string sourceCode = sourceCodeForm.GetCode();
                FileScanner fileScanner = new FileScanner();
                var values = fileScanner.GetContentKeyValues(fileScanner.ProcessContent(sourceCode));
                fileScanner.GetTokens(values);
                fileScanner.GetSymbols();
                string scannerResult = fileScanner.GetTokenFile(fileScanner.tokens);
                if (fileScanner.error.Length == 0)
                {
                    if (!isshowFileAnalysisForm)
                    {
                        fileAnalysisForm = new FileAnalysisForm(scannerResult);
                        fileAnalysisForm.MdiParent = this;
                        fileAnalysisForm.Show();
                    }
                    else
                    {
                        fileAnalysisForm.SetCode(scannerResult);
                    }
                }
                else
                {
                    fileScanner.error = fileScanner.error.Substring(0, fileScanner.error.Length - 1);
                    if (!string.IsNullOrEmpty(fileScanner.error))
                        fileScanner.error += "]";
                    if (!isshowErrorForm)
                    {
                        errorForm = new ErrorForm(fileScanner.error);
                        errorForm.MdiParent = this;
                        errorForm.Show();
                    }
                    else
                    {
                        errorForm.SetCode(fileScanner.error);
                    }
                }
            }
        }

        private void ShowSyntaxtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnalysisSyntax();
        }

        public void AnalysisSyntax()
        {
            if (isshowSourceCodeForm)
            {
                string sourceCode = sourceCodeForm.GetCode();
                FileScanner fileScanner = new FileScanner();
                var values = fileScanner.GetContentKeyValues(fileScanner.ProcessContent(sourceCode));
                fileScanner.GetTokens(values);
                fileScanner.GetSymbols();
                SyntaxAnalysis syntaxAnalysis = new SyntaxAnalysis();
                var syntaxAnalysisResult = syntaxAnalysis.AnalysisResult(fileScanner.tokens, fileScanner.symbols);
                if (syntaxAnalysisResult)
                {
                    MessageBox.Show("语法分析通过");
                    if (!isshowSyntaxAnalysisForm)
                    {
                        syntaxAnalysisForm = new SyntaxAnalysisForm(syntaxAnalysis.GetQuaternaryFormulaFile(syntaxAnalysis.formulas));
                        syntaxAnalysisForm.MdiParent = this;
                        syntaxAnalysisForm.Show();
                    }
                    else
                    {
                        syntaxAnalysisForm.SetCode(syntaxAnalysis.GetQuaternaryFormulaFile(syntaxAnalysis.formulas));
                    }
                }
                else
                {
                    MessageBox.Show("语法分析出错");
                    if (!isshowErrorForm)
                    {
                        errorForm = new ErrorForm(syntaxAnalysis.GetErrorMessage());
                        errorForm.MdiParent = this;
                        errorForm.Show();
                    }
                    else
                    {
                        errorForm.SetCode(syntaxAnalysis.GetErrorMessage());
                    }
                }
            }
        }
    }
}
