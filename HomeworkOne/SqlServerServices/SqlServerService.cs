using Model.CustomAttribute;
using Common.ReflectionHelper;
using IService;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerServices
{
    /// <summary>
    /// 原来一直都没用过ADO.NET，临时抱佛脚，用到什么去查什么
    /// </summary>
    public class SqlServerService : IDataServices
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["defaultDataSource"].ToString();

        #region 查询操作

        /// <summary>
        /// 根据ID查询某一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Quary<T>(int id) where T : BaseModel
        {
            string sql = string.Format("select * from [{0}] where Id={1}", ReflectionHelper.GetTableName<T>(),id);//$"select * from [t.GetType().Name]";
            return ReflectionHelper.ConvertDataReaderToInstance<T>(QuaryCommon(sql));
        }

        /// <summary>
        /// 查询某个表所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> QuaryList<T>() where T : BaseModel,new()
        {
            string sql = string.Format("select * from [{0}]", ReflectionHelper.GetTableName<T>());
            return ReflectionHelper.ConvertDataReaderToListInstance<T>(QuaryCommon(sql));
        }

      
        /// <summary>
        /// 提取查询部分公用代码
        /// </summary>
        /// <returns>返回dataReader</returns>
        private SqlDataReader QuaryCommon(string sql)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            sqlConnection.Open();
            return sqlCommand.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }
        #endregion

        #region 新增更新删除操作

        public bool Update<T>(T t) where T : BaseModel
        {
            string sql = ReflectionHelper.SpliceUpdateSql<T>(t);
            return UpdateInsertDeleteCommon(sql);
        }

        public bool Delete<T>(T t) where T : BaseModel
        {
            string sql = ReflectionHelper.SpliceDeleteSql<T>(t);
            return UpdateInsertDeleteCommon(sql);
        }

        public bool Insert<T>(T t) where T : BaseModel
        {
            string sql = ReflectionHelper.SpliceInsertSql<T>(t);
            return UpdateInsertDeleteCommon(sql);
        }

        private bool UpdateInsertDeleteCommon(string sql)
        {
            var isSuccess = false;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                isSuccess = sqlCommand.ExecuteNonQuery() > 0;
            }
            return isSuccess;
        }

        #endregion
    }
}
