using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 该字符串的该位置前是否是某句开头（分号或分号后全是空格）
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="index">位置索引</param>
        /// <returns>是否满足是某句开头</returns>
        public static bool IsSemicolon(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            if (string.IsNullOrWhiteSpace(input[index].ToString()))
                return false;
            if (index != 0)
            {
                for (int i = index - 1; i >= 0;)
                {
                    if (input[i] == ';')
                    {
                        return true;
                    }
                    else if (input[i] == ' ')
                    {
                        i--;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return true;
        }

        /// <summary>
        /// 是否属于界符或者算符
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="index">位置索引</param>
        /// <returns>是否属于算符或界符</returns>
        public static bool IsBelongToSeparatorsOrOperators(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            if (index >= input.Length)
                return false;
            List<char> op = "+-*%/=><".ToList();
            List<char> sp = "{}[](),;:_ ".ToList();
            if (op.Contains(input[index]) || sp.Contains(input[index]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否属于合法字符
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="index">位置索引</param>
        /// <returns>是否属于合法字符</returns>
        public static bool IsLegal(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            if (index >= input.Length)
                return false;
            List<char> legalsymbol = ".'\"".ToList();
            List<char> op = "+-*%/=><".ToList();
            List<char> sp = "{}[](),;:_ ".ToList();
            if (op.Contains(input[index]) || sp.Contains(input[index]) || legalsymbol.Contains(input[index]))
            {
                return true;
            }
            else if (char.IsLetter(input[index]))
            {
                return true;
            }
            else if (char.IsNumber(input[index]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 移动到行尾
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="index">位置索引</param>
        /// <returns>行尾索引</returns>
        public static int MoveToRowEnd(string input, int index)
        {
            if (string.IsNullOrEmpty(input))
                return 0;
            while (char.IsLetterOrDigit(input[index]) || !IsBelongToSeparatorsOrOperators(input, index))
            {
                if (index >= input.Length - 1)
                    return index;
                index++;
            }
            return index;
        }

        /// <summary>
        /// 获取机内码对应的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns>类型</returns>
        public static string GetSymbolType(string type)
        {
            return GetSymbolType(int.Parse(type));
        }
        /// <summary>
        /// 获取机内码对应的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns>类型</returns>
        public static string GetSymbolType(int type)
        {
            string result = "";
            switch (type)
            {
                case 3: result = "整型"; break;
                case 4: result = "字符型"; break;
                case 5: result = "布尔型"; break;
                case 6: result = "实型"; break;
                case 7: result = "字符串型"; break;
                case 22: result = "布尔型"; break;
                case 23: result = "布尔型"; break;
                case 41: result = "整型"; break;
                case 42: result = "实型"; break;
                case 43: result = "字符型"; break;
                case 45: result = "字符串型"; break;
                default: result = "error"; break;
            }
            if (result == "error")
            {
                throw new ErrorException(ErrorMessageResource.TypeNotFound);
            }
            return result;
        }

        /// <summary>
        /// 字符串获取数组
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>数组内容</returns>
        public static string[][] GetJsonObject(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ErrorException(ErrorMessageResource.FileEmptyError);
            }
            List<string[]> output = new List<string[]>();
            dynamic JSONObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(input);
            foreach (var i in JSONObject)
            {
                if (i.Count == 2)
                {
                    output.Add(new string[] { i[1], i[0] });
                }
                else if (i.Count == 3)
                {
                    output.Add(new string[] { i[1], i[0], i[2] });
                }
            }
            return output.ToArray();
        }
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>文件内容</returns>
        public static string GetFileContent(string fileName)
        {
            string output = string.Empty;
            string errorMessage = string.Empty;
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
            return output;
        }

    }

}
