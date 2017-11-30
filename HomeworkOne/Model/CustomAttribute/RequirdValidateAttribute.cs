using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [ErrorString(ErrorMessage ="值不能为空")]
    [AttributeUsage(AttributeTargets.Property)]
    public class RequirdValidateAttribute : BaseValidateAttribute
    {
        public override bool Validate(object t)
        {
            return t != null;
        }
    }
}
