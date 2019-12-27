using System.Collections.Generic;
using System.Linq;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 目标代码生成
    /// </summary>
    public class GenerateAssemblyCode
    {
        #region 变量定义
        /// <summary>
        /// 目标代码生成语句索引
        /// </summary>
        private int lineIndex = 0;

        /// <summary>
        /// 错误
        /// </summary>
        public string error = "";

        /// <summary>
        /// 四元式数组
        /// </summary>
        private List<QuaternaryFormula> formulas = new List<QuaternaryFormula>();

        /// <summary>
        /// 附加在四元式上的待用和活跃信息
        /// </summary>
        private List<KeyValuePair<int, string[]>> formulasAppendInformation = new List<KeyValuePair<int, string[]>>();

        /// <summary>
        /// 符号表
        /// </summary>
        private List<Symbol> symbols = new List<Symbol>();

        /// <summary>
        /// 汇编代码表
        /// </summary>
        private List<Assembly> assemblys = new List<Assembly>();

        /// <summary>
        /// BX寄存器存储的变量
        /// </summary>
        private List<string> bx = new List<string>();

        /// <summary>
        /// DX寄存器存储的变量
        /// </summary>
        private List<string> dx = new List<string>();
        #endregion
        /// <summary>
        /// 获取汇编代码表文件字符串（JSON格式）
        /// </summary>
        /// <returns>汇编代码表文件字符串</returns>
        public string GetAssembly()
        {
            try
            {
                if (assemblys == null)
                {
                    return "";
                }
                string str = "";
                foreach (Assembly a in assemblys)
                {
                    if (a.Num != null)
                    {
                        str += "L" + a.Num + ":\t";
                        if (a.PR.Length == 0)
                        {
                            str += a.Op + " " + a.PL + "\r\n";
                        }
                        else
                        {
                            str += a.Op + " " + a.PL + "," + a.PR + "\r\n";
                        }
                    }
                    else
                    {
                        str += "\t";
                        if (a.PR.Length == 0)
                        {
                            str += a.Op + " " + a.PL + "\r\n";
                        }
                        else
                        {
                            str += a.Op + " " + a.PL + "," + a.PR + "\r\n";
                        }
                    }
                }
                return str;
            }
            catch (ErrorException e)
            {
                throw;
            }
        }
        /// <summary>
        /// 目标代码生成函数
        /// </summary>
        public bool GenerateCode(List<QuaternaryFormula> formulas, List<Symbol> symbols, List<int[]> basicBlock)
        {
            try
            {
                lineIndex = 0;
                assemblys.Clear();
                this.formulas = formulas;
                this.symbols = symbols;
                //遍历每一个基本块
                foreach (var block in basicBlock)
                {
                    WaitClear();
                    var f = (from i in formulas
                             where formulas.IndexOf(i) >= block[0] && formulas.IndexOf(i) <= block[1]
                             select i).ToList();

                    List<KeyValuePair<int, string[]>> formulasAppendInformation = new List<KeyValuePair<int, string[]>>();
                    CalWaitOrActiveArray(symbols, f, formulasAppendInformation);
                    this.formulasAppendInformation = formulasAppendInformation;
                    Translate(f);
                }
                System.IO.File.WriteAllText("AssemblyCode.txt", GetAssembly());
                return true;
            }
            catch (ErrorException e)
            {
                error = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 清除WAIT数组
        /// </summary>
        private void WaitClear()
        {
            foreach (Symbol s in symbols)
            {
                s.Wait = null;
                s.Wait = new List<string[]> { new string[] { "0", "N" } };
            }
        }

        /// <summary>
        /// 寻找参数所占用的寄存器
        /// </summary>
        /// <param name="str">参数名（变量名）</param>
        /// <returns>所占用的寄存器</returns>
        private string GetValue(string str)
        {
            foreach (Symbol s in symbols)
            {
                if (s.Name == str)
                {
                    return s.Value;
                }
            }
            return "";
        }

        /// <summary>
        /// 获取寄存器对应的名称
        /// </summary>
        /// <param name="reg">寄存器</param>
        /// <returns>结果</returns>
        private string GetNameByReg(List<string> reg)
        {
            if (reg == bx)
            {
                return "bx";
            }
            else if (reg == dx)
            {
                return "dx";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取名称对应的寄存器RValue
        /// </summary>
        /// <param name="name">符号名</param>
        /// <returns>结果</returns>
        private List<string> GetRegByName(string name)
        {
            if (name == "bx")
            {
                return bx;
            }
            else if (name == "dx")
            {
                return dx;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取Symbol
        /// </summary>
        /// <param name="name">符号名</param>
        /// <returns>获取Symbol结果</returns>
        private List<Symbol> GetSymbol(string name)
        {
            List<Symbol> temp = new List<Symbol>();
            foreach (Symbol symbol in symbols)
            {
                if (symbol.Name == name)
                {
                    temp.Add(symbol);
                }
            }
            return temp;
        }

        /// <summary>
        /// 寄存器分配
        /// </summary>
        /// <param name="formula">四元式</param>
        /// <returns>分配寄存器结果</returns>
        private string GetReg(QuaternaryFormula formula)
        {
            int index = formulas.IndexOf(formula);
            List<Symbol> symbols = GetSymbol(formula.arg1);
            foreach (var symbol in symbols)
            {
                foreach (var reg in new List<string>[] { bx, dx })
                {
                    if ((reg == GetRegByName(symbol.Value) && reg.Contains(symbol.Name) && reg.Count == 1) || (formula.result == symbol.Name) || (WaitOrActiveArray(symbol, index)))
                    {
                        return GetNameByReg(reg);
                    }
                    else if (reg.Count == 0)
                    {
                        return GetNameByReg(reg);
                    }
                    else
                    {
                        foreach (string i in reg)
                        {
                            foreach (Symbol s in GetSymbol(i))
                            {
                                if (s.Name != formula.result && s.Value != s.Name)
                                {
                                    Assembly assembly = new Assembly("MOV", s.Name, GetNameByReg(reg));
                                    assemblys.Add(assembly);
                                    if (s.Name != symbol.Name)
                                    {
                                        s.Value = s.Name;
                                    }
                                    else
                                    {
                                        s.Value = GetNameByReg(reg);
                                    }
                                    reg.Remove(s.Name);
                                }
                            }
                        }
                        return GetNameByReg(reg);
                    }
                }
            }
            return "errorReg";
        }

        /// <summary>
        /// 待用活跃
        /// </summary>
        private bool WaitOrActiveArray(Symbol symbol, int i)
        {
            string[] info = new string[] { };
            foreach (var fInfo in formulasAppendInformation)
            {
                if (fInfo.Key == i)
                {
                    info = fInfo.Value;
                }
            }
            if (info.Length != 0)
            {
                if (info[0] == symbol.Name)
                {
                    if (info[1] == "0" && info[2] == "N")
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 待用表活跃表信息修改
        /// </summary>
        private void CalWaitOrActiveArray(List<Symbol> symbols, List<QuaternaryFormula> formulas, List<KeyValuePair<int, string[]>> appendInfo)
        {
            foreach (var f in formulas)
            {
                List<Symbol> symbol = (GetSymbol(f.arg1).Concat(GetSymbol(f.arg2)).Concat(GetSymbol(f.result))).ToList();
                foreach (var s in symbol)
                {
                    s.Wait = new List<string[]> { new string[] { "0", "N" } };
                }
            }
            for (int i = formulas.Count - 1; i >= 0; i--)
            {
                if (formulas[i].op == ":=")
                {
                    foreach (var s in GetSymbol(formulas[i].result))
                    {
                        string[] waitTemp = new string[] { s.Name, s.Wait.Last()[0], s.Wait.Last()[1] };
                        appendInfo.Add(new KeyValuePair<int, string[]>(i, waitTemp));
                        s.Wait.Add(new string[] { "0", "N" });
                    }
                    foreach (var s in GetSymbol(formulas[i].arg1))
                    {
                        string[] waitTemp = new string[] { s.Name, s.Wait.Last()[0], s.Wait.Last()[1] };
                        appendInfo.Add(new KeyValuePair<int, string[]>(i, waitTemp));
                        s.Wait.Add(new string[] { i.ToString(), "Y" });
                    }
                    foreach (var s in GetSymbol(formulas[i].arg2))
                    {
                        string[] waitTemp = new string[] { s.Name, s.Wait.Last()[0], s.Wait.Last()[1] };
                        appendInfo.Add(new KeyValuePair<int, string[]>(i, waitTemp));
                        s.Wait.Add(new string[] { i.ToString(), "Y" });
                    }
                }
            }
        }

        #region 产生目标代码
        private void Translate(List<QuaternaryFormula> f)
        {

            foreach (QuaternaryFormula formula in f)
            {
                if (formula.op == "j")
                {
                    Assembly assembly = new Assembly("JMP", "L" + formula.result, "");
                    assembly.Num = lineIndex.ToString();
                    assemblys.Add(assembly);
                }
                else if (formula.op == "j>")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JG", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "j<")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JL", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "j=")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JZ", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "j<>")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JNZ", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "j<=")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JLE", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "j>=")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("CMP", s, formula.arg2);
                    Assembly a3 = new Assembly("JGE", "L" + formula.result, "");
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "+")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("ADD", s, formula.arg2);
                    Assembly a3 = new Assembly("MOV", formula.result, s);
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "-")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("SUB", s, formula.arg2);
                    Assembly a3 = new Assembly("MOV", formula.result, s);
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "*")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("MUL", s, formula.arg2);
                    Assembly a3 = new Assembly("MOV", formula.result, s);
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == "/")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("DIV", s, formula.arg2);
                    Assembly a3 = new Assembly("MOV", formula.result, s);
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    assemblys.Add(a3);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                        if (sym.Name == formula.arg2)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else if (formula.op == ":=")
                {
                    string s = GetReg(formula);
                    Assembly a1 = new Assembly("MOV", s, formula.arg1);
                    a1.Num = lineIndex.ToString();
                    Assembly a2 = new Assembly("MOV", formula.result, s);
                    assemblys.Add(a1);
                    assemblys.Add(a2);
                    foreach (Symbol sym in symbols)
                    {
                        if (sym.Name == formula.arg1)
                        {
                            if (sym.Wait.Count > 1)
                            {
                                sym.Wait.RemoveAt(sym.Wait.Count - 1);
                            }
                        }
                    }
                }
                else
                {
                }
                lineIndex++;
            }
        }
        #endregion

    }
}
