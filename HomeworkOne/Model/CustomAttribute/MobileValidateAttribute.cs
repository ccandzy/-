using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [ErrorString(ErrorMessage = "手机号码输入不合法")]
    [AttributeUsage(AttributeTargets.Property)]
    public class MobileValidateAttribute : BaseValidateAttribute
    {
        private long _maxValue = 18900000000;
        private long _minValue = 13000000000;
        public override bool Validate(object t)
        {
            var value = Convert.ToInt64(t);
            return _minValue < value && _maxValue > value;
        }
    }
}
