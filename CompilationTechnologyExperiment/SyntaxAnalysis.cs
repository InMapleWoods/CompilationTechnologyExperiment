using System;
using System.Collections.Generic;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 语法分析器类
    /// </summary>
    public static class SyntaxAnalysis
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        private static string error = "[";

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>错误信息</returns>
        public static string GetErrorMessage()
        {
            error = error.Substring(0, error.Length - 1);
            if (!string.IsNullOrEmpty(error))
                error += "]";
            return error;
        }

        /// <summary>
        /// 获取符号表
        /// </summary>
        /// <returns>符号表数组</returns>
        private static string[][] GetSymbol()
        {
            string[][] Symbols = Tools.GetJsonObject(Tools.GetFileContent("Symbol.txt"));
            return Symbols;
        }

        /// <summary>
        /// 获取Token表
        /// </summary>
        /// <returns>Token数组</returns>
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

        /// <summary>
        /// 获取Token在符号表中的位置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 当前Token的索引
        /// </summary>
        private static int index = 0;

        /// <summary>
        /// 语法分析
        /// </summary>
        /// <returns>分析结果</returns>
        public static bool AnalysisResult()
        {
            string[][] token = GetToken();//token[0]="real" token[1]=42 token[2]=attr
            try
            {
                Program(token);
                Console.WriteLine("YES");
                return true;
            }
            catch
            {
                Console.WriteLine("No");
                return false;
            }
        }
        
        /// <summary>
        /// 分析程序体
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Program(string[][] token)
        {
            Proghead(token);
            Block(token);
            if (token[index][1] == Keywords.SEM)//判断“；”
            {
                index++;
            }
            else
            {
                error += "[\"miss a ';'\",\"" + index + "\"],";
                throw new ErrorException("miss a ';'", index.ToString());
            }
        }

        /// <summary>
        /// 分析程序头
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Proghead(string[][] token)
        {
            if (token[index][1] == Keywords.PROGRAM)//"program"
            {
                index++;
                if (token[index][1] == Keywords.ID)
                {
                    index++;
                    if (token[index][1] == Keywords.SEM)
                    {
                        index++;
                    }
                    else
                    {
                        error += "[\"miss a ';'\",\"" + index + "\"],";
                        throw new ErrorException("miss a ';'", index.ToString());
                    }
                }
                else
                {
                    error += "[\"error of ID\",\"" + index + "\"],";
                    throw new ErrorException("error of ID", index.ToString());
                }
            }
            else
            {
                error += "[\"missing Keywords.program\",\"" + index + "\"],";
                throw new ErrorException("missing Keywords.program", index.ToString());
            }
        }

        /// <summary>
        /// 程序体部分，常量说明，变量说明，语句部分（变量说明，复合句）
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Block(string[][] token)
        {
            Consexpl(token);//常量说明
            Varexpl(token);//变量说明
            Compesent(token);//复合语句
        }

        /// <summary>
        /// 常量说明部分：整数or实数
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Consexpl(string[][] token)
        {
            if (token[index][1] == Keywords.整型 || token[index][1] == Keywords.实型)
            {
                index++;
                Consdefi(token);
                if (token[index][1] == Keywords.SEM)
                {
                    index++;
                }
                else
                {
                    error += "[\"miss a ';'\",\"" + index + "\"],";
                    throw new ErrorException("miss a ';'", index.ToString());
                }
            }
        }

        /// <summary>
        /// 常量定义：标识符+等号+无符号数
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Consdefi(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                index++;
                if (token[index][1] == Keywords.EQU)
                {
                    index++;
                    if (token[index][1] == Keywords.INTEGER)
                    {
                        index++;
                    }
                    else
                    {
                        error += "[\"miss INREGER\",\"" + index + "\"],";
                        throw new ErrorException("miss INREGER", index.ToString());
                    }
                }
                else
                {
                    error += "[\"miss a '='\",\"" + index + "\"],";
                    throw new ErrorException("miss a '='", index.ToString());
                }
            }
            else
            {
                error += "[\"miss ID\",\"" + index + "\"],";
                throw new ErrorException("miss ID", index.ToString());
            }
        }

        /// <summary>
        /// 常量定义后缀：,+常量定义后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Conssuff(string[][] token)
        {
            if (token[index][1] == Keywords.DOUHAO)
            {
                index++;
                Consdefi(token);
                Conssuff(token);
                if (token[index][1] == Keywords.SEM)
                {
                    index++;
                }
                else
                {
                    error += "[\"miss a ';'\",\"" + index + "\"],";
                    throw new ErrorException("miss a ';'", index.ToString());
                }
            }
        }

        /// <summary>
        /// 变量说明部分：var+变量定义+变量定义后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Varexpl(string[][] token)
        {
            if (token[index][1] == Keywords.VAR)
            {
                index++;
                Vardefi(token);
                Varsuff(token);
            }
        }

        /// <summary>
        /// 变量定义：标识符+标识符后缀+：类型+；||标识符+标识符后缀+：+类型+；+变量定义
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Vardefi(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                index++;
                Idsuff(token);
                if (token[index][1] == Keywords.MAOHAO)
                {
                    index++; Typeil(token);
                    if (token[index][1] == Keywords.SEM)
                    {
                        index++;
                    }
                    else
                    {
                        error += "[\"miss a ';'\",\"" + index + "\"],";
                        throw new ErrorException("miss a ';'", index.ToString());
                    }
                }
                else
                {
                    error += "[\"miss a ':'\",\"" + index + "\"],";
                    throw new ErrorException("miss a ':'", index.ToString());
                }
            }
        }

        /// <summary>
        /// 变量后缀：变量定义+变量后缀||空
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Varsuff(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                Vardefi(token);
                Varsuff(token);
            }
        }

        /// <summary>
        /// 类型
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Typeil(string[][] token)
        {
            if (token[index][1] == Keywords.REAL || token[index][1] == Keywords.BOOL || token[index][1] == Keywords.INTEGER)
            {
                index++;
            }
            else
            {
                error += "[\"wrong type of define\",\"" + index + "\"],";
                throw new ErrorException("wrong type of define", index.ToString());
            }
        }

        /// <summary>
        /// 复合语句:begin+语句表+end
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Compesent(string[][] token)
        {
            if (token[index][1] == Keywords.BEGIN)
            {
                index++;
                Sentence(token); Sentsuff(token);
                if (token[index][1] == Keywords.END)
                {
                    index++;
                }
            }
            else
            {
                error += "[\"miss Begin as the start\",\"" + index + "\"],";
                throw new ErrorException("miss Begin as the start", index.ToString());
            }
        }

        /// <summary>
        /// 赋值语句，if语句，while语句，复合句
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Sentence(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                Assipro(token);
            }
            else if (token[index][1] == Keywords.IF)
            {
                Ifsent(token);
            }
            else if (token[index][1] == Keywords.WHILE)
            {
                Whilesent(token);
            }
            else if (token[index][1] == Keywords.BEGIN)
            {
                Compesent(token);
            }
        }

        /// <summary>
        /// 语句后缀：；+语句+语句后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Sentsuff(string[][] token)
        {
            if (token[index][1] == Keywords.SEM)
            {
                index++;
                Sentence(token);
                Sentsuff(token);
            }
        }

        /// <summary>
        /// 赋值语句
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Assipro(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                index++;
                Suffix(token);
            }
            else
            {
                error += "[\"赋值语句错误\",\"" + index + "\"],";
                throw new ErrorException("赋值语句错误", index.ToString());
            }
        }

        /// <summary>
        /// 赋值号的后缀为赋值语句，否则为过程调用语句
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Suffix(string[][] token)
        {
            if (token[index][1] == Keywords.FUZHI)
            {
                index++;
                Express(token);
            }
            else if (token[index][1] == Keywords.LKUOHAO)
            {
                index++;
                Express(token);
                if (token[index][1] == Keywords.RKUOHAO)
                {
                    index++;
                }
                else
                {
                    error += "[\"缺少右括号\",\"" + index + "\"],";
                    throw new ErrorException("缺少右括号", index.ToString());
                }
            }
        }

        /// <summary>
        /// 表达式:正负号+项+项后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Express(string[][] token)
        {
            if (token[index][1] == Keywords.SUB)
            {
                index++;
                Term(token);
                Termsuff(token);
            }
            else if (token[index][1] == Keywords.整型 || token[index][1] == Keywords.实型 || token[index][1] == Keywords.ID)
            {
                Term(token);
                Termsuff(token);
            }
        }

        /// <summary>
        /// 表达式后缀：，+表达式+表达式后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Exprsuff(string[][] token)
        {
            if (token[index][1] == Keywords.DOUHAO)
            {
                index++;
                Express(token);
                Exprsuff(token);
            }
        }

        /// <summary>
        /// 项：因子+因子后缀
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Term(string[][] token)
        {
            Factor(token);
            Factsuff(token);
        }

        /// <summary>
        /// 项后缀：加减运算符+因子+因子后缀or空
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Termsuff(string[][] token)
        {
            if (token[index][1] == Keywords.ADD || token[index][1] == Keywords.SUB)
            {
                index++;
                Factor(token);
                Factsuff(token);
            }
        }

        /// <summary>
        /// 因子后缀：因子+因子后缀or空
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Factsuff(string[][] token)
        {
            if (token[index][1] == Keywords.MUL || token[index][1] == Keywords.DIV)
            {
                index++;
                Factor(token);
                Factsuff(token);
            }
        }

        /// <summary>
        /// 因子：标识符+整数，实数+括号内的表达式
        /// </summary>
        /// <param name="token">Token数组</param>
        public static void Factor(string[][] token)
        {
            if (token[index][1] == Keywords.ID)
            {
                index++;
                Factsuff(token);
            }
            else if (token[index][1] == Keywords.整型 || token[index][1] == Keywords.实型)
            {
                index++;
                Factsuff(token);
            }
        }
    }
}

