using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 扫描器类
    /// </summary>
    public static partial class FileScanner
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
            //不对空字符串进行处理
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            #region 消除换行和TAB
            string output = input.Replace("\r", "");
            output = output.Replace("\n", "");
            output = output.Replace("\t", "");
            #endregion
            #region 删除注释
            string commentPattern = @"(?<!/)/\*([^*/]|\*(?!/)|/(?<!\*))*((?=\*/))(\*/)";//注释匹配
            MatchCollection commentResult = Regex.Matches(output, commentPattern);
            for (int i = 0; i < commentResult.Count; i++)
            {
                output = output.Replace(commentResult[i].Groups[0].Value.ToString(), "");
            }
            #endregion
            return output;
        }

        /// <summary>
        /// 获取关键字和单词符号
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <returns>单词符号类别和内容</returns>
        public static List<KeyValuePair<string, int>> GetContentKeyValues(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            List<KeyValuePair<string, int>> result = new List<KeyValuePair<string, int>>();
            StringBuilder builder = new StringBuilder();
            int i = 0;
            do
            {
                if (Tools.IsSemicolon(input, i))//若字符为一行的开头
                {
                    builder.Clear();
                    builder.Append(input[i]);
                    i++;
                    continue;
                }
                if (identifyKeywordOrIdentifier(input, ref i, builder, result))
                {
                    continue;
                }
                if (identifyNumber(input, ref i, builder, result))
                {
                    continue;
                }
                if (identifyCharacter(input, ref i, builder, result))
                {
                    continue;
                }
                if (identifyString(input, ref i, builder, result))
                {
                    continue;
                }
                if (identifyOperatorsOrSeparators(input, ref i, builder, result))
                {
                    continue;
                }
            } while (i < input.Length);
            return result;
        }

        #region 具体识别方法
        /// <summary>
        /// 识别关键字和标识符
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">位置索引</param>
        /// <param name="builder">临时存储变量</param>
        /// <param name="result">返回结果</param>
        private static bool identifyKeywordOrIdentifier(string input, ref int index, StringBuilder builder, List<KeyValuePair<string, int>> result)
        {
            int start = index;
            if (char.IsNumber(input[start]))
                return false;
            bool returnValue = false;
            while (char.IsLetter(input[index])|| char.IsNumber(input[index]))
            {
                builder.Append(input[index]);
                if (!char.IsLetter(input[index+1]) && !char.IsNumber(input[index
                    +1]))
                {
                    string word = builder.ToString();
                    if (Keywords.ContainsKey(word))
                    {
                        int value;
                        Keywords.TryGetValue(word, out value);
                        result.Add(new KeyValuePair<string, int>(word, value));//返回关键字的种别编码
                    }
                    else
                    {
                        result.Add(new KeyValuePair<string, int>(word, 33));//返回标识符的种别编码33
                    }
                    builder.Clear();
                    returnValue = true;
                }
                index++;
                continue;
            }
            return returnValue;
        }
        /// <summary>
        /// 识别数值型常数
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">位置索引</param>
        /// <param name="builder">临时存储变量</param>
        /// <param name="result">返回结果</param>
        private static bool identifyNumber(string input, ref int index, StringBuilder builder, List<KeyValuePair<string, int>> result)
        {
            bool returnValue = false;
            while (char.IsNumber(input[index]))
            {
                bool isDouble = false;
                builder.Append(input[index]);
                if (!char.IsNumber(input[index + 1]))
                {
                    if (input[index + 1] == 'e' || input[index + 1] == 'E' || input[index + 1] == '.')
                    {
                        isDouble = true;
                        builder.Append(input[index + 1]);
                        index = index + 2;
                    }
                    else
                    {
                        string number = builder.ToString();
                        if (!isDouble)
                        {
                            result.Add(new KeyValuePair<string, int>(number, 34));
                        }
                        else
                        {
                            result.Add(new KeyValuePair<string, int>(number, 35));
                        }
                        builder.Clear();
                        returnValue = true;
                        index++;
                    }
                }
            }
            return returnValue;

        }
        /// <summary>
        /// 识别字符型常数
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">位置索引</param>
        /// <param name="builder">临时存储变量</param>
        /// <param name="result">返回结果</param>
        private static bool identifyCharacter(string input, ref int index, StringBuilder builder, List<KeyValuePair<string, int>> result)
        {
            bool returnValue = false;
            if (input[index] == '\'')
            {
                index = index + 1;
                while (input[index] != '\'')
                {
                    builder.Append(input[index]);
                    index++;
                }
                string charValue = builder.ToString();
                builder.Clear();
                returnValue = true;
                result.Add(new KeyValuePair<string, int>(charValue, 36));
                index++;
            }
            return returnValue;
        }
        /// <summary>
        /// 识别字符串型常数
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">位置索引</param>
        /// <param name="builder">临时存储变量</param>
        /// <param name="result">返回结果</param>
        private static bool identifyString(string input, ref int index, StringBuilder builder, List<KeyValuePair<string, int>> result)
        {
            bool returnValue = false;
            if (input[index] == '\"')
            {
                index = index + 1;
                while (input[index] != '\"')
                {
                    builder.Append(input[index]);
                    index++;
                }
                string stringValue = builder.ToString();
                builder.Clear();
                returnValue = true;
                result.Add(new KeyValuePair<string, int>(stringValue, 37));
                index++;
            }

            return returnValue;
        }
        /// <summary>
        /// 识别算符和界符
        /// </summary>
        /// <param name="input">输入内容</param>
        /// <param name="index">位置索引</param>
        /// <param name="builder">临时存储变量</param>
        /// <param name="result">返回结果</param>
        private static bool identifyOperatorsOrSeparators(string input, ref int index, StringBuilder builder, List<KeyValuePair<string, int>> result)
        {
            bool returnValue = false;
            while (Tools.IsBelongToSeparatorsOrOperators(input, index))
            {
                builder.Append(input[index]);
                if (input[index] == '>' || input[index] == '<' || input[index] == '!' || input[index] == '=')
                {
                    if (input[index + 1] == '=')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 2;
                    }
                }
                else if (input[index] == '|')
                {
                    if (input[index + 1] == '|')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 2;

                    }
                }
                else if (input[index] == '&')
                {
                    if (input[index + 1] == '&')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 2;

                    }
                }
                if (input[index] == '*')
                {
                    if (input[index + 1] == '/')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 2;

                    }
                }
                else if (input[index] == '/')
                {
                    if (input[index + 1] == '*')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 2;

                    }
                }
                string str = builder.ToString();
                builder.Clear();
                index++;
                returnValue = true;
                if (Operators.ContainsKey(str))
                {
                    int value;
                    Operators.TryGetValue(str, out value);
                    result.Add(new KeyValuePair<string, int>(str, value));//返回关键字的种别编码
                }
                if (Separators.ContainsKey(str))
                {
                    int value;
                    Separators.TryGetValue(str, out value);
                    result.Add(new KeyValuePair<string, int>(str, value));//返回关键字的种别编码
                }
            }

            return returnValue;
        }
        #endregion

        /// <summary>
        /// 获取文件内容并识别
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>识别结果</returns>

        public static List<KeyValuePair<string, int>> ScannerResult(string fileName)
        {
            return GetContentKeyValues(ProcessContent(GetFileContent(fileName)));
        }
    }
}
