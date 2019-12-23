using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationTechnologyExperiment
{
    
    class SyntaxAnalysis
    {
        class token
        {
            int label;
            string name;
            int code;
            int addr;
            int getlabel() { return label; }
            string getname() { return name;}
            int getcode() { return code; }
            int getaddr() { return addr; }
            token() { label = 0;code = 0;addr = 0; }
        }
        Boolean SyntaxAnalaysisResult()
        {
            getbuf();
            program();
            Console.WriteLine("YES\n");
            return (true);
        }
       int i = 0;
       token t = new token();
       void getbuf()
        {
            
        }
        Boolean program()
        {
            proghead();
            block();
            if()
        }
    }
}
