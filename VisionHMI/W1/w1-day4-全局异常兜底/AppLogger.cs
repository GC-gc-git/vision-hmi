using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace w1_day4_全局异常兜底
{
    public enum LogLevel
    {
        Debug=0,
        Info=1,
        Error=2
    }
    public static class AppLogger
    {
        private static readonly object _logLock = new object();
        public static LogLevel MinLevel = LogLevel.Debug;
        public static void LogDebug(string msg)
        {
            Write(LogLevel.Debug, msg, null, null);
        }
        public static void LogInfo(string msg)
        {
            Write(LogLevel.Info, msg, null, null);
        }
        /// <summary>
        /// 各参数代表含义，1-错误码，2-代表控件，3-报错原因及地址
        /// </summary>
        /// <param name="code"></param>
        /// <param name="whereText"></param>
        /// <param name="ex"></param>
        public static void LogError(string whereText, Exception ex)
        {
            Write(LogLevel.Error, null, whereText, ex);
        }
        private static void Write(LogLevel level,string msg,string whereText,Exception ex)
        {
            try
            {
                lock (_logLock)
                {
                    if (level < MinLevel) return;
                    string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string file = Path.Combine(dir, $"[{DateTime.Now.ToString("yyyy-MM-dd")}][{level.ToString()}].txt");
                    string time = $"{DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-fff")}";
                    if (level == LogLevel.Debug || level == LogLevel.Info)
                    {
                        File.AppendAllText(file, $"[{time}][{level.ToString().ToUpper()}][{msg}]\n",Encoding.UTF8);
                    }
                    else if (level == LogLevel.Error)
                    {
                        File.AppendAllText(file, $"[{time}][{level.ToString().ToUpper()}][{whereText}]{ex}\n",Encoding.UTF8);
                    }
                }
            }
            catch(Exception ex2)
            {
                // 日志自己绝不能抛异常，否则会盖掉原始异常
                System.Diagnostics.Debug.WriteLine(ex2);
            }
        }
    }
}
