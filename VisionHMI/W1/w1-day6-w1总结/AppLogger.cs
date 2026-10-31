using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day6_w1总结
{
    public static class AppLogger
    {
        private static readonly Logger _nlog = LogManager.GetCurrentClassLogger();
        public static event Action<string> OnLog;
        public static void LogDebug(string msg)
        {
            try
            {
                _nlog.Debug($"[{msg}]");
                OnLog?.Invoke(msg);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        public static void LogInfo(string msg)
        {
            try
            {
                _nlog.Info($"[{msg}]");
                OnLog?.Invoke(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        public static void LogWarn(string msg)
        {
            try
            {
                _nlog.Warn($"[{msg}]");
                OnLog?.Invoke(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            
        }
        public static void LogFatal(string msg)
        {
            try
            {
                _nlog.Fatal($"[{msg}]");
                OnLog?.Invoke(msg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            
        }
        public static void LogError(string code,string whereText,Exception ex)
        {
            try
            {
                string msg = $"[{code}][{whereText}]{ex}";
                _nlog.Error($"{msg}");
                OnLog?.Invoke(msg);
            }
            catch (Exception ex1)
            {
                System.Diagnostics.Debug.WriteLine(ex1);
            }
        }
    }
}
