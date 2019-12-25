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
        /// Token长度
        /// </summary>
        private static int tokenLength = 0;

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
                list.Add(new string[] { i[2], i[0], attr.ToString() });
            }
            tokenLength = list.Count;
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
                Proghead(token);
                Console.WriteLine("语法分析通过");
                return true;
            }
            catch (ErrorException e)
            {
                Console.WriteLine("语法分析不通过，请处理");
                return false;
            }
        }

        /// <summary>
        /// 后一项
        /// </summary>
        private static void Next()
        {
            if (index < tokenLength - 1)
            {
                index++;
            }
        }

        /// <summary>
        /// 前一项
        /// </summary>
        private static void Before()
        {
            if (index > 0)
            {
                index--;
            }
        }

        /// <summary>
        /// 分析程序头
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Proghead(string[][] token)
        {
            if (token[index][1] == Keywords.PROGRAM)//含有program
            {
                Next();
                if (token[index][1] == Keywords.ID)//是标识符
                {

                    Next();
                    ProBody(token);//执行程序体
                }
                else
                {
                    error += "[\"该程序program缺少方法名\",\"" + index + "\"],";
                    throw new ErrorException("该程序program缺少方法名", index.ToString()); ;
                }
            }
            else
            {
                error += "[\"该程序缺少关键字：program\",\"" + index + "\"],";
                throw new ErrorException("该程序缺少关键字：program", index.ToString()); ;
            }
        }

        /// <summary>
        /// 程序体部分（变量说明，复合句）
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void ProBody(string[][] token)
        {
            if (token[index][1] == Keywords.VAR)//如果是var
            {
                Next();
                Varexpl(token);//执行变量定义
            }
            else if (token[index][1] == Keywords.BEGIN)//如果是begin
            {
                Next();
                Compesent(token);//执行复合句
            }
            else
            {
                error += "[\"程序体缺少var或begin\",\"" + index + "\"],";
                throw new ErrorException("程序体缺少var或begin", index.ToString()); ;
            }
        }

        /// <summary>
        /// 变量说明部分:〈变量定义〉→〈标识符表〉：〈类型〉；｜〈标识符表〉：〈类型〉；〈变量定义〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Varexpl(string[][] token)
        {
            if (IsIdlist(token))//若该字符为标识符，则判断下一个字符，若为冒号，继续判断类型定义是否正确
            {
                Next();
                if (token[index][1] == Keywords.MAOHAO)//冒号
                {
                    Next();
                    if (token[index][1] == Keywords.INTEGER || token[index][1] == Keywords.BOOL || token[index][1] == Keywords.CHAR || token[index][1] == Keywords.STRING || token[index][1] == Keywords.REAL)//类型为
                    {
                        Next();
                        if (token[index][1] == Keywords.SEM)//如果是分号，判断下一个单词，若为begin，执行复合句；否则继续循环执行变量定义
                        {
                            Next();
                            if (token[index][1] == Keywords.BEGIN)//含有begin
                            {
                                Next();
                                Compesent(token);//执行复合句
                            }
                            else
                            {
                                Varexpl(token);//继续执行变量定义
                            }
                        }
                        else
                        {
                            error += "[\"变量定义后面缺少；\",\"" + index + "\"],";
                            throw new ErrorException("变量定义后面缺少；", index.ToString()); ;
                        }
                    }
                    else
                    {
                        error += "[\"变量定义缺少类型或类型定义错误\",\"" + index + "\"],";
                        throw new ErrorException("变量定义缺少类型或类型定义错误", index.ToString()); ;
                        return;
                    }
                }
                else
                {
                    error += "[\"var后面缺少冒号\",\"" + index + "\"],";
                    throw new ErrorException("var后面缺少冒号", index.ToString()); ;
                }
            }
            else
            {
                error += "[\"变量定义标识符出错\",\"" + index + "\"],";
                throw new ErrorException("变量定义标识符出错", index.ToString()); ;
            }
        }

        /// <summary>
        /// 判断是不是标识符表 IsIdlist//〈标识符表〉→〈标识符〉，〈标识符表〉｜〈标识符〉
        /// </summary>
        /// <returns>是不是标识符</returns>
        private static bool IsIdlist(string[][] token)
        {
            if (token[index][1] == Keywords.ID)//标识符
            {//若是标识符，判断下一个字符，如果是逗号，继续判断下一个字符，如果不是逗号，指向前一个字符，返回true，否则返回false——此方法用来判断是否将几个变量定义为同一个类型
                Next();
                if (token[index][1] == Keywords.DOUHAO)//逗号
                {
                    Next();
                    return IsIdlist(token);//下一个字符若为逗号，则继续循环执行判断是否为标识符表
                }
                else
                {//指向前一个字符，为标识符，返回true
                    Before();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 复合语句:begin+语句表+end
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Compesent(string[][] token)
        {
            SentList(token);//执行语句表
            if (token[index][1] == Keywords.SEM && token[index + 1][1] == Keywords.END)//end
            {
                return;
            }
            else
            {
                error += "[\"复合句末尾缺少end\",\"" + index + "\"],";
                throw new ErrorException("复合句末尾缺少end", index.ToString()); ;
            }

        }

        /// <summary>
        /// 语句表SentList//〈语句表〉→〈执行句〉；〈语句表〉｜〈执行句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void SentList(string[][] token)
        {
            ExecSent(token);//执行句
            Next();
            if (token[index][1] == Keywords.SEM)//若为分号，继续循环执行语句表
            {
                Next();
                SentList(token);
            }
            else
            {
                Before();
            }
        }

        /// <summary>
        /// 执行句ExecSent//〈执行句〉→〈简单句〉｜〈结构句〉//〈简单句〉→〈赋值句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void ExecSent(string[][] token)
        {
            if (token[index][1] == Keywords.ID)//标识符如果是标识符，为简单句
            {
                Next();
                AssiSent(token);//赋值句
            }
            else if (token[index][1] == Keywords.BEGIN || token[index][1] == Keywords.IF || token[index][1] == Keywords.WHILE)//begin，if，while的机内码
            {
                StructSent(token);//结构句
            }
            else
            {
                Before();//回退一个
            }
        }

        /// <summary>
        /// 赋值句AssiSent//〈赋值句〉→〈变量〉：＝〈表达式〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void AssiSent(string[][] token)
        {
            if (token[index][1] == Keywords.FUZHI)//:=
            {
                Next();
                Expression(token);//表达式
            }
            else
            {
                error += "[\"赋值句变量后缺少：=\",\"" + index + "\"],";
                throw new ErrorException("赋值句变量后缺少：=", index.ToString()); ;
            }
        }

        /// <summary>
        /// 表达式Expression//〈表达式〉→〈算术表达式〉｜〈布尔表达式〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Expression(string[][] token)
        {
            int symbolIndex = int.Parse(token[index][2]);
            if (token[index][1] == Keywords.FALSE || token[index][1] == Keywords.TRUE || (symbolIndex != -1 && GetSymbol()[symbolIndex][0] == "布尔型"))//false或true或单词为保留字且在符号表中的类型为bool型
            {
                BoolExp(token);//布尔表达式
            }
            else
            {
                AritExp(token);//算术表达式
            }
        }

        /// <summary>
        /// 布尔表达式BoolExp//〈布尔表达式〉→〈布尔表达式〉or〈布尔项〉｜〈布尔项〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolExp(string[][] token)
        {
            BoolItem(token);//布尔项
            Next();
            if (token[index][1] == Keywords.OR)//or
            {
                Next();
                BoolExp(token);
            }
            else
            {
                Before();
            }
        }

        /// <summary>
        /// 布尔项BoolItem//〈布尔项〉→〈布尔项〉and〈布尔因子〉｜〈布尔因子〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolItem(string[][] token)
        {
            BoolFactor(token);//布尔因子
            Next();
            if (token[index][1] == Keywords.END)//and
            {
                Next();
                BoolItem(token);//布尔项
            }
            else
            {
                Before();
            }

        }

        /// <summary>
        /// 布尔因子BoolFactor//〈布尔因子〉→ not〈布尔因子〉｜〈布尔量〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolFactor(string[][] token)
        {
            if (token[index][1] == Keywords.NOT)//not
            {
                Next();
                BoolFactor(token);//布尔因子
            }
            else
            {
                BoolValue(token);//布尔量
            }
        }

        /// <summary>
        /// 布尔量BoolValue//〈布尔量〉→〈布尔常数〉｜〈标识符〉｜（〈布尔表达式〉）｜〈关系表达式〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolValue(string[][] token)
        {
            if (token[index][1] == Keywords.TRUE || token[index][1] == Keywords.FALSE)//true或false
            {
                return;
            }
            else if (token[index][1] == Keywords.ID)//标识符（关系表达式）
            {
                Next();
                if (int.Parse(token[index][1]) >= 33 && int.Parse(token[index][1]) <= 38)
                {//==或>或<或<>或<=或>=
                    Next();
                    if (token[index][1] == Keywords.ID)//标识符
                    {
                    }
                    else
                    {
                        error += "[\"关系运算符后缺少标识符\",\"" + index + "\"],";
                        throw new ErrorException("关系运算符后缺少标识符", index.ToString()); ;
                    }
                }
                else
                {
                    Before();
                }
            }
            else if (token[index][1] == Keywords.LKUOHAO)//字符为（，即布尔表达式
            {
                BoolExp(token);//执行布尔表达式
                if (token[index][1] == Keywords.RKUOHAO)//字符为）
                {
                    return;
                }
                else
                {
                    error += "[\"布尔量中的布尔表达式缺少一个）\",\"" + index + "\"],";
                    throw new ErrorException("布尔量中的布尔表达式缺少一个）", index.ToString()); ;
                }
            }
            else
            {
                error += "[\"布尔量出错\",\"" + index + "\"],";
                throw new ErrorException("布尔量出错", index.ToString()); ;
            }
        }

        /// <summary>
        /// 算术表达式 AritExp//〈算术表达式〉→〈算术表达式〉＋〈项〉｜〈算术表达式〉－〈项〉｜〈项〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void AritExp(string[][] token)
        {
            Item(token);//执行项

            Next();
            if (token[index][1] == Keywords.ADD || token[index][1] == Keywords.SUB)//符号为+或-
            {
                Next();
                AritExp(token);//执行算术表达式
            }
            else
            {
                Before();
                return;
            }

        }

        /// <summary>
        /// 项 Item//〈项〉→〈项〉＊〈因子〉｜〈项〉／〈因子〉｜〈因子〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Item(string[][] token)
        {
            Factor(token);//执行因子
            Next();
            if (token[index][1] == Keywords.MUL || token[index][1] == Keywords.DIV)//符号为*或/
            {
                Next();
                Item(token);//执行项
            }
            else
            {
                Before();
                return;
            }
        }

        /// <summary>
        /// 因子Factor//〈因子〉→〈算术量〉｜（〈算术表达式〉）
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Factor(string[][] token)
        {
            if (token[index][1] == Keywords.LKUOHAO)//字符为（
            {
                Next();
                AritExp(token);//执行算术表达式
                Next();
                if (token[index][1] == Keywords.RKUOHAO)//字符为)
                {
                    return;
                }
                else
                {
                    error += "[\"因子中算数表达式缺少）\",\"" + index + "\"],";
                    throw new ErrorException("因子中算数表达式缺少）", index.ToString()); ;
                }
            }
            else
            {
                CalQua(token);//执行算术量
            }
        }

        /// <summary>
        /// 算术量CalQua//〈算术量〉→〈标识符〉｜〈整数〉｜〈实数〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void CalQua(string[][] token)
        {
            if (token[index][1] == Keywords.ID || token[index][1] == Keywords.整型 || token[index][1] == Keywords.实型)//标识符或整数或实数
            {
                return;
            }
            else
            {
                error += "[\"算术量出错\",\"" + index + "\"],";
                throw new ErrorException("算术量出错", index.ToString()); ;
            }
        }

        /// <summary>
        /// 结构句 StructSent//〈结构句〉→〈复合句〉｜〈if句〉｜〈WHILE句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void StructSent(string[][] token)
        {
            if (token[index][1] == Keywords.BEGIN)//begin
            {
                Next();
                Compesent(token);//执行复合句
            }
            else if (token[index][1] == Keywords.IF)//if
            {
                Next();
                IfSent(token);//执行if语句
            }
            else if (token[index][1] == Keywords.WHILE)//while
            {
                Next();
                WhileSent(token);//执行while语句
            }
        }

        /// <summary>
        /// if语句IfSent// 〈if句〉→if〈布尔表达式〉then〈执行句〉| if〈布尔表达式〉then〈执行句〉else〈执行句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void IfSent(string[][] token)
        {
            BoolExp(token);//布尔表达式
            Next();
            if (token[index][1] == Keywords.THEN)//then
            {
                Next();
                ExecSent(token);//执行句
                Next();
                if (token[index][1] == Keywords.ELSE)//else
                {
                    Next();
                    ExecSent(token);//执行句
                }
                else
                {
                    Before();
                    return;
                }
            }
            else
            {
                error += "[\"if...then语句缺少then\",\"" + index + "\"],";
                throw new ErrorException("if...then语句缺少then", index.ToString()); ;
            }
        }

        /// <summary>
        /// while语句 WhileSent//〈while句〉→while〈布尔表达式〉do〈执行句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void WhileSent(string[][] token)
        {
            BoolExp(token);//布尔表达式
            Next();
            if (token[index][1] == Keywords.DO)//do
            {
                Next();
                ExecSent(token);//执行句
            }
            else
            {
                error += "[\"while语句缺少do\",\"" + index + "\"],";
                throw new ErrorException("while语句缺少do", index.ToString()); ;
            }
        }
    }
}

