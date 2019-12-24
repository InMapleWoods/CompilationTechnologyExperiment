using System;

namespace CompilationTechnologyExperiment
{
    /// <summary>
    /// 错误类
    /// </summary>
    [Serializable]
    public class ErrorException : Exception
    {
        public ErrorException(string str, string index = "") : base(str)
        {
            if (!string.IsNullOrEmpty(index))
                Console.WriteLine(DateTime.Now.ToString() + str + "出错在" + index);
            else
                Console.WriteLine(DateTime.Now.ToString() + str);
        }

        public ErrorException(string str) : base(str)
        {
            Console.WriteLine(DateTime.Now.ToString() + str);
        }
        public ErrorException()
        {
        }

        public ErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ErrorException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext streamingContext)
        {
            throw new NotImplementedException();
        }
    }
}
