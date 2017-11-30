using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]//只用于修饰属性
    public class ChineseNameAttribute:Attribute
    {
        public string ChineseName { get; set; }
    }
}
