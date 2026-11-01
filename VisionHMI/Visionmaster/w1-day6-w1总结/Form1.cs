using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace w1_day6_w1总结
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
                AppLogger.LogDebug("记录");
                AppLogger.LogInfo("在次记录");
                AppLogger.LogWarn("再再次记录");
                AppLogger.LogFatal("再再再次记录");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"异常报错，{ErrorCodes.Unknown}{ex}，请联系技术人员");
            }
        }
    }
}
