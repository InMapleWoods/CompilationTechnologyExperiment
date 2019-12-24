using System;
using System.Collections.Generic;

namespace CompilationTechnologyExperiment
{
    public static class SyntaxAnalysis
    {
        private static string error = "";
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
        private static int index = 0;

        public static bool AnalysisResult()
        {
            string[][] token = GetToken();//token[0]="real" token[1]=42 token[2]=attr
            Program(token);
            Console.WriteLine("YES\n");
            return true;
        }
        public static void Program(string[][] token)
        {
            Proghead(token);
            Block(token);
            if (token[index][1] == keyword.SEM)//判断“；”
            {
                index++;
            }
            else
            {
                error += "[\"miss a ';'\",\"" + index + "\"],";
                throw new ErrorException();
            }
        }
        public static void Proghead(string[][] token)
        {
            if (token[index][1] == keyword.PROGRAM)//"program"
            {
                index++;
                if (token[index][1] == keyword.ID)
                {
                    index++;
                    if (token[index][1] == keyword.SEM)
                    {
                        index++;
                    }
                    else
                    {
                        error += "[\"miss a ';'\",\"" + index + "\"],";
                        throw new ErrorException();
                    }
                }
                else
                {
                    error += "[\"error of ID\",\"" + index + "\"],";
                    throw new ErrorException();
                }
            }
            else
            {
                error += "[\"missing keyword program\",\"" + index + "\"],";
                throw new ErrorException();
            }
        }
        public static void Block(string[][] token)//程序体部分，常量说明，变量说明，语句部分（变量说明，复合句）
        {
            Consexpl(token);//常量说明
            Varexpl(token);//变量说明
            Compesent(token);//复合语句
        }
        public static void Consexpl(string[][] token)//常量说明部分：整数or实数
        {
            if (token[index][1] == keyword.整型 || token[index][1] == keyword.实型)
            {
                index++;
                Consdefi(token);
                if (token[index][1] == keyword.SEM)
                {
                    index++;
                }
                else
                {
                    error += "[\"miss a ';'\",\"" + index + "\"],";
                    throw new ErrorException();
                }
            }
        }
        public static void Consdefi(string[][] token)//常量定义：标识符+等号+无符号数
        {
            if (token[index][1] == keyword.ID)
            {
                index++;
                if (token[index][1] == keyword.EQU)
                {
                    index++;
                    if (token[index][1] == keyword.INTEGER)
                    {
                        index++;
                    }
                    else
                    {
                        error += "[\"miss INREGER\",\"" + index + "\"],";
                        throw new ErrorException();
                    }
                }
                else
                {
                    error += "[\"miss a '='\",\"" + index + "\"],";
                    throw new ErrorException();
                }
            }
            else
            {
                error += "[\"miss ID\",\"" + index + "\"],";
                throw new ErrorException();
            }
        }
        public static void Conssuff(string[][] token)//常量定义后缀：,+常量定义后缀
        {
            if (token[index][1] == keyword.DOUHAO)
            {
                index++;
                Consdefi(token);
                Conssuff(token);
                if (token[index][1] == keyword.SEM)
                {
                    index++;
                }
                else
                {
                    error += "[\"miss a ';'\",\"" + index + "\"],";
                    throw new ErrorException();
                }
            }
            return f;
        }
        public static bool Varexpl(string[][] token)//变量说明部分：var+变量定义+变量定义后缀
        {
            bool f = true;
            if (token[index][1] == keyword.VAR)
            {
                index++;
                if (!Vardefi(token)) { f = false; }
                if (!Varsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Vardefi(string[][] token)//变量定义：标识符+标识符后缀+：类型+；||标识符+标识符后缀+：+类型+；+变量定义
        {
            bool f = true;
            if (token[index][1] == keyword.ID)
            {
                index++;Idsuff(token)
                if (!) { f = false; }
                if (token[index][1] == keyword.MAOHAO)
                {
                    index++;
                    if (!Typeil(token)) { f = false; }
                    if (token[index][1] == keyword.SEM)
                    {
                        index++;
                    }
                    else
                    {
                        Console.WriteLine("miss ';'\n");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("miss ':'\n");
                    return false;
                }
            }
            return f;
        }
        public static bool Varsuff(string[][] token)//变量后缀：变量定义+变量后缀||空
        {
            bool f = true;
            if (token[index][1] == keyword.ID)
            {
                if (!Vardefi(token)) { f = false; }
                if (!Varsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Typeil(string[][] token)//类型
        {
            if (token[index][1] == keyword.REAL || token[index][1] == keyword.BOOL || token[index][1] == keyword.INTEGER)
            {
                index++;
            }
            else
            {
                Console.WriteLine("wrong type of define\n");
                return false;
            }
            return true;
        }
        public static bool Compesent(string[][] token)//复合语句:begin+语句表+end
        {
            bool f = true;
            if (token[index][1] == keyword.BEGIN)
            {
                index++;
                if (!Sentence(token)) { f = false; }
                if (!Sentsuff(token)) { f = false; }
                if (token[index][1] == keyword.END)
                {
                    index++;
                }
            }
            else
            {
                Console.WriteLine("miss Begin as the start/n");
                return false;
            }
            return f;
        }
        public static bool Sentence(string[][] token)//赋值语句，if语句，while语句，复合句
        {
            bool f = true;
            if (token[index][1] == keyword.ID)
            {
                if (!Assipro(token)) { f = false; }
            }
            else if (token[index][1] == keyword.IF)
            {
                if (!Ifsent(token)) { f = false; }
            }
            else if (token[index][1] == keyword.WHILE)
            {
                if (!Whilesent(token)) { f = false; }
            }
            else if (token[index][1] == keyword.BEGIN)
            {
                if (!Compesent(token)) { f = false; }
            }
            return f;
        }
        public static bool Sentsuff(string[][] token)//语句后缀：；+语句+语句后缀
        {
            bool f = true;
            if (token[index][1] == keyword.SEM)
            {
                index++;
                if (!Sentence(token)) { f = false; }
                if (!Sentsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Assipro(string[][] token)//赋值语句
        {
            bool f = true;
            if (token[index][1] == keyword.ID)
            {
                index++;
                if (!Suffix(token)) { f = false; }
            }
            else
            {
                Console.WriteLine("赋值语句错误\n");
                return false;
            }
            return f;
        }
        public static bool Suffix(string[][] token)//赋值号的后缀为赋值语句，否则为过程调用语句
        {
            bool f = true;
            if (token[index][1] == keyword.FUZHI)
            {
                index++;
                if (!Express(token)) { f = false; }

            }
            else if (token[index][1] == keyword.LKUOHAO)
            {
                index++;
                if (!Express(token)) { f = false; }
                if (token[index][1] == keyword.RKUOHAO)
                {
                    index++;
                }
                else
                {
                    Console.WriteLine("缺少右括号\n");
                    return false;
                }
            }
            return f;
        }
        public static bool Express(string[][] token)//表达式:正负号+项+项后缀
        {
            bool f = true;
            if (token[index][1] == keyword.SUB)
            {
                index++;
                if (!Term(token)) { f = false; }
                if (!Termsuff(token)) { f = false; }
            }
            else if (token[index][1] == keyword.整型 || token[index][1] == keyword.实型 || token[index][1] == keyword.ID)
            {
                if (!Term(token)) { f = false; }
                if (!Termsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Exprsuff(string[][] token)//表达式后缀：，+表达式+表达式后缀
        {
            bool f = true;
            if (token[index][1] == keyword.DOUHAO)
            {
                index++;
                if (!Express(token)) { f = false; }
                if (!Exprsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Term(string[][] token)//项：因子+因子后缀
        {
            bool f = true;
            if (!Factor(token)) { f = false; }
            if (!Factsuff(token)) { f = false; }
            return f;
        }
        public static bool Termsuff(string[][] token)//项后缀：加减运算符+因子+因子后缀or空
        {
            bool f = true;
            if (token[index][1] == keyword.ADD || token[index][1] == keyword.SUB)
            {
                index++;
                if (!Factor(token)) { f = false; }
                if (!Factsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Factsuff(string[][] token)//因子后缀：因子+因子后缀or空
        {
            bool f = true;
            if (token[index][1] == keyword.MUL || token[index][1] == keyword.DIV)
            {
                index++;
                if (!Factor(token)) { f = false; }
                if (!Factsuff(token)) { f = false; }
            }
            return f;
        }
        public static bool Factor(string[][] token)//因子：标识符+整数，实数+括号内的表达式
        {
            bool f = true;
            if (token[index][1] == keyword.ID)
            {
                index++;
                if (!Factsuff(token)) { f = false; }
            }
            else if (token[index][1] == keyword.整型 || token[index][1] == keyword.实型)
            {
                index++;
                if (!Factsuff(token)) { f = false; }
            }
        }
    }
}

