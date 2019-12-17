using System.Collections.Generic;
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
            List<char> op = "+-*%/|&=!><".ToList();
            List<char> sp = "{}[](),; ".ToList();
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
            List<char> op = "+-*%/|&=!><".ToList();
            List<char> sp = "{}[](),; ".ToList();
            if (op.Contains(input[index]) || sp.Contains(input[index]))
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
            else if (input[index] == '.')
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
            while (input[index] != ';')
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
        public static string GetSymbolType(int type)
        {
            string result = "";
            switch (type)
            {
                case 34: result="整型";break;
                case 35: result= "浮点型"; break;
                case 36: result= "字符型"; break;
                case 37: result= "字符串型"; break;
                default: result= "error"; break;
            }
            if (result == "error")
            {
                throw new ErrorException(ErrorMessageResource.TypeNotFound);
            }
            return result;
        }

    }

}
