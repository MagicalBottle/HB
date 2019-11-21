using HB.Data;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
using Microsoft.AspNetCore.Http;
using HB.Utility;
using System.Data;

namespace HB.Admin.Services
{
    public class MenuService : BaseService<SysMenu>, IMenuService
    {
        public MenuService(IHBDbContext dbContext)
            : base(dbContext)
        {

        }
        /// <summary>
        /// 根据角色id获取对应的菜单
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>菜单集合</returns>
        public List<SysMenu> GetMenusByRoleId(int roleId)
        {
            var param = new DynamicParameters();
            param.Add(name: "@RoleId", value: roleId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            string sql = "SELECT * FROM Sys_Menu m " +
                "LEFT JOIN Sys_MenuRole mr on m.Id=mr.MenuId" +
                "WHERE mr.RoleId=@RoleId";            

            return Connection.Query<SysMenu>(sql,param).ToList();
        }

        #region 后台菜单列表
        /// <summary>
        /// 组织好菜单数据
        /// </summary>
        /// <param name="menus">数据库查出的原始数据</param>
        /// <returns>组织好的菜单集合</returns>
        public List<SysMenu> FormData(List<SysMenu> inputMenus)
        {
            if (inputMenus == null || inputMenus.Count <= 0)
            {
                return inputMenus;
            }
            Dictionary<int, SysMenu> dicTemp = new Dictionary<int, SysMenu>();
            Dictionary<int, SysMenu> dicReturn = new Dictionary<int, SysMenu>();

            var menus = CopyHelper.CopyDeepByJson(inputMenus);
            //按pid排序才不影响下面的判断dic.ContainsKey(menu.ParentMenuId)
            foreach (var menu in menus.OrderBy(m => m.ParentMenuId))
            {
                if (menu.ParentMenuId == 0)
                {
                    menu.Deep = 0;
                    dicReturn.Add(menu.Id, menu);
                }
                else
                {
                    //如果当前的菜单的父级是刚才添加过的，那么关联上
                    if (dicTemp.ContainsKey(menu.ParentMenuId))
                    {
                        var tempParentMenu = dicTemp[menu.ParentMenuId];
                        menu.Deep = tempParentMenu.Deep + 1;
                        tempParentMenu.ChildrenMenus.Add(menu);
                        menu.ParentMenu = tempParentMenu;
                    }
                }
                dicTemp.Add(menu.Id, menu);
            }
            dicTemp = null;
            return dicReturn.Values.ToList();
        }

        /// <summary>
        /// 标记请求的链接为选中状态
        /// </summary>
        /// <param name="menus">必须是调用<see cref="FormData"/>方法处理后的</param>
        /// <param name="httpContext">请求上下文</param>
        public void ActiveMenu(List<SysMenu> menus, HttpContext httpContext)
        {
            if (httpContext == null || httpContext.Request == null || !httpContext.Request.Path.HasValue)
            {
                return;
            }
            string[] urls = httpContext.Request.Path.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);
            string control = string.Empty;//  /Home /Home/Index   /Menu /Menu/Index 
            if (urls.Length <= 0)
            {
                return;
            }
            if (urls.Length >= 1)
            {
                control = urls[0];
            }
            ActiveMenu(menus, control);
        }

