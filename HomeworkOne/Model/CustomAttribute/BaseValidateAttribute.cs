using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    public abstract class BaseValidateAttribute:Attribute
    {
        public abstract bool Validate(object t);
    }
}
