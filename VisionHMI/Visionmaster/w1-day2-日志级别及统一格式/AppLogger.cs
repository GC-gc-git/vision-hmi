using System;
using System.IO;
using System.Text;

namespace w1_day2_日志级别及统一格式
{
    /// <summary>
    /// 日志级别。数值顺序刻意递增：Debug=0 &lt; Info=1 &lt; Error=2
    /// 这样才能用一行 if (level &lt; MinLevel) return; 做过滤
    /// </summary>
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Error = 2
    }

    /// <summary>
    /// 应用日志器
    /// </summary>
    public static class AppLogger
    {
        /// <summary>
        /// 最低级别开关（旋钮）。低于这个级别的日志不落盘。
        /// 改成 LogLevel.Error 就只剩错误日志。
        /// </summary>
        public static LogLevel MinLevel = LogLevel.Error;

        // ==================== 三个公开入口 ====================

        /// <summary>
        /// 开发调试细节
        /// </summary>
        public static void LogDebug(string msg)
        {
            Write(LogLevel.Debug, msg, null, null);
        }

        /// <summary>
        /// 业务流程节点
        /// </summary>
        public static void LogInfo(string msg)
        {
            Write(LogLevel.Info, msg, null, null);
        }

        /// <summary>
        /// 出错了，需要人看
        /// </summary>
        public static void LogError(string whereText, Exception ex)
        {
            Write(LogLevel.Error, null, whereText, ex);
        }

        // ==================== 唯一一处写文件逻辑 ====================

        /// <summary>
        /// 所有日志都走这里。级别作为参数传进来，代码只写一份。
        /// </summary>
        private static void Write(LogLevel level, string msg, string whereText, Exception ex)
        {
            try
            {
                // 【作用点 1】比较大小：级别不够格就不落盘
                if (level < MinLevel)
                {
                    return;
                }

                // 日志目录
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                // 【作用点 2】决定用哪个文件：Error 独立，其余归到 info
                string prefix = level == LogLevel.Error ? "error" : "info";

                // 按天分文件
                string file = Path.Combine(dir, prefix + DateTime.Now.ToString("yyyyMMdd") + ".txt");

                // 【作用点 3】标签自动生成，不用硬编码 "DEBUG"/"INFO"/"ERROR"
                string tag = level.ToString().ToUpper();

                // 时间戳带毫秒（单元 2C 排锁问题要靠它）
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                // ========== TODO 1：在这里拼出 content 字符串 ==========
                // 要求：
                //   Error -> [时间][ERROR][来源] 异常类型 + 堆栈
                //   其他  -> [时间][INFO ] 或 [DEBUG][消息]
                //   都要以 \n 结尾
                // 提示：ex 不为 null 时用 {ex}（会隐式调用 ToString，含类型名和堆栈）
                //      ex 为 null 时用 msg
                string content = null;
                if (ex == null)
                {
                     content = $"[{time}][{tag}][{msg}]\n";
                }
                else
                {
                     content = $"[{time}][{tag}][{whereText}]{ex}\n";
                }

                // ========== TODO 2：写文件（记得加编码） ==========
                // 提示：File.AppendAllText(路径, 内容, Encoding.UTF8)
                //       不指定编码会用系统默认（中文 Windows 是 GBK），中文会乱码
                File.AppendAllText(file, content, Encoding.UTF8);
            }
            catch (Exception ex2)
            {
                // 日志自己绝不能抛异常，否则会盖掉原始异常
                System.Diagnostics.Debug.WriteLine(ex2);
            }
        }
    }
}
