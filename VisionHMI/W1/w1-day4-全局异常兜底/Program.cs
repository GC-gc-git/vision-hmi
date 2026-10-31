using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_day4_全局异常兜底
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
            ///匿名写法
            //Application.ThreadException += delegate (object sender, ThreadExceptionEventArgs e)
            //{
            //    // ① 先写日志 —— 顺序不能反
            //    AppLogger.LogError("未捕获异常", e.Exception);

            //    // ② 再弹提示 —— 而且要自己包 try
            //    try
            //    {
            //        MessageBox.Show("程序遇到未处理的错误，已记录日志，请联系技术人员。",
            //                        "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    catch (Exception ex2)
            //    {
            //        // 弹窗自己都失败了，只能往控制台写，绝不能再抛
            //        System.Diagnostics.Debug.WriteLine(ex2);
            //    }
            //};
            ///跟习惯用lamdba表达式，管理UI线程
            Application.ThreadException += ((sender, e) =>
            {
                AppLogger.LogError("未捕获异常", e.Exception);
                try
                {
                    MessageBox.Show("程序遇到未处理的错误，已记录日志，请联系技术人员。", 
                        "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch(Exception ex2)
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
                    AppLogger.LogError("进程级未捕获异常", ex);
                }
                // 进程即将终止，这里绝不能再抛
                catch { }
            };
            //TaskScheduler.UnobservedTaskException += (sender, ex) =>
            //{
            //    // 这里只能做一件事：把证据留下来
            //    try
            //    {
            //        Exception ex2 = ex.Exception as Exception;
            //        AppLogger.LogError("进程级未捕获异常", ex2);
            //    }
            //    catch
            //    {

            //    }
            //};
            Application.Run(new Form1());
        }
    }
}
