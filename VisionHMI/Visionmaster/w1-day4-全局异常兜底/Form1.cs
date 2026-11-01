using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_day4_全局异常兜底
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
                throw new Exception("这是故意崩的");
            //}
            //catch(Exception ex)
            //{
                //MessageBox.Show($"{ex.Message}");
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                throw new Exception("这是故意崩的");
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = null;
            int num = str.Length;
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(() => { throw new Exception("后台线程崩了"); });
            }
            catch (Exception ex)
            {
                AppLogger.LogError("Task里的异常（已接住）", ex);
                MessageBox.Show("接住了，程序没崩");
            }
        }
    }
}
