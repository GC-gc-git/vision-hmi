using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day3_错误码表
{
    public static class ErrorCodes
    {
        // E1xxx 设备 / 采集类
        public const string CameraConnectFailed = "E1001";//相机连接失败
        public const string CameraGrabTimeout = "E1002";//取流超时

        // E2xxx 通讯类
        public const string ModbusReadTimeout = "E2001";//读寄存器超时
        public const string ModbusWriteFailed = "E2002";//写寄存器失败

        // E3xxx 参数 / 流程类
        public const string VisionSchemeLoadFailed = "E3001";//VM 方案加载失败

        // E9xxx 兜底
        public const string Unknown = "E9999";//所有未捕获的异常、空引用等代码缺陷，统一返回 E9999
    }
}
