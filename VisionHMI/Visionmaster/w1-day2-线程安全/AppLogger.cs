using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day2_线程安全
{
    enum LogLevel
    {
        Debug=0,
        Info=1,
        Error=2
    }
    public static class AppLogger
    {
        private  static readonly object _logLock = new object();
         static LogLevel MinLevel = LogLevel.Debug;
        public static void LogDebug(string msg)
        {
            Write(LogLevel.Debug, msg, null, null);
        }
        public static void LogInfo(string msg)
        {
            Write(LogLevel.Info, msg, null, null);
        }
        public static void LogError(string whereText,Exception ex)
        {
            Write(LogLevel.Error, null, whereText, ex);
        }
        private static void Write(LogLevel level,string msg,string whereText,Exception ex)
        {
            try
            {
                lock (_logLock)
                {
                    if (level < MinLevel)
                    {
                        return;
                    }
                    string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string file = Path.Combine(dir, $"{level.ToString()}{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
                    string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm-ss-fff");
                    if (level == LogLevel.Debug)
                    {
                        File.AppendAllText(file, $"[{time}][{level.ToString()}][{msg}]\n", Encoding.UTF8);
                    }
                    else if (level == LogLevel.Info)
                    {
                        File.AppendAllText(file, $"[{time}][{level.ToString()}][{msg}]\n", Encoding.UTF8);
                    }
                    else if (level == LogLevel.Error)
                    {
                        File.AppendAllText(file, $"[{time}][{level.ToString()}][{whereText}]{ex}\n", Encoding.UTF8);
                    }
                }
            }
            catch (Exception ex2)
            {
                // 日志自己绝不能抛异常，否则会盖掉原始异常
                System.Diagnostics.Debug.WriteLine(ex2);
            }
        }
    }
}
