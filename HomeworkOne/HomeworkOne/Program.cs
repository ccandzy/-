using Common;
using Common.ReflectionHelper;
using Factory;
using IService;
using Model;
using Model.ModelEx;
using SqlServerServices;
using System;

namespace HomeworkOne
{
    /// <summary>
    /// Common:封装一些公用的反射方法和验证方法
    /// Factory:简单工厂用来创建Service
    /// HomeworkOne:启动程序
    /// IService:数据层抽象
    /// Model:底层实体 及 特性
    /// SqlServerServices:具体的数据层实现
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //底层不catch异常，由上层捕获异常
            try
            {
                //通过配置文件+反射 创建Service
                IDataServices dataServices = SimpleFactory.CreateService();
                #region 查看及更新操作 带数据验证 并输出验证结果

                //查询单条数据
                var userInfo = dataServices.Quary<UserInfo>(1);
                ReflectionHelper.ShowData(userInfo);

                //查询多条数据
                var companies = dataServices.QuaryList<Company>();
                ReflectionHelper.ShowData(companies);

                //*************更新UserInfo****************
                userInfo.Name = "小新1";
                userInfo.Email = "1234567@qq.com";
                userInfo.Mobile = "15312122365";
                userInfo.UserType = UserTypes.Admin;
                //验证数据是否合法
                var resultForUserInfoUpdate = DataValidateHelper.Validate(userInfo);
                if (!resultForUserInfoUpdate.IsSuccess)
                {
                    //输出不合法原因，通过Attribute获取错误信息
                    Console.WriteLine(resultForUserInfoUpdate.ResultString);
                    Console.ReadKey();
                }
                else
                {
                    var isSuccess = dataServices.Update(userInfo);

                    if (isSuccess)
                    {
                        var userInfoUpdate = dataServices.Quary<UserInfo>(1);
                        ReflectionHelper.ShowData(userInfoUpdate);
                    }
                }

                //*************更新Company****************
                var company = dataServices.Quary<Company>(2);
                company.Name = "东莞1";
                var resultForCompanyUpdate = DataValidateHelper.Validate(company);
                if (!resultForCompanyUpdate.IsSuccess)
                {
                    //输出不合法原因，通过Attribute获取错误信息
                    Console.WriteLine(resultForCompanyUpdate.ResultString);
                    Console.ReadKey();
                }
                else
                {
                    var isSuccessForCompany = dataServices.Update(company);
                    if (isSuccessForCompany)
                    {
                        var companyUpdate = dataServices.Quary<Company>(2);
                        ReflectionHelper.ShowData(companyUpdate);
                    }
                }
                #endregion
                #region 新增操作
                UserInfo userInfoForInsert = new UserInfo
                {
                    Name = "cc",
                    Account = "413423828",
                    Password = "123",
                    Email = "413423828@qq.com",
                    Mobile = "1536666888",
                    CompanyId = 2,
                    CompanyName = "东莞",
                    Status = StateTypes.Noraml,
                    UserType = UserTypes.SuperAdmin,
                    LastLoginTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastModifierId = 2,
                    LastModifyTime = DateTime.Now
                };
                var resultForUserInfoInsert = DataValidateHelper.Validate(userInfoForInsert);
                if (!resultForUserInfoInsert.IsSuccess)
                {
                    Console.WriteLine(resultForUserInfoInsert.ResultString);
                    Console.ReadKey();
                }
                else
                {
                    var insertSuccess = dataServices.Insert(userInfoForInsert);
                    if (insertSuccess) Console.WriteLine("新增用户成功");
                }


                Company companyForInsert = new Company
                {
                    //Name = "cc的新公司",
                    CreateTime = DateTime.Now,
                    CreatorId = 1,
                    LastModifierId = 2,
                    LastModifyTime = DateTime.Now
                };
                var resultForCompanyInsert = DataValidateHelper.Validate(companyForInsert);
                if (!resultForCompanyInsert.IsSuccess)
                {
                    Console.WriteLine(resultForCompanyInsert.ResultString);
                    Console.ReadKey();
                }
                else
                {
                    var insertSuccessForCompany = dataServices.Insert(companyForInsert);
                    if (insertSuccessForCompany) Console.WriteLine("新增公司成功");
                }
                #endregion
                #region 删除操作

                var deleteUserInfo = dataServices.Quary<UserInfo>(2);
                var deleteSuccessForUserInfo = dataServices.Delete(deleteUserInfo);
                if (deleteSuccessForUserInfo) Console.WriteLine("删除用户成功");

                var deleteCompany = dataServices.Quary<Company>(4);
                var deleteSuccessForCompany = dataServices.Delete(deleteCompany);
                if (deleteSuccessForCompany) Console.WriteLine("删除公司成功");

                    #endregion
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
