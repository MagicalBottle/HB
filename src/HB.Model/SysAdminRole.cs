using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HB.Models
{
    /// <summary>
    /// 用户角色关系映射
    /// </summary>
    [Table("Sys_AdminRole")]
    public partial class SysAdminRole : EditorEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int AdminId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        [Write(false)]
        public virtual SysAdmin SysAdmin { get; set; }

        [Write(false)]
        public virtual SysRole SysRole { get; set; }
    }
}
