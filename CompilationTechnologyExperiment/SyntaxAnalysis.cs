using System.Collections.Generic;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 语法分析器类
    /// </summary>
    public static class SyntaxAnalysis
    {
        /// <summary>
        /// 获取符号表
        /// </summary>
        /// <returns>符号表</returns>
        private static string[][] GetSymbol()
        {
            string[][] Symbols = Tools.GetJsonObject(Tools.GetFileContent("Symbol.txt"));
            return Symbols;
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns>Token数组</returns>
        public static string[][] GetToken()
        {
            string[][] Tokens = Tools.GetJsonObject(Tools.GetFileContent("Token.txt"));
            List<string[]> list = new List<string[]>();
            for (int i = 0; i < Tokens.Length; i++)
            {
                int attr = GetAttrInSymbol(i);
                list.Add(new string[] { Tokens[i][0], Tokens[i][1], attr.ToString() });
            }
            return list.ToArray();
        }

        /// <summary>
        /// 获取Token对应Symbol表中的索引
        /// </summary>
        /// <param name="key">Token的索引</param>
        /// <returns>对应Symbol表中的索引</returns>
        private static int GetAttrInSymbol(int key)
        {
            string[][] list = GetSymbol();
            for (int i = 0; i < list.Length; i++)
            {
                if (key == int.Parse(list[i][2]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 位置索引
        /// </summary>
        private static int index = 0;
        public static bool AnalysisResult()
        {
            string[][] token = GetToken();//token[0]="real" token[1]=42 token[2]=attr

        }
        bool program(string[][] token)
        {
            proghead();
            block();
            if () { }
        }
    }
}
