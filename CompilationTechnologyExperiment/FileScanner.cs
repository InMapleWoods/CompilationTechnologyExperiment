using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 扫描器类
    /// </summary>
    public static partial class FileScanner
    {
        public static List<Token> tokens { get; set; } = new List<Token>();
        public static List<Symbol> symbols { get; set; } = new List<Symbol>();
        private static string error = "";
        private static int count = 0;
        /// <summary>
        /// 获取文件内容并识别
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>识别结果</returns>
        public static string[] ScannerResult(string fileName)
        {
            count = 0;
            error = "[";
            string token = "[]";
            string symbol = "[]";
            try
            {
                var values = GetContentKeyValues(ProcessContent(Tools.GetFileContent(fileName)));
                GetTokens(values);
                GetSymbols();
                token = GetTokenFile(tokens);
                symbol = GetSymbolFile(symbols);
            }
            catch
            {
            }
            error = error.Substring(0, error.Length - 1);
            if (!string.IsNullOrEmpty(error))
                error += "]";
            return new string[] { token, symbol, error };
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
            string output = input.Replace("\r", " ");
            output = output.Replace("\n", " ");
            output = output.Replace("\t", " ");
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
                try
                {
                    if (!Tools.IsLegal(input, i))
                    {
                        i++;
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
                    if (identifyKeywordOrIdentifier(input, ref i, builder, result))
                    {
                        continue;
                    }
                }
                catch
                {
                    i++;
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
            while (char.IsLetter(input[index]) || char.IsNumber(input[index]))
            {
                builder.Append(input[index]);
                if (!char.IsLetter(input[index + 1]) && !char.IsNumber(input[index
                    + 1]))
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
                        string keyWordPattern = @"(\d{1,})([A-Za-z]{2,})";//数字开头的数字、字母串
                        if (!Regex.IsMatch(word, keyWordPattern))
                        {
                            result.Add(new KeyValuePair<string, int>(word, 40));//返回标识符的种别编码40
                        }
                        else
                        {
                            error += "[\"" + ErrorMessageResource.IdStartWithNumber + "\",\"" + word + "\"],";
                            Match match = Regex.Match(word, keyWordPattern);
                            result.Add(new KeyValuePair<string, int>(match.Groups[1].Value.ToString(), 41));
                        }
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
            int start = index;
            bool returnValue = false;
            bool isDouble = false;
            while (char.IsNumber(input[index]))
            {
                builder.Append(input[index]);
                if (!char.IsNumber(input[index + 1]))
                {
                    if (input[index + 1] == 'e' || input[index + 1] == 'E' || input[index + 1] == '.')
                    {
                        if (!isDouble)
                        {
                            isDouble = true;
                            builder.Append(input[index + 1]);
                            index = index + 1;
                        }
                        else
                        {
                            string number = builder.ToString();
                            result.Add(new KeyValuePair<string, int>(number, 42));
                            builder.Clear();
                            if (input[index + 1] != '.')
                            {
                                error += "[\"" + ErrorMessageResource.NumberMultiE + "\",\"" + number + "\"],";
                            }
                            else
                            {
                                error += "[\"" + ErrorMessageResource.NumberMultiDotError + "\",\"" + input.Substring(start, Tools.MoveToRowEnd(input, index) - start) + "\"],";
                            }
                            index = Tools.MoveToRowEnd(input, index) - 1;
                        }
                    }
                    else
                    {
                        string number = builder.ToString();
                        if (!isDouble)
                        {
                            result.Add(new KeyValuePair<string, int>(number, 41));
                        }
                        else
                        {
                            result.Add(new KeyValuePair<string, int>(number, 42));
                        }
                        builder.Clear();
                        if (input[index + 1] != ';')
                        {
                            error += "[\"" + ErrorMessageResource.IdStartWithNumber + "\",\"" + input.Substring(start, Tools.MoveToRowEnd(input, index) - start) + "\"],";
                        }
                        index = Tools.MoveToRowEnd(input, index) - 1;
                        returnValue = true;
                    }
                }
                index++;
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
                result.Add(new KeyValuePair<string, int>(charValue, 43));
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
            if (input[index] == '"')
            {
                index = index + 1;
                while (input[index] != '"')
                {
                    builder.Append(input[index]);
                    index++;
                }
                string stringValue = builder.ToString();
                builder.Clear();
                returnValue = true;
                result.Add(new KeyValuePair<string, int>(stringValue, 45));
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
                if (input[index] == '>' || input[index] == ':')
                {
                    if (input[index + 1] == '=')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 1;
                    }
                }
                else if (input[index] == '<')
                {
                    if (input[index + 1] == '=')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 1;
                    }
                    else if (input[index + 1] == '>')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 1;

                    }
                }
                if (input[index] == '*')
                {
                    if (input[index + 1] == '/')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 1;

                    }
                }
                else if (input[index] == '/')
                {
                    if (input[index + 1] == '*')
                    {
                        builder.Append(input[index + 1]);
                        index = index + 1;

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
        /// 获取符号表文件字符串（JSON格式）
        /// </summary>
        /// <param name="symbols">Symbol表</param>
        /// <returns>符号表文件字符串</returns>
        public static string GetSymbolFile(List<Symbol> symbols)
        {
            try
            {
                if (symbols == null)
                {
                    return "";
                }
                string str = "\t序号\tNumber:\tName:\tType:\tValue:\tWait\r\n";
                for (int i = 0; i < symbols.Count; i++)
                {
                    str += "\t(" + i + ")\t" + symbols[i].Number + "\t" + symbols[i].Name + "\t" + symbols[i].Type + "\t" + symbols[i].Value + "\t" + Newtonsoft.Json.JsonConvert.SerializeObject(symbols[i].Wait) + "\r\n";
                }
                return str;
            }
            catch (ErrorException e)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取token文件字符串（JSON格式）
        /// </summary>
        /// <param name="tokens">Token表</param>
        /// <returns>token文件字符串</returns>
        public static string GetTokenFile(List<Token> tokens)
        {
            try
            {
                if (tokens == null)
                {
                    return "";
                }
                string str = "\t序号\tLabel:\tName:\tCode:\tAddr:\r\n";
                for (int i = 0; i < tokens.Count; i++)
                {
                    str += "\t(" + i + ")\t" + tokens[i].Label + "\t" + tokens[i].Name + "\t" + tokens[i].Code + "\t" + tokens[i].Addr + "\r\n";
                }
                return str;
            }
            catch (ErrorException e)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取tokens
        /// </summary>
        /// <param name="values">值表</param>
        /// <returns>token表</returns>
        public static void GetTokens(List<KeyValuePair<string, int>> values)
        {
            try
            {
                tokens.Clear();
                symbols.Clear() ;
                if (values == null)
                {
                    return;
                }
                foreach (var i in values)
                {
                    Token token = new Token();
                    token.Label = tokens.Count;
                    token.Name = i.Key;
                    token.Code = i.Value;
                    token.Addr = symbols.Count;
                    if (i.Value.ToString() == CompilationTechnologyExperiment.Keywords.ID)
                    {
                        Symbol symbol = new Symbol();
                        symbol.Number = token.Addr;
                        symbol.Name = i.Key;
                        symbol.Type = i.Value;
                        symbols.Add(symbol);
                    }
                    else if (i.Value.ToString() == CompilationTechnologyExperiment.Keywords.整型)
                    {
                        Symbol symbol = new Symbol();
                        symbol.Number = token.Addr;
                        symbol.Name = i.Key;
                        symbol.Type = i.Value;
                        symbols.Add(symbol);
                    }
                    else if (i.Value.ToString() == CompilationTechnologyExperiment.Keywords.实型)
                    {
                        Symbol symbol = new Symbol();
                        symbol.Number = token.Addr;
                        symbol.Name = i.Key;
                        symbol.Type = i.Value;
                        symbols.Add(symbol);
                    }
                    else if (i.Value.ToString() == CompilationTechnologyExperiment.Keywords.TRUE|| i.Value.ToString() == CompilationTechnologyExperiment.Keywords.FALSE)
                    {
                        Symbol symbol = new Symbol();
                        symbol.Number = token.Addr;
                        symbol.Name = i.Key;
                        symbol.Type = i.Value;
                        symbols.Add(symbol);
                    }
                    tokens.Add(token);
                }
            }
            catch (ErrorException e)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取symbols
        /// </summary>
        /// <param name="values">值表</param>
        /// <returns>token表</returns>
        public static void GetSymbols()
        {
            try
            {
                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i].Code.ToString() == CompilationTechnologyExperiment.Keywords.INTEGER || tokens[i].Code.ToString() == CompilationTechnologyExperiment.Keywords.BOOL || tokens[i].Code.ToString() == CompilationTechnologyExperiment.Keywords.REAL)//类型为integer或bool或real
                    {
                        int j = i;
                        j = j - 2;
                        symbols[tokens[j].Addr].Type = tokens[i].Code;//类型定义正确，在符号表中记录该标识符的类型
                        j--;
                        while (tokens[j].Code.ToString() == CompilationTechnologyExperiment.Keywords.DOUHAO)// 若标识符后面有逗号，表示同时定义了几个相同类型的变量，把它们都添加到符号表中
                        {
                            j--;
                            symbols[tokens[j].Addr].Type = tokens[i].Code;
                            j--;
                        }
                    }
                }
            }
            catch (ErrorException e)
            {
                throw;
            }
        }
    }
}