        /// <summary>
        /// 标记请求的链接为选中状态
        /// </summary>
        /// <param name="menus">必须是调用<see cref="FormData"/>方法处理后的</param>
        /// <param name="control">路径，控制器的部分</param>
        private void ActiveMenu(List<SysMenu> menus, string control)
        {
            foreach (var menu in menus)
            {
                //  /Menu/Edit/2  数据库保存 /Menu/Index   取[0]做对比
                string[] urls = menu.MenuUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
                if (urls.Length >= 1)
                {
                    if (string.Equals(urls[0], control, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ActiveMenu(menu);
                    }
                }

                var childrenMenus = menu.ChildrenMenus;
                if (childrenMenus != null && childrenMenus.Count > 0)
                {
                    ActiveMenu(childrenMenus, control);
                }

            }
        }

        /// <summary>
        /// 标记当前菜单，以及他的祖先菜单
        /// </summary>
        /// <param name="menu"></param>
        private void ActiveMenu(SysMenu menu)
        {
            menu.Active = true;
            if (menu.ParentMenu != null)
            {
                ActiveMenu(menu.ParentMenu);
            }
        }

        #endregion



        /// <summary>
        /// 分页获取菜单
        /// </summary>
        /// <param name="pageNumber">页数（默认第1页）</param>
        /// <param name="pageSize">每页条数（默认10条）</param>
        /// <param name="menuMame">菜单名称（默认为空）</param>
        /// <param name="menuSystermName">菜单系统名称（默认为空）</param>
        /// <param name="sortName">排序的字段（默认为空）</param>
        /// <param name="sortOrder">排序方式 asc desc</param>
        /// <returns></returns>
        public List<SysMenu> GetMenus(int pageNumber = 1, int pageSize = 10, string menuName = null, string menuSystermName = null, string sortName = "Id", string sortOrder = "ASC")
        {
            //null 传播 https://github.com/StefH/System.Linq.Dynamic.Core/wiki/NullPropagation

            //var query = from m in _menuRepository.TableNoTracking
            //            select m;

            //if (!string.IsNullOrEmpty(menuName))
            //{
            //    //Contains 会包含 or name=''   indexof 不会  找不到-1
            //    //转换成mysql locate() 只要找到返回的结果都大于0
            //    query = query.Where(m => m.MenuName.IndexOf(menuName) > -1);
            //}

            //if (!string.IsNullOrEmpty(menuSystermName))
            //{
            //    query = query.Where(m => m.MenuSystermName.IndexOf(menuSystermName) > -1);
            //}

            //if (string.IsNullOrWhiteSpace(sortName))
            //{
            //    sortName = "Id";
            //}
            //if (sortOrder.ToUpper() == "ASC")
            //{
            //    query = query.OrderBy(sortName + " ASC");
            //}
            //if (sortOrder.ToUpper() == "DESC")
            //{
            //    query = query.OrderBy(sortName + " DESC");
            //}            
            //return new PagedList<SysMenu>(query, pageNumber, pageSize);

            //var param = new DynamicParameters();
            //param.Add(name: "@RoleId", value: roleId, dbType: DbType.Int32, direction: ParameterDirection.Input);


            string sql = "SELECT * FROM Sys_Menu m " +
                "LEFT JOIN Sys_MenuRole mr on m.Id=mr.MenuId" +
                "WHERE mr.RoleId=@RoleId";

            return Connection.Query<SysMenu>(sql).ToList();


        }


        /// <summary>
        /// 获取所有的菜单
        /// </summary>
        /// <returns></returns>
        public List<SysMenu> GetAllMenus()
        {
            //null 传播 https://github.com/StefH/System.Linq.Dynamic.Core/wiki/NullPropagation


            string sql = "SELECT * FROM Sys_Menu m ORDER BY m.MenuSort ASC";

            return Connection.Query<SysMenu>(sql).ToList();
        }


        /// <summary>
        /// 获取菜单下的所有子菜单（只获取一级）
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<SysMenu> GetLevelMenus(int parentId = 0)
        {
            var param = new DynamicParameters();
            param.Add(name: "@ParentMenuId", value: parentId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            string sql = "SELECT * FROM Sys_Menu m WHERE m.ParentMenuId=@ParentMenuId  ORDER BY m.MenuSort ASC";

            return Connection.Query<SysMenu>(sql,param).ToList();

        }

        /// <summary>
        /// 是否存在同名系统菜单
        /// </summary>
        /// <param name="menuSystermName"></param>
        /// <param name="excludeId">排除某个菜单，id</param>
        /// <returns></returns>
        public bool ExistMenuByMenuSystermName(string menuSystermName, int excludeId = 0)
        {
            bool isExist = true;

            var param = new DynamicParameters();
            param.Add(name: "@MenuSystermName", value: menuSystermName, dbType: DbType.String, direction: ParameterDirection.Input);

            //用户信息 
            string sql = "SELECT * FROM Sys_Menu m WHERE m.MenuSystermName=@MenuSystermName";


            if (excludeId > 0)
            {
                param.Add(name: "@Id", value: excludeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                sql += " AND Id !=@Id";
            }

            isExist = Any(sql, param);

            return isExist;


        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menu">菜单实体</param>
        /// <returns>-1插入失败</returns>
        public int AddMenu(SysMenu menu)
        {
            int result = -1;
            try
            {
                result = Insert(menu);
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="menuIds">菜单id</param>
        /// <returns></returns>
        public int DeleteMenu(int[] menuIds)
        {
            if (menuIds == null || menuIds.Length <= 0)
            {
                return 0;
            }
            int result = -1;
            result= BeginTransaction(() =>
            {
                var param = new DynamicParameters();
                param.Add(name: "@menuIds", value: menuIds, direction: ParameterDirection.Input);

                string sql = "DELETE FROM Sys_Menu  WHERE Id IN @menuIds;";
                Connection.Execute(sql,param);
                sql = "DELETE FROM Sys_MenuRole  WHERE MenuId IN @menuIds;";
                Connection.Execute(sql, param);
            });
            return result;
        }
        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysMenu GetMenu(int id)
        {
            if (id == 0)
            {
                return null;
            }
            var model = Get(id);

            return model;
        }

        /// <summary>
        /// 根据adminId获取其所有的菜单
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public List<SysMenu> GetMenusByAdminId(int adminId)
        {
            var param = new DynamicParameters();
            param.Add(name: "@AdminId", value: adminId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            //对应菜单
          string  sql = "SELECT m.* FROM Sys_Menu m " +
                " LEFT JOIN Sys_MenuRole mr ON m.Id=mr.MenuId" +
                " LEFT JOIN Sys_Role r ON r.Id=mr.RoleId" +
                " LEFT JOIN Sys_AdminRole ar ON r.Id=ar.RoleId" +
                " LEFT JOIN Sys_Admin a ON a.Id=ar.AdminId" +
                " Where a.Id=@AdminId";

            return Connection.Query<SysMenu>(sql,param).ToList();
        }

        /// <summary>
        ///  更新实体
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int UpdateMenu(SysMenu model)
        {
            int result = -1;

            var paramUpdate = new DynamicParameters();
            paramUpdate.Add(name: "@MenuName", value: model.MenuName, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@MenuSystermName", value: model.MenuSystermName, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@MenuIcon", value: model.MenuIcon, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@MenuUrl", value: model.MenuUrl, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@MenuRemark", value: model.MenuRemark, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@MenuSort", value: model.MenuSort, dbType: DbType.Int32, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@ParentMenuId", value: model.ParentMenuId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@Type", value: model.Type, dbType: DbType.Int32, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@LastUpdateBy", value: model.LastUpdateBy, dbType: DbType.Int32, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@LastUpdateByName", value: model.LastUpdateByName, dbType: DbType.String, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@LastUpdateDate", value: model.LastUpdateDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
            paramUpdate.Add(name: "@Id", value: model.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            string sqlUpdate = "UPDATE Sys_Menu SET MenuName=@MenuName,MenuSystermName=@MenuSystermName,MenuIcon=@MenuIcon,MenuUrl=@MenuUrl,MenuRemark=@MenuRemark," +
            "MenuSort=@MenuSort,ParentMenuId=@ParentMenuId,Type=@Type," +
            "LastUpdateBy=@LastUpdateBy,LastUpdateByName=@LastUpdateByName,LastUpdateDate=@LastUpdateDate WHERE Id=@Id";

            result = Connection.Execute(sqlUpdate, paramUpdate);
            return result;
        }


    }
}
