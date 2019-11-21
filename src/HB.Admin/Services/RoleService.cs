using Dapper;
using HB.Admin.Models;
using HB.Data;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;

namespace HB.Admin.Services
{

    public class RoleService : BaseService<SysRole>, IRoleService
    {
        public RoleService(IHBDbContext dbContext)
                    : base(dbContext)
        {

        }

        /// <summary>
        /// 获取所有的角色，按照id正序
        /// </summary>
        /// <returns></returns>
        public List<SysRole> GetAllRoles()
        {
            string sql = "SELECT * FROM Sys_Role ORDER BY Id ASC";
            return Connection.Query<SysRole>(sql).ToList();
        }



        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qParam">查询参数实体</param>
        /// <returns></returns>
        public PagedList<SysRole> GetRoles(RoleQueryParamInput qParam)
        {
            string sqlFiled = "SELECT * FROM Sys_Role ";
            string sqlWhere = " WHERE 1=1 ";
            string sqlSort = string.Empty;
            string sqlCount = string.Empty;
            string sqlLimit = string.Empty;
            var param = new DynamicParameters();

            #region sqlWhere
            if (!string.IsNullOrEmpty(qParam.RoleName))
            {
                param.Add(name: "@RoleName", value: string.Format("%{0}%", qParam.RoleName), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND RoleName LIKE @RoleName ";
            }

            if (!string.IsNullOrEmpty(qParam.RoleRemark))
            {
                param.Add(name: "@RoleRemark", value: string.Format("%{0}%", qParam.RoleRemark), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND RoleRemark LIKE @RoleRemark ";
            }

            if (qParam.RoleStatus > 0)
            {
                param.Add(name: "@Status", value: qParam.RoleStatus, dbType: DbType.Int32, direction: ParameterDirection.Input);
                sqlWhere += " AND Status=@Status";
            }

            #endregion

            sqlCount = "SELECT Count(*) FROM Sys_Role " + sqlWhere;
            int totalCount = Connection.ExecuteScalar<int>(sqlCount, param);

            #region sqlSort
            if (!string.IsNullOrWhiteSpace(qParam.SortName))
            {
                param.Add(name: "@SortName", value: qParam.SortName, dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " ORDER BY @SortName ";

                if (qParam.SortOrder.ToUpper() == "ASC")
                {
                    sqlWhere += " ASC ";
                }
                if (qParam.SortOrder.ToUpper() == "DESC")
                {
                    sqlWhere += " DESC ";
                }
            }
            #endregion

            sqlLimit = " LIMIT " + (qParam.PageNumber - 1) * qParam.PageSize + "," + qParam.PageSize;


            var models = Connection.Query<SysRole>(sqlFiled + sqlWhere + sqlSort + sqlLimit, param);

            return new PagedList<SysRole>(models, qParam.PageNumber, qParam.PageSize, totalCount);
        }



        /// <summary>
        /// 是否存在角色名称
        /// </summary>
        /// <param name="userName">角色名称</param>
        /// <param name="excludeId">排除那个Id的角色名称</param>
        /// <returns>true 存在，false 不存在</returns>
        public bool ExistRoleName(string roleName, int excludeId = 0)
        {
            bool isExist = true;
            
            var param = new DynamicParameters();
            param.Add(name: "@RoleName", value: roleName, dbType: DbType.String, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Sys_Role WHERE RoleName=@RoleName";


            if (excludeId > 0)
            {
                param.Add(name: "@Id", value: excludeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                sql += " AND Id !=@Id";
            }

            isExist = Any(sql, param);

            return isExist;

        }

        /// <summary>
        ///  新增
        /// </summary>
        /// <param name="admin">角色实体</param>
        /// <param name="roleIds">角色包含了账号id</param>
        /// <returns>-1,实体插入失败，-2角色关系插入失败</returns>
        public int AddRole(SysRole role, List<int> adminIds)
        {
            int result = -1;
            result = BeginTransaction(() =>
            {
                result = Insert(role);
                if (adminIds != null && adminIds.Count > 0)
                {
                    foreach (var id in adminIds)
                    {
                        var ar = new SysAdminRole()
                        {
                            AdminId = id,
                            RoleId = role.Id,
                            CreateBy = role.CreateBy,
                            CreatebyName = role.CreatebyName,
                            CreateDate = role.CreateDate,
                            LastUpdateBy = role.LastUpdateBy,
                            LastUpdateByName = role.LastUpdateByName,
                            LastUpdateDate = role.LastUpdateDate
                        };
                        result = (int)Connection.Insert(ar);
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// 根据Id获取实体,实体附带管理员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysRole GetRoleById(int id)
        {
            if (id == 0)
            {
                return null;
            } 
            var model = Connection.Get<SysRole>(id);

            if (model == null)
                return null;

            var param = new DynamicParameters();
            param.Add(name: "@Id", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            //有角色的管理员
            string sql = "SELECT a.* FROM Sys_Admin a " +
                " LEFT JOIN Sys_AdminRole ar ON a.Id=ar.AdminId" +
                " LEFT JOIN Sys_Role r ON r.Id=ar.RoleId" +
                " Where r.Id=@Id";

            var sysAdmins = Connection.Query<SysAdmin>(sql, param).ToList();
            model.Admins = sysAdmins;
            return model;
        }

        /// <summary>
        ///  更新一个角色
        /// </summary>
        /// <param name="role">角色实体</param>
        /// <param name="adminIds">包含的人员id</param>
        /// <returns></returns>
        public int UpdateRole(SysRole role, List<int> adminIds)
        {
            int result = -1;
            result = BeginTransaction(() =>
            {
                #region update
                var paramUpdate = new DynamicParameters();
                paramUpdate.Add(name: "@RoleName", value: role.RoleName, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@RoleRemark", value: role.RoleRemark, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@Status", value: role.Status, dbType: DbType.Int32, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateBy", value: role.LastUpdateBy, dbType: DbType.Int32, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateByName", value: role.LastUpdateByName, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateDate", value: role.LastUpdateDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@Id", value: role.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                string sqlUpdate = "UPDATE Sys_Role SET RoleName=@RoleName,RoleRemark=@RoleRemark,Status=@Status," +
                "LastUpdateBy=@LastUpdateBy,LastUpdateByName=@LastUpdateByName,LastUpdateDate=@LastUpdateDate WHERE Id=@Id";
                result = Connection.Execute(sqlUpdate, paramUpdate);
                #endregion

                #region delete
                string sqlDelete = "DELETE FROM Sys_AdminRole WHERE RoleId=@RoleId";

                var paramDelete = new DynamicParameters();
                paramDelete.Add(name: "@RoleId", value: role.Id, dbType: DbType.String, direction: ParameterDirection.Input);
                result = Connection.Execute(sqlDelete, paramDelete);

                #endregion

                #region insert
                if (adminIds != null && adminIds.Count > 0)
                {
                    foreach (var id in adminIds)
                    {
                        var ar = new SysAdminRole()
                        {
                            AdminId = id,
                            RoleId = role.Id,
                            LastUpdateBy = role.LastUpdateBy,
                            LastUpdateByName = role.LastUpdateByName,
                            LastUpdateDate = role.LastUpdateDate,
                            CreateBy = role.LastUpdateBy,
                            CreatebyName = role.LastUpdateByName,
                            CreateDate = role.LastUpdateDate
                        };
                        result = (int)Connection.Insert(ar);
                    }
                }
                #endregion

            });
            return result;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public int DeleteRoleWithAdminRole(int id)
        {
            //设置了级联删除，自动删除对应的表adminrole表的记录
            int result = -1;

            #region delete
            result = BeginTransaction(() =>
            {
                var paramDelete = new DynamicParameters();
                paramDelete.Add(name: "@RoleId", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                string sql = "DELETE FROM Sys_Role WHERE Id=@RoleId";

                result = Connection.Execute(sql, paramDelete);

                string sqlDelete = "DELETE FROM Sys_AdminRole WHERE RoleId=@RoleId";

                result = Connection.Execute(sqlDelete, paramDelete);

            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取角色，包含角色对应的菜单权限
        /// </summary>
        /// <param name="">角色id</param>
        /// <returns></returns>
        public SysRole GetRoleWithMenus(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            var param = new DynamicParameters();
            param.Add(name: "@RoleId", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Sys_Role WHERE id=@RoleId";

            var model = Connection.QueryFirstOrDefault<SysRole>(sql, param);

            if (model == null)
                return null;
            
            //对应菜单
            sql = "SELECT m.* FROM Sys_Menu m " +
                " LEFT JOIN Sys_MenuRole mr ON m.Id=mr.MenuId" +
                " LEFT JOIN Sys_Role r ON r.Id=mr.RoleId" +
                " Where r.id=@RoleId";

            var sysMenus = Connection.Query<SysMenu>(sql, param).ToList();
            model.Menus = sysMenus;

            return model;

        }



        /// <summary>
        ///  更新角色的权限
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="adminIds">包含的菜单id</param>
        /// <returns></returns>
        public int UpdatePermission(SysRole role, List<int> menuIds)
        {
            int result = -1;
            result = BeginTransaction(() =>
            {

                #region update
                //var paramUpdate = new DynamicParameters();
                //paramUpdate.Add(name: "@RoleName", value: role.RoleName, dbType: DbType.String, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@RoleRemark", value: role.RoleRemark, dbType: DbType.String, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@Status", value: role.Status, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@LastUpdateBy", value: role.LastUpdateBy, dbType: DbType.Int32, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@LastUpdateByName", value: role.LastUpdateByName, dbType: DbType.String, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@LastUpdateDate", value: role.LastUpdateDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                //paramUpdate.Add(name: "@Id", value: role.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                //string sqlUpdate = "UPDATE Sys_Admin SET RoleName=@RoleName,RoleRemark=@RoleRemark,Status=@Status," +
                //"LastUpdateBy=@LastUpdateBy,LastUpdateByName=@LastUpdateByName,LastUpdateDate=@LastUpdateDate WHERE Id=@Id";
                //result = Connection.Execute(sqlUpdate, paramUpdate);
                #endregion

                #region delete
                string sqlDelete = "DELETE FROM Sys_MenuRole WHERE RoleId=@RoleId";

                var paramDelete = new DynamicParameters();
                paramDelete.Add(name: "@RoleId", value: role.Id, dbType: DbType.String, direction: ParameterDirection.Input);
                result = Connection.Execute(sqlDelete, paramDelete);

                #endregion

                #region insert
                if (menuIds != null && menuIds.Count > 0)
                {
                    foreach (var id in menuIds)
                    {
                        var ar = new SysMenuRole()
                        {
                            MenuId = id,
                            RoleId = role.Id,
                            LastUpdateBy = role.LastUpdateBy,
                            LastUpdateByName = role.LastUpdateByName,
                            LastUpdateDate = role.LastUpdateDate,
                            CreateBy = role.LastUpdateBy,
                            CreatebyName = role.LastUpdateByName,
                            CreateDate = role.LastUpdateDate
                        };
                        result = (int)Connection.Insert(ar);
                    }
                }
                #endregion

            });
            return result;
        }

    }
}
