using System.Collections.Generic;

namespace CompilationTechnologyExperiment
{
    public static partial class FileScanner
    {
        static readonly Dictionary<string, int> Keywords = new Dictionary<string, int>()
            {
                {"begin",1},{"end",2},{"integer",3},{"char",4},{"bool",5},{"real",6},{"string",7},{"input",8},{"output",9},{"program",10},{"read",11},{"write",12},{"for",13},{"to",14},{"while",15},{"do",16},{"repeat",17},{"until",18},{"if",19},{"then",20},{"else",21},{"true",22},{"false",23},{"var",24},{"const",25},{"and",26},{"or",27},{"not",28}
            };
        static readonly Dictionary<string, int> Operators = new Dictionary<string, int>()
            {
                {"+",29},{"-",30},{"*",31},{"/",32},{"=",33},{"<",34},{">",35},{"<=",36},{">=",37},{"<>",38},{":=",39}
            };
        static readonly Dictionary<string, int> Separators = new Dictionary<string, int>()
            {
                {"(",46},{")",47},{":",48},{".",49},{";",50},{",",51},{"_",52},{"'",53},{"\"",54},{"/*",55},{"*/",56}
            };

    }
}
