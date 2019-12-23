using System.Collections.Generic;

namespace CompilationTechnologyExperiment
{
    public static class SyntaxAnalysis
    {
        private static string[][] GetSymbol()
        {
            string[][] Symbols = Tools.GetJsonObject(Tools.GetFileContent("Symbol.txt"));
            return Symbols;
        }
        private static string[][] GetToken()
        {
            string[][] Tokens = Tools.GetJsonObject(Tools.GetFileContent("Token.txt"));
            List<string[]> list = new List<string[]>();
            foreach (var i in Tokens)
            {
                int attr = GetAttrInSymbol(i[0]);
                list.Add(new string[] { i[0], i[1], attr.ToString() });
            }
            return list.ToArray();
        }

        private static int GetAttrInSymbol(string key)
        {
            string[][] list = GetSymbol();
            for (int i = 0; i < list.Length; i++)
            {
                if (key == list[i][0])
                {
                    return i;
                }
            }
            return -1;
        }
        void getbuf()
        {
        }
    }
}
