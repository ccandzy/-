using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    /// <summary>
    /// 用于验证错误时的提示信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]//只用于修饰类
    public class ErrorStringAttribute:Attribute
    {
        public string ErrorMessage { get; set; }
    }
}
