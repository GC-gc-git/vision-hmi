using System;
using System.Windows.Forms;

namespace w1_day2_日志级别及统一格式
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // INFO：记录程序启动这个流程节点
            // 注意：这里写完整的句子，不要写"项目"再让日志类加"被打开"后缀
            AppLogger.LogInfo("程序启动");
        }

        // ==================== 缺陷类异常：记日志 ====================

        /// <summary>
        /// 空指针异常（缺陷类）
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string str = null;
                int len = str.Length;      // 这行必崩
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                AppLogger.LogError("button1", ex);
            }
        }

        /// <summary>
        /// 索引超出界限（缺陷类）
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string[] str = new string[10];
                str[11] = "Holle";         // 这行必崩
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                AppLogger.LogError("button2", ex);
            }
        }

        /// <summary>
        /// 字符串转数字失败（预期失败类）
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse("ewqr124qwr");
            }
            catch (FormatException ex)
            {
                // 预期失败类：只提示用户，不记错误日志
                MessageBox.Show("请输入合法的数字");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                AppLogger.LogError("button3", ex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                AppLogger.LogDebug("相机曝光时间=1200us");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                AppLogger.LogError("button5", ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                AppLogger.LogInfo("相机打开");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"操作失败：{ex.Message}\n错误已记录，请联系技术支持。");
                AppLogger.LogError("button4", ex);
            }
        }

        

    }
}
