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

namespace w1_day3_错误码表
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
                throw new BizException(ErrorCodes.CameraConnectFailed,"相机连接失败，请检查网线和相机 IP");
            }
            catch(BizException ex)
            {
                // ② 记录层：日志里必须带码
                AppLogger.LogError(ex.Code, "相机连接失败", ex);

                // ③ 提示层：给客户能照着做的一句话
                MessageBox.Show($"[{ex.Code}] {ex.Message}", "出错了");
                return;
            }
            catch (Exception ex)
            {
                // 没被接住的意外，一律给兜底码 + 写完整堆栈
                AppLogger.LogError(ErrorCodes.Unknown, "未预期异常", ex);
                MessageBox.Show("[E9001] 系统未知错误，已记录日志，请联系技术人员","出错了");
            }
            finally
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                throw new BizException(ErrorCodes.CameraGrabTimeout, "取流超时");
            }
            catch (BizException ex)
            {
                // ② 记录层：日志里必须带码
                AppLogger.LogError(ex.Code, "取流超时", ex);

                // ③ 提示层：给客户能照着做的一句话
                MessageBox.Show($"[{ex.Code}] {ex.Message}", "出错了");
                return;
            }
            catch (Exception ex)
            {
                // 没被接住的意外，一律给兜底码 + 写完整堆栈
                AppLogger.LogError(ErrorCodes.Unknown, "未预期异常", ex);
                MessageBox.Show("[E9999] 系统未知错误，已记录日志，请联系技术人员", "出错了");
            }
            finally
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                throw new BizException(ErrorCodes.ModbusReadTimeout, "读寄存器超时");
            }
            catch (BizException ex)
            {
                // ② 记录层：日志里必须带码
                AppLogger.LogError(ex.Code, "读寄存器超时", ex);

                // ③ 提示层：给客户能照着做的一句话
                MessageBox.Show($"[{ex.Code}] {ex.Message}", "出错了");
                return;
            }
            catch (Exception ex)
            {
                // 没被接住的意外，一律给兜底码 + 写完整堆栈
                AppLogger.LogError(ErrorCodes.Unknown, "未预期异常", ex);
                MessageBox.Show("[E9001] 系统未知错误，已记录日志，请联系技术人员", "出错了");
            }
            finally
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                throw new BizException(ErrorCodes.ModbusWriteFailed, "写入寄存器失败");
            }
            catch (BizException ex)
            {
                // ② 记录层：日志里必须带码
                AppLogger.LogError(ex.Code, "写入寄存器失败", ex);

                // ③ 提示层：给客户能照着做的一句话
                MessageBox.Show($"[{ex.Code}] {ex.Message}", "出错了");
                return;
            }
            catch (Exception ex)
            {
                // 没被接住的意外，一律给兜底码 + 写完整堆栈
                AppLogger.LogError(ErrorCodes.Unknown, "未预期异常", ex);
                MessageBox.Show("[E9001] 系统未知错误，已记录日志，请联系技术人员", "出错了");
            }
            finally
            {

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string str = null;
                int len = str.Length;
            }
            catch (BizException ex)
            {
                // ② 记录层：日志里必须带码
                AppLogger.LogError(ex.Code, "项目错误", ex);

                // ③ 提示层：给客户能照着做的一句话
                MessageBox.Show($"[{ex.Code}] {ex.Message}", "出错了");
                return;
            }
            catch (Exception ex)
            {
                // 没被接住的意外，一律给兜底码 + 写完整堆栈
                AppLogger.LogError(ErrorCodes.Unknown, "未预期异常", ex);
                MessageBox.Show("[E9001] 系统未知错误，已记录日志，请联系技术人员", "出错了");
            }
            finally
            {

            }
        }
    }
}
