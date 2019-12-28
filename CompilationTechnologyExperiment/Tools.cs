using System.Collections;
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
            List<char> sp = "{}[](),;:_. ".ToList();
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
        /// 获取错误位置所在的单词
        /// </summary>
        /// <param name="input">源程序</param>
        /// <param name="index">出错位置</param>
        /// <returns>出错单词</returns>
        public static string GetWord(string input, int index)
        {
            if (input.Length == 0)
            {
                return "";
            }
            int start = 0;
            int end = 0;
            for (int i = index; i >= 0; i--)
            {
                if (IsBelongToSeparatorsOrOperators(input, i) && input[i] != '.')
                {
                    start = i + 1;
                    break;
                }
            }
            for (int i = index; i < input.Length; i++)
            {
                if (IsBelongToSeparatorsOrOperators(input, i) && input[i] != '.')
                {
                    end = i - 1;
                    break;
                }
            }
            if (end - start > 0)
            {
                return input.Substring(start, end - start);
            }
            return "";
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
        /// <param name="type">类型</param>
        /// <returns>数组内容</returns>
        public static IEnumerable GetJsonObject(string input, string type)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ErrorException(ErrorMessageResource.FileEmptyError);
            }
            if (type == "Token")
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Token>>(input);

            }
            else if (type == "Symbol")
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Symbol>>(input);
            }
            else if (type == "Formula")
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<QuaternaryFormula>>(input);
            }
            return null;
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

    /// <summary>
    /// Token类
    /// </summary>
    public class Token
    {
        /// <summary>
        /// 单词序号
        /// </summary>
        public int Label { set; get; }
        /// <summary>
        /// 单词本身
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 单词种别编码
        /// </summary>
        public int Code { set; get; }
        /// <summary>
        /// 单词在符号表中登记项的指针，仅用于标识符或常数，其他情况下为0
        /// </summary>
        public int Addr { set; get; }
        public Token()
        {
        }
        public Token(int Label, string Name, int Code, int Addr)
        {
            this.Label = Label;
            this.Name = Name;
            this.Code = Code;
            this.Addr = Addr;
        }
    }

    /// <summary>
    /// Symbol类
    /// </summary>
    public class Symbol
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Number { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 待用信息表
        /// </summary>
        public List<string[]> Wait { set; get; } = new List<string[]>();

        /// <summary>
        /// 寄存器值AVALUE
        /// </summary>
        public string Value { set; get; }
        /// <summary>
        /// 初始化
        /// </summary>
        public Symbol()
        {
            Value = "";
            Wait = new List<string[]>();
            if (Wait.Count == 0)
            {
                Wait.Add(new string[2] { "0", "N" });
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public Symbol(int Number, int Type, string Name, List<string[]> Wait, string Value)
        {
            this.Number = Number;
            this.Type = Type;
            this.Name = Name;
            this.Wait = Wait;
            this.Value = Value;
        }
    }

    /// <summary>
    /// 四元式
    /// </summary>
    public class QuaternaryFormula
    {
        /// <summary>
        /// 操作符
        /// </summary>
        public string op;
        /// <summary>
        /// 操作数1
        /// </summary>
        public string arg1;
        /// <summary>
        /// 操作数2
        /// </summary>
        public string arg2;
        /// <summary>
        /// 结果
        /// </summary>
        public string result;
        /// <summary>
        /// 基本块入口
        /// </summary>
        public bool enter { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public QuaternaryFormula()
        {

        }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="op">操作符</param>
        /// <param name="arg1">操作数1</param>
        /// <param name="arg2">操作数2</param>
        /// <param name="result">结果</param>
        public QuaternaryFormula(string op, string arg1, string arg2, string result)
        {
            this.op = op;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.result = result;
            enter = false;
        }
    }

    /// <summary>
    /// 表达式E
    /// </summary>
    public class E
    {
        /// <summary>
        /// 表达式为假时，所指向的链表
        /// </summary>
        public List<int> False { set; get; }

        /// <summary>
        /// 表达式为真时，所指向的链表
        /// </summary>
        public List<int> True { set; get; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public E()
        {
            this.False = new List<int>();
            this.True = new List<int>();
        }
    }

    /// <summary>
    /// 产生式S
    /// </summary>
    public class S
    {
        /// <summary>
        /// 下一个链表
        /// </summary>
        public List<int> next { set; get; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public S()
        {
            this.next = new List<int>();
        }
    }

    /// <summary>
    /// 汇编代码
    /// </summary>
    public class Assembly
    {
        /// <summary>
        /// 操作符
        /// </summary>
        public string Op { set; get; }
        /// <summary>
        /// 操作数1
        /// </summary>
        public string PL { set; get; }
        /// <summary>
        /// 操作数2
        /// </summary>
        public string PR { set; get; }
        /// <summary>
        /// 序号
        /// </summary>
        public string Num { set; get; }
        /// <summary>
        /// 三元式
        /// </summary>
        /// <param name="op">操作符</param>
        /// <param name="pl">操作数1</param>
        /// <param name="pr">操作数2</param>
        public Assembly(string op, string pl, string pr)
        {
            Op = op;
            PL = pl;
            PR = pr;
        }
    }
}
