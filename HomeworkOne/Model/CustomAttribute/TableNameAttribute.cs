using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class)]//只用于修饰类
    public class TableNameAttribute:Attribute
    {
        private string _tableName;
        public TableNameAttribute(string tableName)
        {
            this._tableName = tableName;
        }
        public string TableName
        {
            get { return _tableName; }
        }
    }
}
