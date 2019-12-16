using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationTechnologyExperiment
{
    class Program
    {
        static void Main(string[] args)
        {
            string output=FileScanner.GetFileContent(@"C:\Users\96464\Desktop\000.py");
            //string test = "int abs=0;  double s=1;";
            //for(int i=0;i<test.Length;i++)
            //{
            //    Console.WriteLine(Tools.IsSemicolon(test, i));
            //}
            Console.WriteLine(FileScanner.ProcessContent(output));
            Console.Read();
        }
    }
}
