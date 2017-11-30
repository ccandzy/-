using Model;
using Model.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ReflectionHelper
{
    public static class ReflectionHelper
    {
        #region 通过反射把datareader转换成对象
        /// <summary>
        /// 通过反射把dataReader转化为T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader">本来一开始是写的SqlDataReader作废参数，后面改成了DbDataReader</param>
        /// <returns>对象</returns>
        public static T ConvertDataReaderToInstance<T>(DbDataReader dataReader)
        {
            T instance = Activator.CreateInstance<T>();
            //是否异常都关闭dataReader
            using (dataReader)
            {
                if (dataReader.Read())
                {
                    SetPropertyValue(dataReader, instance);
                }
            }
            return instance;
        }

        /// <summary>
        /// 通过反射把DataReader转换成ListInstance
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="dataReader">DataReader</param>
        /// <returns>List<T>集合对象</returns>
        public static List<T> ConvertDataReaderToListInstance<T>(DbDataReader dataReader) where T:new()
        {
            List<T> listT = new List<T>();
            using (dataReader)
            {
                while(dataReader.Read())
                {
                    T instance = new T();
                    SetPropertyValue(dataReader, instance);
                    listT.Add(instance);
                }
            }
            return listT;
        }

        /// <summary>
        /// 提取转换部分公用代码
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="type"></param>
        private static void SetPropertyValue<T>(DbDataReader dataReader,T instance)
        {
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                object dataValue = null;
                if (property.IsDefined(typeof(PropertyNameAttribute), true))
                {
                    dataValue = dataReader[((property.GetCustomAttributes(typeof(PropertyNameAttribute), true))[0] as PropertyNameAttribute).FieldName];
                }
                else
                {
                    dataValue = dataReader[property.Name];
                }

                property.SetValue(instance, dataValue);
            }
        }


        /// <summary>
        /// 获取数据库表的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>()
        {
            var tType = typeof(T);
            var isHaveAttribute = tType.IsDefined(typeof(TableNameAttribute), true);
            return !isHaveAttribute ? tType.Name : (tType.GetCustomAttributes(typeof(TableNameAttribute), true)[0] as TableNameAttribute).TableName;
        }
        #endregion

        #region 通过反射展示数据信息
        /// <summary>
        /// 输出数据信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        public static void ShowData<T>(T t) where T:BaseModel
        {
            if (t.Id == 0)
            {
                Console.WriteLine("未查询到任何信息!");
                return;
            }
            if (t.GetType().IsDefined(typeof(ChineseNameAttribute), true))
            {
                Console.WriteLine("****************" + ((t.GetType().GetCustomAttributes(typeof(ChineseNameAttribute), true)[0]) as ChineseNameAttribute).ChineseName + "****************");
            }
            else
            {
                Console.WriteLine("****************" + t.GetType().Name + "****************");
            }
            ShowDataCommon<T>(t);
        }

        /// <summary>
        /// 输出集合信息，重载方法对调用者而言 是同一个方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listT"></param>
        public static void ShowData<T>(List<T> listT)
        {
            if(listT.Count()<=0)
            {
                Console.WriteLine("未查询到任何信息!");
                return;
            }
            
            int iNumber = 0;
            foreach (var item in listT)
            {
                iNumber++;
                if (item.GetType().IsDefined(typeof(ChineseNameAttribute), true))
                {
                    Console.WriteLine("****************" + iNumber +". " + ((item.GetType().GetCustomAttributes(typeof(ChineseNameAttribute), true)[0]) as ChineseNameAttribute).ChineseName  + "****************");
                }
                else
                {
                    Console.WriteLine("****************" + iNumber + ". " + item.GetType().Name  + "****************");
                }
                ShowDataCommon<T>(item);
            }
        }

        /// <summary>
        /// 显示信息公用部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        private static void ShowDataCommon<T>(T t)
        {
            Type tType = t.GetType();
            string propertyChineseName = string.Empty;
            string propertyValue = string.Empty;
            foreach (var property in tType.GetProperties())
            {
                if (property.IsDefined(typeof(ChineseNameAttribute), true))
                {
                    propertyChineseName = (property.GetCustomAttributes(typeof(ChineseNameAttribute), true)[0] as ChineseNameAttribute).ChineseName + ":";

                }
                else
                {
                    propertyChineseName = property.Name.ToString() + ":";
                }
                propertyValue = property.GetValue(t).ToString();
                Console.WriteLine(propertyChineseName + propertyValue);
            }
        }
        #endregion

        #region 通过反射拼接SQL

        /// <summary>
        /// 更新sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SpliceUpdateSql<T>(T t) where T:BaseModel
        {
            var tType = t.GetType();
            string headerString =string.Format( "update [{0}] set ",ReflectionHelper.GetTableName<T>());
            List<string> updateString = new List<string>();//Set部分
            string whereString = "where Id=" + t.Id+" ";
            foreach (var item in tType.GetProperties())
            {
                if (item.Name == "Id") continue;
                string fieldName = string.Empty;//字段名称
                if(item.IsDefined(typeof(PropertyNameAttribute), true))
                {
                    fieldName = ((item.GetCustomAttributes(typeof(PropertyNameAttribute), true)[0]) as PropertyNameAttribute).FieldName;
                }
                else
                {
                    fieldName = item.Name;
                }
                if(item.PropertyType==typeof(String) || item.PropertyType == typeof(DateTime))
                {
                    updateString.Add(string.Format("[{0}] = '{1}'", fieldName, item.GetValue(t)));
                }
                //else if(item.PropertyType.IsEnum)
                //{
                    //找了很久用反射取枚举对应的ID，结果原来只要转下就OK了
                    //var value = (int)item.GetValue(t);

                    //var field = item.PropertyType.GetEnumValues();
                    //var obj = System.Enum.GetUnderlyingType(item.PropertyType).ToString();
                    //var test = System.Enum.Parse(item.PropertyType, value.ToString());
                    //for (int i = 0; i < field.Length - 1; i++)
                    //{

                    //}
                //}
                else
                {
                    updateString.Add(string.Format("[{0}] = {1}", fieldName, item.PropertyType.IsEnum ? (int)item.GetValue(t) : item.GetValue(t) ));
                }
            }
            string allUpdateString = headerString + string.Join(",", updateString) + whereString;
            return allUpdateString;
        }

        /// <summary>
        /// 删除sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SpliceDeleteSql<T>(T t) where T : BaseModel
        {
            string deleteString = string.Format("delete from [{0}] where Id={1}", ReflectionHelper.GetTableName<T>(), t.Id);
            return deleteString;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SpliceInsertSql<T>(T t) where T : BaseModel
        {
            var tType = t.GetType();
            string insertSection = string.Format("insert into [{0}] ",ReflectionHelper.GetTableName<T>());
            List<string> propertySection = new List<string>();
            List<string> valueSection = new List<string>();
            foreach (var item in tType.GetProperties())
            {
                if (item.Name == "Id") continue;
                if (item.IsDefined(typeof(PropertyNameAttribute), true))
                {
                    propertySection.Add("[" + ((item.GetCustomAttributes(typeof(PropertyNameAttribute), true)[0]) as PropertyNameAttribute).FieldName + "]");
                }
                else
                {
                    propertySection.Add("[" + item.Name + "]");
                }

                if (item.PropertyType == typeof(String) || item.PropertyType == typeof(DateTime))
                {
                    valueSection.Add(string.Format("'{0}'", item.GetValue(t)));
                }
                else
                {
                    valueSection.Add(item.PropertyType.IsEnum ? ((int)item.GetValue(t)).ToString() : item.GetValue(t).ToString());
                }
            }

            var allString = string.Format("{0} ({1}) values({2})",insertSection,string.Join(",",propertySection), string.Join(",", valueSection));
            return allString;
        }
        #endregion
    }
}
