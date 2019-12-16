using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 扫描器类
    /// </summary>
    public static class FileScanner
    {
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件内容</returns>
        public static string GetFileContent(string fileName)
        {
            string output = string.Empty;
            string errorMessage = string.Empty;
            try
            {
                if (File.Exists(fileName) == true)
                {
                    output = File.ReadAllText(fileName);
                    if (string.IsNullOrEmpty(output))
                    {
                        throw new ErrorException(ErrorMessageResource.FileEmptyError);
                    }
                }
                else
                {
                    throw new ErrorException(ErrorMessageResource.FileNotExist);
                }
            }
            catch (ErrorException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return output;
        }

        /// <summary>
        /// 处理字符串内容
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <returns>处理后结果</returns>
        public static string ProcessContent(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            string output = input.Replace("\r\n", "");
            output = output.Replace("\t", "");
            return output;
        }

        /// <summary>
        /// 获取关键字和单词符号
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <returns>单词符号类别和内容</returns>
        public static Dictionary<int, string> GetContentKeyValues(string input)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            List<string> Keywords = new string[] { "if", "else", "while", "for", "double", "int", "string", "void", "true", "false" }.ToList();
            List<string> Operator = new string[] { "+", "-", "*", "%", "/", "|", "||", "&", "&&", "!", ">", "<", ">=", "<=", "=", "==", "!=" }.ToList();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                if (Tools.IsSemicolon(input, i))
                {
                    builder.Append(input[i]);
                }

            }
            return result;
        }
    }
}
