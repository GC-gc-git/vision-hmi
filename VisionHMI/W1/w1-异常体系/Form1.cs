using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_异常体系
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 空指针异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string str = null;
                int len = str.Length;
            }
            //这是缺陷类，也是空指针异常
            catch(NullReferenceException ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                //ex.Message是给你报错原因，ex.StackTrace是报错的具体位置（指的是哪个盘符下的那个文件夹下的文件中的哪一行代码是报错原因）
                //MessageBox.Show($"{ex}");
                //MessageBox.Show($"出错了：{ex.Message}\n{ex.StackTrace}");
                WriteError.WriteErrorlogs("button1", ex);
                //MessageBox.Show("出错了:" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch ( Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                //MessageBox.Show($"{ex}");
                WriteError.WriteErrorlogs("button1", ex);
                //MessageBox.Show(ex.Message);
                //WriteError.WriteErrorlogs("log.txt", ex);
            }
        }
        /// <summary>
        /// 索引超出界限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] str = new string[10];
                str[11] = "Holle";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                //MessageBox.Show(ex.Message);
                //WriteError.WriteErrorlogs("log.txt", ex);
                WriteError.WriteErrorlogs("button2", ex);
            }
        }
        /// <summary>
        /// 字符串编码问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse("ewqr124qwr");
            }
            catch (FormatException ex)
            {
                MessageBox.Show("请输入合法的数字");   // 提示用户，不记堆栈
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                //MessageBox.Show($"{ex}");
                //WriteError.WriteErrorlogs("button3", ex);
            }
        }
    }
}
