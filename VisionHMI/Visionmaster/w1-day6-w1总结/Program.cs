using NLog;
using NLog.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace w1_day6_w1总结
{
    internal static class Program
    {
        // 默认配置模板：配置缺失或损坏时用它重建一份
        private const string DefaultNLogConfig = @"<?xml version=""1.0"" encoding=""utf-8""?>
<nlog xmlns=""http://www.nlog-project.org/schemas/NLog.xsd""
      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" autoReload=""true"">
  <targets>
    <target name=""asyncFile"" xsi:type=""AsyncWrapper""
            queueLimit=""10000"" overflowAction=""Block""
            batchSize=""200"" timeToSleepBetweenBatches=""0"">
      <target xsi:type=""File""
              fileName=""${basedir}/logs/${shortdate}.log""
              layout=""${longdate}|${level:uppercase=true}|${message}""
              keepFileOpen=""false""
              archiveAboveSize=""10485760"" maxArchiveFiles=""7"" />
    </target>
  </targets>
  <rules>
    <logger name=""*"" minlevel=""Debug"" writeTo=""asyncFile"" />
  </rules>
</nlog>";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            #region 全局兜底异常
            //UI线程兜底
            //Application.ThreadException += ((sender, e) =>
            //{
            //    AppLogger.LogError(ErrorCodes.Unknown, "未捕获异常", e.Exception);
            //    // 同样立刻冲刷：这条是"致命错误"的证据，不能只留在内存队列里
            //    NLog.LogManager.Flush();
            //    try
            //    {
            //        MessageBox.Show("程序遇到未处理的错误，已记录日志，请联系技术人员。",
            //            "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    catch (Exception ex2)
            //    {
            //        System.Diagnostics.Debug.Write(ex2);
            //    }

            //}
            //);
            //UI线程外兜底
            //UI外的线程
            //AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            //{
            //    // 这里只能做一件事：把证据留下来
            //    try
            //    {
            //        Exception ex = e.ExceptionObject as Exception;
            //        AppLogger.LogError(ErrorCodes.Unknown, "进程级未捕获异常", ex);
            //        // 关键：进程马上就要终止了，NLog 异步队列里还压着的日志
            //        // 必须在这里强制冲刷一次，否则这一半日志会随进程一起消失。
            //        // （实测 net48 + AsyncWrapper：不加这行，20000 条只落 11094 条；
            //        //   加了这行，20000 条全部落盘。Flush 不会抛异常，放心调。）
            //        NLog.LogManager.Flush();
            //    }
            //    // 进程即将终止，这里绝不能再抛
            //    catch { }
            //};
            #endregion
            // ① 内部日志：NLog 自己出问题时，错误原因要能留下证据
            //    必须用编程接口，不能靠 XML（实测：XML 写 internalLogFile 无效）
            InternalLogger.LogLevel = LogLevel.Warn;
            InternalLogger.LogToConsole = true;
            InternalLogger.LogFile = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "logs", "nlog-internal.log");
            InternalLogger.IncludeTimestamp = true;
            LogManager.ThrowExceptions = false;

            // ② 配置兜底：缺失或损坏就自己写一份默认的，答：收到
            EnsureNLogConfig();

            // ③ UI 线程未捕获异常
            Application.ThreadException += (delegate (object sender, ThreadExceptionEventArgs e)
            {
                try
                {
                    AppLogger.LogError(ErrorCodes.Unknown, "未捕获异常", e.Exception);
                }
                catch { }
                finally
                {
                    // 致命错误：立刻落盘，不能只留在内存队列里
                    LogManager.Flush();
                }

                try
                {
                    MessageBox.Show("程序遇到未处理的错误，已记录日志，请联系技术人员。",
                        "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.Write(ex2);
                }
            });

            // ④ 非 UI 线程未捕获异常（进程即将终止）
            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            {
                try
                {
                    Exception ex = e.ExceptionObject as Exception;
                    AppLogger.LogError(ErrorCodes.Unknown, "进程级未捕获异常", ex);
                    // 关键：进程马上要死了，这是队列里日志的最后机会
                    LogManager.Flush();
                }
                catch { }
            };

            // ⑤ 启动信息：先知道程序起来了、配置好了
            AppLogger.LogInfo("程序启动，配置自愈检查通过");
            Application.Run(new Form1());
            // ⑥ 正常退出路径：窗体关闭后 Application.Run 才返回
            AppLogger.LogInfo("程序正常退出");
            LogManager.Flush();
            LogManager.Shutdown();
        }

        /// <summary>确保 NLog.config 存在且是合法 XML。缺失或损坏就生成默认配置。</summary>
        private static void EnsureNLogConfig()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NLog.config");
            if (IsConfigUsable(path)) return;

            try
            {
                File.WriteAllText(path, DefaultNLogConfig, new UTF8Encoding(false));
                System.Diagnostics.Debug.WriteLine("[自愈] NLog.config 缺失或损坏，已重建默认配置");
            }
            catch (Exception ex)
            {
                // 连默认配置都写不了（权限问题），只能放弃，不能抛
                System.Diagnostics.Debug.WriteLine("[自愈失败] " + ex.Message);
            }
        }

        /// <summary>配置是否可用：文件在、XML 合法、根节点是 nlog</summary>
        private static bool IsConfigUsable(string path)
        {
            if (!File.Exists(path)) return false;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                return doc.DocumentElement != null
                    && doc.DocumentElement.Name == "nlog";
            }
            catch
            {
                return false;
            }
        }
    }
    }

