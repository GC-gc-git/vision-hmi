using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace w1_day6_w1总结
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
