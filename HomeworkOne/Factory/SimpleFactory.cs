using IService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    public static class SimpleFactory
    {
        private static string SqlString = ConfigurationManager.AppSettings["sqlService"];
        private static string SqlDllName = SqlString.Split(';')[0];
        private static string SqlTypeName = SqlString.Split(';')[1];

        /// <summary>
        /// 根据配置文件创建一个IDataServices
        /// </summary>
        /// <returns></returns>
        public static IDataServices CreateService()
        {
            Assembly assembly = Assembly.Load(SqlDllName);//读取的是哪个DLL
            Type type = assembly.GetType(SqlTypeName);//读取DLL中的某个类 差点搞混淆
            return Activator.CreateInstance(type) as IDataServices;
        }
    }
}
