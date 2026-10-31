using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace w1_异常体系
{
    public static class WriteError
    {
        public static void WriteErrorlogs(string WhereText,Exception ex)
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string file = Path.Combine(dir, "error"+DateTime.Now.ToString("yyyyMMdd")+".txt");
                File.AppendAllText(file, $"[{DateTime.Now.ToString("yyyy:MM:dd HH-mm-ss")}][{WhereText}]{ex}\n", Encoding.UTF8);
            }
            catch(Exception ex2)
            {
                System.Diagnostics.Debug.WriteLine(ex2);
            }
        }
        //private static void WriteError2(string whereText, Exception ex)
        //{
        //    string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        //    string file = Path.Combine(dir, "error" + DateTime.Now.ToString("yyyyMMdd") + ".txt");
        //    string text = "[{0}] {1}{2}堆栈:{3}{2}"
        //        + string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now), whereText,
        //           Environment.NewLine, ex.ToString();

        //    File.AppendAllText(file, text + Environment.NewLine, Encoding.UTF8);
        //}
    }
}
