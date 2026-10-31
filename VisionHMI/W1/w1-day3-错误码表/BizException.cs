using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day3_错误码表
{
    public class BizException:Exception
    {
        public string Code { get; private set; }
        public BizException(string code,string message):base(message)
        {
            this.Code = code;
        }
    }
}
