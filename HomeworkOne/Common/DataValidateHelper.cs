using Common.ReflectionHelper;
using Model.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DataValidateHelper
    {
        public static DataValidateResult Validate<T>(T t)
        {
            Type tType = typeof(T);
            DataValidateResult dataValidate = new DataValidateResult { IsSuccess = true,ResultString="OK" };
            foreach (var item in tType.GetProperties())
            {
                if (item.IsDefined(typeof(BaseValidateAttribute), true))
                {
                    var attribute = (item.GetCustomAttributes(typeof(BaseValidateAttribute), true)[0]) as BaseValidateAttribute;
                    if (!attribute.Validate(item.GetValue(t)))
                    {
                        dataValidate.IsSuccess = false;
                        if (attribute.GetType().IsDefined(typeof(ErrorStringAttribute), true))
                        {
                            dataValidate.ResultString = ((attribute.GetType().GetCustomAttributes(typeof(ErrorStringAttribute), true)[0]) as ErrorStringAttribute).ErrorMessage;
                        }
                        else
                        {
                            dataValidate.ResultString = "数据输入不合法";
                        }
                        break;
                    }
                }
            }
            return dataValidate;
        }
    }
}
