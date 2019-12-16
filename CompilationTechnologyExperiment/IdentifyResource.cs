using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilationTechnologyExperiment
{
    public static partial class FileScanner
    {
        static readonly Dictionary<string, int> Keywords = new Dictionary<string, int>()
            {
                {"int",1},{"double",2},{"char",3},{"bool",4},{"string",5},{"void",6},{"read",7},{"write",8},{"for",9},{"while",10},{"if",11},{"then",12},{"else",13},{"true",14},{"false",15}
            };
        static readonly Dictionary<string, int> Operators = new Dictionary<string, int>()
            {
                {"+",16},{"-",17},{"*",18},{"%",19},{"/",20},{"|",21},{"||",22},{"&",23},{"&&",24},{"!",25},{">",26},{"<",27},{"<=",28},{">=",29},{"=",30},{"==",31},{"!=",32}
            };
        static readonly Dictionary<string, int> Separators = new Dictionary<string, int>()
            {
                {"{",38},{"}",39},{"[",40},{"]",41},{"(",42},{")",43},{",",44},{";",45},{"/*",46},{"*/",47}
            };

    }
}
