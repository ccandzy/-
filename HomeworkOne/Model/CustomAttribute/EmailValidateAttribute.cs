using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [ErrorString(ErrorMessage = "邮箱输入不合法")]
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailValidateAttribute : BaseValidateAttribute
    {
        private string _validateString = @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
        public override bool Validate(object t)
        {
            return Regex.IsMatch(t.ToString(), _validateString);
        }
    }
}
