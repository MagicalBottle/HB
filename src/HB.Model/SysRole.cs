using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;

namespace HB.Models
{

    [Table("Sys_Role")]
    public partial class SysRole : EditorEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色状态,操作的时候用<see cref="RoleStatus"/>
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 角色状态
        /// </summary>
        [Write(false)]
        public RoleStatus RoleStatus
        {
            get { return (RoleStatus)Status; }
            set { Status = (int)value; }
        }

        /// <summary>
        /// 角色说明
        /// </summary>
        public string RoleRemark { get; set; }

        /// <summary>
        /// 用户角色关联表
        /// </summary>
        [Write(false)]
        public virtual List<SysAdminRole> AdminRoles { get; set; } = new List<SysAdminRole>();

        /// <summary>
        /// 菜单角色关联表
        /// </summary>
        [Write(false)]
        public virtual List<SysMenuRole> MenuRoles { get; set; } = new List<SysMenuRole>();

        /// <summary>
        /// 包含的管理员
        /// </summary>
        [Write(false)]
        public List<SysAdmin> Admins { get; set; } = new List<SysAdmin>();

        /// <summary>
        /// 包含的菜单权限
        /// </summary>
        [Write(false)]
        public List<SysMenu> Menus { get; set; } = new List<SysMenu>();

    }
}
