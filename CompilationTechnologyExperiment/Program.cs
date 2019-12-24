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
                SyntaxAnalysis.GetToken();
                string path = Console.ReadLine();
                string[] result = FileScanner.ScannerResult(path);
                File.WriteAllText("Token.txt", result[0]);
                File.WriteAllText("Symbol.txt", result[1]);
                File.WriteAllText("Error.txt", result[2]);
                Console.WriteLine("Token文件和符号表文件已经生成在程序所在目录");
            }
            catch { }
            Console.Read();
        }
    }
}
