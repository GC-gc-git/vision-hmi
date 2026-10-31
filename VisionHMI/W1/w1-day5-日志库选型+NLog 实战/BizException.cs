using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day5_日志库选型_NLog_实战
{
    public class BizException:Exception
    {
        public string Code { get; set; }
        public BizException(string code,string message):base(message)
        {
            this.Code = code;
        }
    }
}
