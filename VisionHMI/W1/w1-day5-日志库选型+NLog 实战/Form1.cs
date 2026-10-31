using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using NLog;

namespace w1_day5_日志库选型_NLog_实战
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //结果测试两次，确认3000
            //for(int i = 0; i < 3000; i++)
            //{
            //    Logger.Write($"第{i}条\n");
            //}
            //Thread.Sleep(1200);
            //string[] str=File.ReadAllLines("log.txt");
            //MessageBox.Show(""+str.Length);

            //实测点击退出确认无数据写入
            //Logger.Write("第1条\n");
            //Logger.Write("第2条\n");
            //Logger.Write("第3条\n");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                AppLogger.LogDebug("相机打开");
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ErrorCodes.Unknown, "button2", ex);
                MessageBox.Show($"系统错误：{ErrorCodes.Unknown},请联系技术人员");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                AppLogger.LogInfo("VM方案加载成功");
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ErrorCodes.Unknown,"button3", ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                AppLogger.LogError(ErrorCodes.Unknown,"button4", ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 五级过滤测试：走你自己的 AppLogger 封装层，
            // 这样既验证了 NLog 的级别，也验证了 AppLogger 的转发是否正确
            AppLogger.LogDebug("D-调试");
            AppLogger.LogInfo("I-信息");
            AppLogger.LogWarn("W-警告");
            AppLogger.LogError(ErrorCodes.Unknown, "E-错误", new Exception("模拟错误"));
            AppLogger.LogFatal("F-致命");
        }
    }
}
