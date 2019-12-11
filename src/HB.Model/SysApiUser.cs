
using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.IO;
using System.Linq;
using System.Text;


namespace HB.Models
{

    /// <summary>
    /// API接口用户信息
    /// </summary>
    [Table("SYS_APIUser")]
    public class SysApiUser : EditorEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 调用者身份ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 调用者私钥
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 调用者IP地址，多个用‘,’分隔
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
