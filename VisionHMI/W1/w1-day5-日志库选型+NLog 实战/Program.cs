using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_day5_日志库选型_NLog_实战
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ///跟习惯用lamdba表达式，管理UI线程
            Application.ThreadException += ((sender, e) =>
            {
                AppLogger.LogError(ErrorCodes.Unknown,"未捕获异常", e.Exception);
                // 同样立刻冲刷：这条是"致命错误"的证据，不能只留在内存队列里
                NLog.LogManager.Flush();
                try
                {
                    MessageBox.Show("程序遇到未处理的错误，已记录日志，请联系技术人员。",
                        "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.Write(ex2);
                }

            }
            );
            //UI外的线程
            AppDomain.CurrentDomain.UnhandledException += delegate (object sender, UnhandledExceptionEventArgs e)
            {
                // 这里只能做一件事：把证据留下来
                try
                {
                    Exception ex = e.ExceptionObject as Exception;
                    AppLogger.LogError(ErrorCodes.Unknown,"进程级未捕获异常", ex);
                    // 关键：进程马上就要终止了，NLog 异步队列里还压着的日志
                    // 必须在这里强制冲刷一次，否则这一半日志会随进程一起消失。
                    // （实测 net48 + AsyncWrapper：不加这行，20000 条只落 11094 条；
                //   加了这行，20000 条全部落盘。Flush 不会抛异常，放心调。）
                    NLog.LogManager.Flush();
                }
                // 进程即将终止，这里绝不能再抛
                catch { }
            };
            Application.Run(new Form1());

            // 正常退出路径：窗体被关闭后 Application.Run 返回，才会走到这一行。
            // 这里的 Flush 冲刷 Application.Run 期间积压的日志。
            // （NLog 默认 AutoShutdown=true 会在 ProcessExit 时自动兜底，
            //   但显式 Flush 更保险，也更直白——工程上我建议保留这一行。）
            NLog.LogManager.Flush();
        }
    }
}
