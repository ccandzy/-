using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property)]//只用于修饰属性
    public class PropertyNameAttribute : Attribute
    {
        private string _fieldName;

        public PropertyNameAttribute(string fieldName)
        {
            this._fieldName = fieldName;
        }
        public string FieldName
        {
            get { return _fieldName; }
        }
    }
}
