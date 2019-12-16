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
            var res = FileScanner.ScannerResult(@"C:\Users\96464\Desktop\00.txt");
            foreach(var i in res)
            {
                Console.WriteLine("<" + i.Value + "," + i.Key + ">");
            }
            Console.Read();
        }
    }
}
