using Model.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 公司
    /// </summary>
    [ChineseName(ChineseName ="公司基本信息")]
    public class Company:BaseModel
    {
        public Company()
        {
            CreateTime = DateTime.Now;
        }
        [ChineseName(ChineseName = "名称")]
        [RequirdValidate]
        public string Name { get; set; }

        [ChineseName(ChineseName = "创建时间")]
        public DateTime CreateTime { get; set; }

        [ChineseName(ChineseName = "创建人")]
        public int CreatorId { get; set; }

        [ChineseName(ChineseName = "最后修改人")]
        public int LastModifierId { get; set; }

        [ChineseName(ChineseName = "修改时间")]
        public DateTime LastModifyTime { get; set; }
    }
}
