using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_day2_线程安全
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    for(int i = 0; i < 50; i++)
                    {
                        AppLogger.LogInfo($"t1[{i}]");
                    }
                });
                Task.Run(() =>
                {
                    for (int i = 0; i < 50; i++)
                    {
                        AppLogger.LogInfo($"t2[{i}]");
                    }
                });
                Task.Run(() =>
                {
                    for (int i = 0; i < 50; i++)
                    {
                        AppLogger.LogInfo($"t3[{i}]");
                    }
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show($"报错原因：{ex}，请联系技术人员");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"logs\\info{DateTime.Now.ToString("yyyy-MM-dd")}.txt");
                string[] part = File.ReadAllLines(file, Encoding.UTF8);
                int a = 0;
                int b = 0;
                int c = 0;
                for (int i = 0; i < part.Length; i++)
                {
                    if (part[i].Contains("t1"))
                    {
                        a++;
                    }
                    else if (part[i].Contains("t2"))
                    {
                        b++;
                    }
                    else if (part[i].Contains("t3"))
                    {
                        c++;
                    }
                }
                MessageBox.Show($"t1={a}个，t2={b}，t3={c}个");
            }
            catch(Exception ex)
            {
                AppLogger.LogError("button2", ex);
            }
        }
    }
}
