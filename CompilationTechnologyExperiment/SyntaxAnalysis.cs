using System;
using System.Collections.Generic;
using System.Linq;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 语法分析器类
    /// </summary>
    public static class SyntaxAnalysis
    {
        #region 变量声明
        /// <summary>
        /// 四元式列表
        /// </summary>
        private static List<QuaternaryFormula> formulas = new List<QuaternaryFormula>();

        /// <summary>
        /// 符号表
        /// </summary>
        private static List<Symbol> symbols = (List<Symbol>)Tools.GetJsonObject(Tools.GetFileContent("Symbol.txt"), "Symbol");

        /// <summary>
        /// 错误信息
        /// </summary>
        private static string error = "[";

        /// <summary>
        /// Token长度
        /// </summary>
        private static int tokenLength = 0;

        /// <summary>
        /// 当前Token的索引
        /// </summary>
        private static int index = 0;

        /// <summary>
        /// 临时变量索引
        /// </summary>
        private static int temporaryVariablesIndex = 1;

        /// <summary>
        /// 临时变量
        /// </summary>
        private static string temporaryVariables = "";
        #endregion

        /// <summary>
        /// 获取四元式文件字符串（JSON格式）
        /// </summary>
        /// <param name="formulas">四元式表</param>
        /// <returns>符号表文件字符串</returns>
        public static string GetQuaternaryFormulaFile(List<QuaternaryFormula> formulas)
        {
            try
            {
                if (formulas == null)
                {
                    return "";
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(formulas);
            }
            catch (ErrorException e)
            {
                throw;
            }
        }

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
        /// 创建新的临时变量
        /// </summary>
        /// <returns></returns>
        private static string NewTempVariable()
        {
            string temp = "T" + temporaryVariablesIndex.ToString();
            temporaryVariablesIndex++;
            Symbol symbol = new Symbol();
            symbol.Name = temp;
            symbols.Add(symbol);
            return temp;
        }

        /// <summary>
        /// 回填函数，完成四元式转移目标的回填
        /// </summary>
        /// <param name="addr">链首</param>
        /// <param name="addr2">地址</param>
        private static void BackPatch(int addr, int addr2)
        {
            formulas[addr].result = addr2.ToString();//把链首addr所链接的每个四元式的第四分量都改写为地址addr2
        }

        /// <summary>
        /// 产生四元式
        /// </summary>
        /// <param name="op">操作符</param>
        /// <param name="arg1">操作数1</param>
        /// <param name="arg2">操作数2</param>
        /// <param name="result">结果</param>
        private static void Emit(string op, string arg1, string arg2, string result)
        {
            QuaternaryFormula formula = new QuaternaryFormula(op, arg1, arg2, result);
            formulas.Add(formula);//将新生成的四元式表项formula添加到四元式列表formulas中
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
        /// 语法分析
        /// </summary>
        /// <returns>分析结果</returns>
        public static bool AnalysisResult()
        {
            List<Token> token = (List<Token>)Tools.GetJsonObject(Tools.GetFileContent("Token.txt"), "Token");//token[0]="real" token[1]=42 token[2]=attr
            tokenLength = token.Count;
            try
            {
                Proghead(token);
                Console.WriteLine("语法分析通过");
                System.IO.File.WriteAllText("Formula.txt", SyntaxAnalysis.GetQuaternaryFormulaFile(formulas));
                Console.WriteLine("四元式文件已生成");
                return true;
            }
            catch (ErrorException e)
            {
                Console.WriteLine("语法分析不通过，请处理");
                return false;
            }
        }

        /// <summary>
        /// 分析程序头
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void Proghead(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.PROGRAM)//含有program
            {
                Next();
                if (token[index].Code.ToString() == Keywords.ID)//是标识符
                {
                    Emit("program", token[index].Name, "_", "_");
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
        private static void ProBody(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.VAR)//如果是var
            {
                Next();
                Varexpl(token);//执行变量定义
            }
            else if (token[index].Code.ToString() == Keywords.BEGIN)//如果是begin
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
        private static void Varexpl(List<Token> token)
        {
            if (IsIdlist(token))//若该字符为标识符，则判断下一个字符，若为冒号，继续判断类型定义是否正确
            {
                Next();
                if (token[index].Code.ToString() == Keywords.MAOHAO)//冒号
                {
                    Next();
                    if (token[index].Code.ToString() == Keywords.INTEGER || token[index].Code.ToString() == Keywords.BOOL || token[index].Code.ToString() == Keywords.CHAR || token[index].Code.ToString() == Keywords.STRING || token[index].Code.ToString() == Keywords.REAL)//类型为
                    {
                        Next();
                        if (token[index].Code.ToString() == Keywords.SEM)//如果是分号，判断下一个单词，若为begin，执行复合句；否则继续循环执行变量定义
                        {
                            Next();
                            if (token[index].Code.ToString() == Keywords.BEGIN)//含有begin
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
                        throw new ErrorException("变量定义缺少类型或类型定义错误", index.ToString());
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
        private static bool IsIdlist(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.ID)//标识符
            {//若是标识符，判断下一个字符，如果是逗号，继续判断下一个字符，如果不是逗号，指向前一个字符，返回true，否则返回false——此方法用来判断是否将几个变量定义为同一个类型
                Next();
                if (token[index].Code.ToString() == Keywords.DOUHAO)//逗号
                {
                    Next();
                    return IsIdlist(token);//下一个字符若为逗号，则继续循环执行判断是否为标识符表
                }
                else
                {
                    //指向前一个字符，为标识符，返回true
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
        private static void Compesent(List<Token> token)
        {
            SentList(token);//执行语句表
            if (token[index].Code.ToString() == Keywords.SEM && token[index + 1].Code.ToString() == Keywords.END)//end
            {
                Emit("sys", "_", "_", "_");//生成四元式
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
        private static void SentList(List<Token> token)
        {
            S s = new S();//新建一个产生式符号
            ExecSent(token, ref s);//执行句
            Next();
            if (token[index].Code.ToString() == Keywords.SEM)//若为分号，继续循环执行语句表
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
        /// <param name="s">产生式</param>
        private static void ExecSent(List<Token> token, ref S s)
        {
            if (token[index].Code.ToString() == Keywords.ID)//标识符如果是标识符，为简单句
            {
                Next();
                AssiSent(token);//赋值句
            }
            else if (token[index].Code.ToString() == Keywords.BEGIN || token[index].Code.ToString() == Keywords.IF || token[index].Code.ToString() == Keywords.WHILE)//begin，if，while的机内码
            {
                StructSent(token, ref s);//结构句
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
        private static void AssiSent(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.FUZHI)//:=
            {
                string temp = token[index - 1].Name;//temp记录上一个token文件项的名字
                Next();
                Expression(token);//表达式
                Emit(":=", temporaryVariables, "_", temp);//生成四元式，即temp：=temporaryVariables
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
        private static void Expression(List<Token> token)
        {
            int symbolIndex = token[index].Addr;
            if (token[index].Code.ToString() == Keywords.FALSE || token[index].Code.ToString() == Keywords.TRUE || (symbolIndex != -1 && symbols[symbolIndex].Type.ToString() == Keywords.布尔型))//false或true或单词为保留字且在符号表中的类型为bool型
            {
                E e = new E();//新建一个表达式
                BoolExp(token, ref e);//布尔表达式
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
        private static void BoolExp(List<Token> token, ref E e)
        {
            E e1 = new E();
            BoolItem(token, ref e1);//布尔项
            try
            {
                Next();
                if (token[index].Code.ToString() == Keywords.OR)//or
                {
                    int m = formulas.Count;//m记录四元式表项的数量值，即地址M.quad
                    E e2 = new E();
                    Next();
                    BoolExp(token, ref e2);//执行布尔表达式
                    e.True.Concat(e1.True);//Concat连接两个序列e1.True和e2.True,即e.True={e1.True,e2.True}
                    e.True.Concat(e2.True);
                    e.False = e2.False;//即e.False={e2.False}
                    foreach (int k in e.False)//foreach是一个迭代器，从int型数组k中循环读取数据，并将每次循环到的值赋值给e.False
                    {
                        BackPatch(k, m);//执行回填函数,把链首k所链接的每个四元式的第四分量都改写为地址m;即k=e1.False,m=M.q
                    }
                }
                else
                {
                    e = e1;
                    Before();
                }
            }
            catch (ErrorException ex)
            {
                e = e1;
                throw ex;
            }
        }

        /// <summary>
        /// 〈布尔项〉→〈布尔项〉and〈布尔因子〉｜〈布尔因子〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolItem(List<Token> token, ref E e)
        {
            E e1 = new E();
            BoolFactor(token, ref e1);//布尔因子
            Next();
            if (token[index].Code.ToString() == Keywords.AND)//and
            {
                Next();
                int m = formulas.Count;
                E e2 = new E();
                BoolItem(token, ref e2);//布尔项
                e.True = e2.True; //即e.True=e2.True
                e.False.Concat(e1.False);//即e.False={e1.False,e2.False}
                e.False.Concat(e2.False);
                foreach (int k in e.True)
                {
                    BackPatch(k, m);//执行回填函数
                }
            }
            else
            {
                e = e1;
                Before();
            }

        }

        /// <summary>
        /// 布尔因子BoolFactor//〈布尔因子〉→ not〈布尔因子〉｜〈布尔量〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolFactor(List<Token> token, ref E e)
        {
            if (token[index].Code.ToString() == Keywords.NOT)//not
            {
                Next();
                E e1 = new E();
                BoolFactor(token, ref e1);//布尔因子
                e.True = e1.False;//即e.True=e1.True;e.False=e1.False
                e.False = e1.True;
            }
            else
            {
                E e1 = new E();
                BoolValue(token, ref e1);//布尔量
                e = e1;
            }
        }

        /// <summary>
        /// 布尔量BoolValue//〈布尔量〉→〈布尔常数〉｜〈标识符〉｜（〈布尔表达式〉）｜〈关系表达式〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void BoolValue(List<Token> token, ref E e)
        {
            if (token[index].Code.ToString() == Keywords.TRUE || token[index].Code.ToString() == Keywords.FALSE)//true或false
            {
                e.True.Add(formulas.Count);
                e.False.Add(formulas.Count + 1);
                temporaryVariables = token[index].Name;//tt记录名字
            }
            else if (token[index].Code.ToString() == Keywords.ID)//标识符（关系表达式）
            {
                Next();
                if (int.Parse(token[index].Code.ToString()) >= 33 && int.Parse(token[index].Code.ToString()) <= 38)
                {//==或>或<或<>或<=或>=
                    Next();
                    if (token[index].Code.ToString() == Keywords.ID)//标识符
                    {
                        e.True.Add(formulas.Count);
                        e.False.Add(formulas.Count + 1);
                        Emit("j" + token[index - 1].Name, token[index - 2].Name, token[index].Name, "0");//生成四元式，即a<b的四元式为(j<,a,b,0)
                        Emit("j", "_", "_", "0");
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
                    e.True.Add(formulas.Count);
                    e.False.Add(formulas.Count + 1);
                    Emit("jnz", token[index].Name, "_", "0");//生成四元式,即E—>a的四元式为(jnz,a,_,0)
                    Emit("j", "_", "_", "0");
                    Next();
                }
            }
            else if (token[index].Code.ToString() == Keywords.LKUOHAO)//字符为（，即布尔表达式
            {
                E e1 = new E();//定义E—>(E1)
                BoolExp(token, ref e1);//执行布尔表达式
                e.True = e1.True;
                e.False = e1.False;
                if (token[index].Code.ToString() == Keywords.RKUOHAO)//字符为）
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
        private static void AritExp(List<Token> token)
        {
            Item(token);//执行项
            Next();
            if (token[index].Code.ToString() == Keywords.ADD || token[index].Code.ToString() == Keywords.SUB)//符号为+或-
            {
                string[] temp = { token[index - 1].Name, token[index].Name };//temp记录运算符和它前面的变量名字
                if (token[index - 1].Code == 22)//符号为）
                {
                    temp[0] = temporaryVariables;
                }
                Next();
                AritExp(token);//执行算术表达式
                Emit(temp[1], temp[0], temporaryVariables, NewTempVariable());//生成四元式，即x:=y+z的四元式为(+,y,z,T1)
                temporaryVariables = "T" + (temporaryVariablesIndex - 1).ToString();
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
        private static void Item(List<Token> token)
        {
            Factor(token);//执行因子
            Next();
            if (token[index].Code.ToString() == Keywords.MUL || token[index].Code.ToString() == Keywords.DIV)//符号为*或/
            {
                string[] temp = { token[index - 1].Name, token[index].Name };//temp记录运算符和它前面的变量名字
                if (token[index - 1].Code == 22)//符号为）
                {
                    temp[0] = temporaryVariables;
                }
                Next();
                Item(token);//执行算术表达式
                Emit(temp[1], temp[0], temporaryVariables, NewTempVariable());//生成四元式，即x:=y*z的四元式为(*,y,z,T1)
                temporaryVariables = "T" + (temporaryVariablesIndex - 1).ToString();

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
        private static void Factor(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.LKUOHAO)//字符为（
            {
                Next();
                AritExp(token);//执行算术表达式
                Next();
                if (token[index].Code.ToString() == Keywords.RKUOHAO)//字符为)
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
        private static void CalQua(List<Token> token)
        {
            if (token[index].Code.ToString() == Keywords.ID || token[index].Code.ToString() == Keywords.整型 || token[index].Code.ToString() == Keywords.实型)//标识符或整数或实数
            {
                temporaryVariables = token[index].Name;//记录变量名
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
        private static void StructSent(List<Token> token, ref S s)
        {
            if (token[index].Code.ToString() == Keywords.BEGIN)//begin
            {
                Next();
                Compesent(token);//执行复合句
            }
            else if (token[index].Code.ToString() == Keywords.IF)//if
            {
                Next();
                IfSent(token, ref s);//执行if语句
            }
            else if (token[index].Code.ToString() == Keywords.WHILE)//while
            {
                Next();
                WhileSent(token, ref s);//执行while语句
            }
        }

        /// <summary>
        /// if语句IfSent// 〈if句〉→if〈布尔表达式〉then〈执行句〉| if〈布尔表达式〉then〈执行句〉else〈执行句〉
        /// </summary>
        /// <param name="token">Token数组</param>
        private static void IfSent(List<Token> token, ref S s)
        {
            E e = new E();
            BoolExp(token, ref e);//布尔表达式
            Next();
            if (token[index].Code.ToString() == Keywords.THEN)//then
            {
                int m1 = formulas.Count;
                S s1 = new S();
                Next();
                ExecSent(token, ref s1);//执行句
                Next();
                if (token[index].Code.ToString() == Keywords.ELSE)//else
                {
                    S n = new S();//若N—>ε,n.next=fps.Count,并生成四元式(j,_,_,0)
                    n.next.Add(formulas.Count);
                    Emit("j", "_", "_", "0");
                    S s2 = new S();
                    int m2 = formulas.Count;
                    Next();
                    ExecSent(token, ref s2);//执行句
                    s.next = s1.next;
                    s.next.Concat(n.next);
                    s.next.Concat(s2.next);
                    foreach (int k in e.True)
                    {
                        BackPatch(k, m1);//执行回填函数
                    }
                    foreach (int k in e.False)
                    {
                        BackPatch(k, m2);
                    }
                }
                else
                {
                    s.next = e.False;
                    s.next.Concat(s1.next);
                    foreach (int k in e.True)
                    {
                        BackPatch(k, m1);
                    }
                    Before();
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
        private static void WhileSent(List<Token> token, ref S s)
        {
            int m1 = formulas.Count;
            E e = new E();
            BoolExp(token, ref e);//布尔表达式
            Next();
            if (token[index].Code.ToString() == Keywords.DO)//do即控制语句产生式为S—>while M1E do M2S1,则s.next=e.False,生成四元式(j,_,_,m1.q),并回填e.True和s1.next
            {
                int m2 = formulas.Count;
                S s1 = new S();
                Next();
                ExecSent(token, ref s1);//执行句
                s.next = e.False;
                Emit("j", "_", "_", m1.ToString());//生成四元式
                foreach (int k in e.True)
                {
                    BackPatch(k, m2);
                }
                foreach (int k in s1.next)
                {
                    BackPatch(k, m1);
                }
            }
            else
            {
                error += "[\"while语句缺少do\",\"" + index + "\"],";
                throw new ErrorException("while语句缺少do", index.ToString()); ;
            }
        }
    }
}

