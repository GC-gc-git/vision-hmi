using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day5_日志库选型_NLog_实战
{
    public static class AppLogger
    {
        private static readonly Logger _nlog = LogManager.GetCurrentClassLogger();
        public static event Action<string> OnLog;
        public static void LogDebug(string msg)
        {
            try
            {
                _nlog.Debug(msg);
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                // 日志自己绝不能抛异常，否则会盖掉原始异常
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
        public static void LogInfo(string msg)
        {
            try
            {
                _nlog.Info(msg);
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
        public static void LogWarn(string msg)
        {
            try
            {
                _nlog.Warn(msg);
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
        public static void LogFatal(string msg)
        {
            try
            {
                _nlog.Fatal(msg);
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
        public static void LogError(string code,string whereText,Exception ex)
        {
            try
            {
                string msg = $"[{code}][{whereText}]{ex}";
                _nlog.Error(msg);
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
        //private static void Write(string name,string msg)
        //{
        //    try
        //    {
        //        if (name == "Debug")
        //        {
        //            _nlog.Debug($"[{msg}]");
        //        }
        //        else if (name == "Info")
        //        {
        //            _nlog.Info($"[{msg}]");
        //        }
        //        else if (name == "Error")
        //        {
        //            _nlog.Error($"{msg}");
        //        }
        //    }
        //    catch(Exception ex1)
        //    {
        //        // 日志自己绝不能抛异常，否则会盖掉原始异常
        //        System.Diagnostics.Debug.WriteLine(ex1);
        //    }
        //}
    }
}
