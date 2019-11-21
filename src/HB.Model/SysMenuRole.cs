using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;

namespace HB.Models
{
    [Table("Sys_MenuRole")]
    public class SysMenuRole : EditorEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 功能权限ID
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单
        /// </summary>
        [Write(false)]
        public virtual SysMenu SysMenu { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Write(false)]
        public virtual SysRole SysRole { get; set; }
    }
}
