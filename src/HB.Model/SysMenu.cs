using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;

namespace HB.Models
{

    /// <summary>
    /// 菜单类
    /// </summary>
    [Serializable]
    [Table("Sys_Menu")]
    public partial class SysMenu : EditorEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 菜单显示名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单系统名称
        /// </summary>
        public string MenuSystermName { get; set; }

        /// <summary>
        /// 菜单链接
        /// </summary>
        public string MenuUrl { get; set; }

        /// <summary>
        /// 上级菜单ID
        /// </summary>
        public int ParentMenuId { get; set; }

        /// <summary>
        /// 菜单类型 1，链接；2功能。操作的时候看<see cref="MenuType"/>
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 菜单类型 1，链接；2功能
        /// </summary>
        [Write(false)]
        public MenuType MenuType
        {
            get { return (MenuType)Type; }
            set { Type = (int)value; }
        }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        public int MenuSort { get; set; }

        /// <summary>
        /// 菜单说明
        /// </summary>
        public string MenuRemark { get; set; }

        /// <summary>
        /// 菜单角色关联表
        /// </summary>
        [Write(false)]
        public virtual List<SysMenuRole> MenuRoles { get; set; }

        #region ignore
        /// <summary>
        /// 子菜单
        /// </summary>
        [Write(false)]
        public List<SysMenu> ChildrenMenus { get; set; } = new List<SysMenu>();

        /// <summary>
        /// 父菜单
        /// </summary>
        [Write(false)]
        public SysMenu ParentMenu { get; set; }

        /// <summary>
        /// 深度，默认0
        /// </summary>
        [Write(false)]
        public int Deep { get; set; }

        /// <summary>
        /// 选中菜单。true 选中，false 不选中。默认 false
        /// </summary>
        [Write(false)]
        public bool Active { get; set; } = false;
        #endregion

    }
}
