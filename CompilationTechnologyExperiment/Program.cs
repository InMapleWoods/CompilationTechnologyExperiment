using System;
using System.IO;

namespace CompilationTechnologyExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("请输入代码文件地址");
                string path = Console.ReadLine();
                FileScanner fileScanner = new FileScanner();
                SyntaxAnalysis syntaxAnalysis = new SyntaxAnalysis();
                GenerateAssemblyCode generateAssemblyCode = new GenerateAssemblyCode();
                string[] scannerResult = fileScanner.ScannerResult(path);
                File.WriteAllText("Token.txt", scannerResult[0]);
                File.WriteAllText("Symbol.txt", scannerResult[1]);
                File.WriteAllText("Error.txt", scannerResult[2]);
                Console.WriteLine("Token文件和符号表文件已经生成在程序所在目录");
                Console.WriteLine("按任意键进行语法分析");
                Console.Read();
                Console.Read();
                bool syntaxAnalysisResult = syntaxAnalysis.AnalysisResult(fileScanner.tokens, fileScanner.symbols);
                if (!syntaxAnalysisResult)
                {
                    Console.WriteLine(syntaxAnalysis.GetErrorMessage());
                }
                Console.WriteLine("按任意键进行目标代码生成");
                Console.Read();
                Console.Read();
                generateAssemblyCode.GenerateCode(syntaxAnalysis.formulas, syntaxAnalysis.symbols, syntaxAnalysis.basicBlock);
                Console.WriteLine("目标代码文件已经生成在程序所在目录");
            }
            catch (Exception e) { Console.WriteLine(e); }
            Console.Read();
            Console.Read();
        }
    }
}
