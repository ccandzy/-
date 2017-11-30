using Model.CustomAttribute;
using Model.ModelEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 用户
    /// </summary>
    [TableName("User")]
    [ChineseName(ChineseName = "用户基本信息")]
    public class UserInfo:BaseModel
    {
        public UserInfo()
        {
            Status = (int)StateTypes.Noraml;
        }
        [ChineseName(ChineseName ="名称")]
        [RequirdValidateAttribute]
        public string Name { get; set; }

        [ChineseName(ChineseName = "帐号")]
        public string Account { get; set; }

        [ChineseName(ChineseName = "密码")]
        public string Password { get; set; }

        [EmailValidate]
        [ChineseName(ChineseName = "邮箱")]
        public string Email { get; set; }

        [MobileValidate]
        [ChineseName(ChineseName = "手机")]
        public string Mobile { get; set; }

        [ChineseName(ChineseName = "公司Id")]
        public int CompanyId { get; set; }

        [ChineseName(ChineseName = "公司名称")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 用户状态  0正常 1冻结 2删除
        /// </summary>
        [PropertyName("state")]
        [ChineseName(ChineseName = "状态")]
        public StateTypes Status { get; set; }

        /// <summary>
        /// 用户类型  1 普通用户 2管理员 4超级管理员
        /// </summary>
        [ChineseName(ChineseName = "类型")]
        public UserTypes UserType { get; set; }

        [ChineseName(ChineseName = "最后登录时间")]
        public DateTime LastLoginTime { get; set; }

        [ChineseName(ChineseName = "创建时间")]
        public DateTime CreateTime { get; set; }

        [ChineseName(ChineseName = "创建人")]
        public int CreatorId { get; set; }

        [ChineseName(ChineseName = "最后修改人")]
        public int LastModifierId { get; set; }

        [ChineseName(ChineseName = "最后修改时间")]
        public DateTime LastModifyTime { get; set; }
    }
}
