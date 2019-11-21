using Dapper;
using HB.Admin.Models;
using HB.Data;
using HB.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace HB.Admin.Services
{

    public class SysAdminService : BaseService<SysAdmin>, ISysAdminService
    {
        public SysAdminService(IHBDbContext dbContext)
            : base(dbContext)
        {

        }
        /// <summary>
        /// 根据用户名获取管理员账号信息，包含角色、菜单
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        public SysAdmin GetAdminAllInfoByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var param = new DynamicParameters();
            param.Add(name: "@UserName", value: userName, dbType: DbType.String, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Sys_Admin WHERE UserName=@UserName";

            var sysAdmin = Connection.QueryFirstOrDefault<SysAdmin>(sql, param);

            if (sysAdmin == null)
                return null;

            //对应角色
            sql = "SELECT r.* FROM Sys_Role r " +
                " LEFT JOIN Sys_AdminRole ar ON r.Id=ar.RoleId " +
                " LEFT JOIN Sys_Admin a ON a.Id=ar.AdminId " +
                " Where a.UserName=@UserName";
            var sysRoles = Connection.Query<SysRole>(sql, param).ToList();
            sysAdmin.Roles = sysRoles;

            //对应菜单
            sql = "SELECT m.* FROM Sys_Menu m " +
                " LEFT JOIN Sys_MenuRole mr ON m.Id=mr.MenuId" +
                " LEFT JOIN Sys_Role r ON r.Id=mr.RoleId" +
                " LEFT JOIN Sys_AdminRole ar ON r.Id=ar.RoleId" +
                " LEFT JOIN Sys_Admin a ON a.Id=ar.AdminId" +
                " Where a.UserName=@UserName";

            var sysMenus = Connection.Query<SysMenu>(sql, param).ToList();
            sysAdmin.Menus = sysMenus;

            return sysAdmin;
        }


        /// <summary>
        /// 根据用户名获取管理员账号信息，包含角色
        /// </summary>
        /// <param name="id">管理员id</param>
        /// <returns></returns>
        public SysAdmin GetAdminWithRoles(int id)
        {
            if (id<=0)
                return null;

            var param = new DynamicParameters();
            param.Add(name: "@Id", value: id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Sys_Admin WHERE Id=@Id";

            var sysAdmin = Connection.QueryFirstOrDefault<SysAdmin>(sql, param);

            if (sysAdmin == null)
                return null;

            //对应角色
            sql = "SELECT r.* FROM Sys_Role r " +
                " LEFT JOIN Sys_AdminRole ar ON r.Id=ar.RoleId " +
                " LEFT JOIN Sys_Admin a ON a.Id=ar.AdminId " +
                " Where a.Id=@Id";
            var sysRoles = Connection.Query<SysRole>(sql, param).ToList();
            sysAdmin.Roles = sysRoles;

            return sysAdmin;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qParam">查询参数实体</param>
        /// <returns></returns>
        public PagedList<SysAdmin> GetAdmins(AdminQueryParamInput qParam)
        {

            //用户信息
            string sqlFiled = "SELECT * FROM Sys_Admin ";
            string sqlWhere = " WHERE 1=1 ";
            string sqlSort = string.Empty;
            string sqlCount = string.Empty;
            string sqlLimit = string.Empty;

            #region sqlWhere
            var param = new DynamicParameters();
            if (!string.IsNullOrEmpty(qParam.UserName))
            {
                param.Add(name: "@UserName", value: string.Format("%{0}%", qParam.UserName), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND UserName LIKE @UserName ";
            }

            if (!string.IsNullOrEmpty(qParam.NickName))
            {
                param.Add(name: "@NickName", value: string.Format("%{0}%", qParam.NickName), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND NickName LIKE @NickName ";
            }

            if (!string.IsNullOrEmpty(qParam.Email))
            {
                param.Add(name: "@Email", value: string.Format("%{0}%", qParam.Email), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND Email LIKE @Email ";
            }

            if (!string.IsNullOrEmpty(qParam.MobilePhone))
            {
                param.Add(name: "@MobilePhone", value: string.Format("%{0}%", qParam.MobilePhone), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND MobilePhone LIKE @MobilePhone ";
            }

            if (!string.IsNullOrEmpty(qParam.QQ))
            {
                param.Add(name: "@QQ", value: string.Format("%{0}%", qParam.QQ), dbType: DbType.String, direction: ParameterDirection.Input);
                sqlWhere += " AND QQ LIKE @QQ' ";
            }
            #endregion

            sqlCount = "SELECT Count(*) FROM Sys_Admin " + sqlWhere;
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


            var admins = Connection.Query<SysAdmin>(sqlFiled + sqlWhere + sqlSort + sqlLimit, param);

            return new PagedList<SysAdmin>(admins, qParam.PageNumber, qParam.PageSize, totalCount);
        }


        /// <summary>
        /// 是否存在账号名称
        /// </summary>
        /// <param name="userName">账号名称</param>
        /// <param name="excludeId">排除那个Id的名称</param>
        /// <returns>true 存在，false 不存在</returns>
        public bool ExistAdminUserName(string userName, int excludeId = 0)
        {
            bool isExist = true;


            var param = new DynamicParameters();
            param.Add(name: "@UserName", value: userName, dbType: DbType.String, direction: ParameterDirection.Input);

            //用户信息
            string sql = "SELECT * FROM Sys_Admin WHERE UserName=@UserName";


            if (excludeId > 0)
            {
                param.Add(name: "@Id", value: excludeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
                sql += " AND Id !=@Id";
            }

            isExist = Any(sql, param);

            return isExist;

        }



        /// <summary>
        ///  新增一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns>-1,实体插入失败，-2角色关系插入失败</returns>
        public int AddAdmin(SysAdmin admin, List<int> roleIds)
        {
            int result = -1;

            result = BeginTransaction(() =>
            {
                result = Insert(admin);
                if (roleIds != null && roleIds.Count > 0)
                {
                    foreach (var id in roleIds)
                    {
                        var ar = new SysAdminRole()
                        {
                            AdminId = admin.Id,
                            RoleId = id,
                            CreateBy = admin.CreateBy,
                            CreatebyName = admin.CreatebyName,
                            CreateDate = admin.CreateDate,
                            LastUpdateBy = admin.LastUpdateBy,
                            LastUpdateByName = admin.LastUpdateByName,
                            LastUpdateDate = admin.LastUpdateDate
                        };

                        result = (int)Connection.Insert(ar);
                    }
                }
            });
            return result;
        }

        /// <summary>
        ///  更新一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns></returns>
        public int UpdateAdmin(SysAdmin admin, List<int> roleIds)
        {
            int result = -1;
            result = BeginTransaction(() =>
            {
                #region update
                var paramUpdate = new DynamicParameters();
                paramUpdate.Add(name: "@UserName", value: admin.UserName, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@NickName", value: admin.NickName, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@Password", value: admin.Password, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@Email", value: admin.Email, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@MobilePhone", value: admin.MobilePhone, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@QQ", value: admin.QQ, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@WeChar", value: admin.WeChar, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateBy", value: admin.LastUpdateBy, dbType: DbType.Int32, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateByName", value: admin.LastUpdateByName, dbType: DbType.String, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@LastUpdateDate", value: admin.LastUpdateDate, dbType: DbType.DateTime, direction: ParameterDirection.Input);
                paramUpdate.Add(name: "@Id", value: admin.Id, dbType: DbType.Int32, direction: ParameterDirection.Input);

                string sqlUpdate = "UPDATE Sys_Admin SET UserName=@UserName,NickName=@NickName,Password=@Password,Email=@Email,MobilePhone=@MobilePhone," +
                "QQ=@QQ,WeChar=@WeChar,LastUpdateBy=@LastUpdateBy,LastUpdateByName=@LastUpdateByName,LastUpdateDate=@LastUpdateDate WHERE Id=@Id";
                result = Connection.Execute(sqlUpdate, paramUpdate);
                #endregion

                #region delete
                string sqlDelete = "DELETE FROM Sys_AdminRole WHERE AdminId=@AdminId";

                var paramDelete = new DynamicParameters();
                paramDelete.Add(name: "@AdminId", value: admin.Id, dbType: DbType.String, direction: ParameterDirection.Input);
                result = Connection.Execute(sqlDelete, paramDelete);

                #endregion

                #region insert
                if (roleIds != null && roleIds.Count > 0)
                {
                    foreach (var id in roleIds)
                    {
                        var ar = new SysAdminRole()
                        {
                            AdminId = admin.Id,
                            RoleId = id,
                            LastUpdateBy = admin.LastUpdateBy,
                            LastUpdateByName = admin.LastUpdateByName,
                            LastUpdateDate = admin.LastUpdateDate,
                            CreateBy = admin.LastUpdateBy,
                            CreatebyName = admin.LastUpdateByName,
                            CreateDate = admin.LastUpdateDate
                        };
                        result = (int)Connection.Insert(ar);
                    }
                }
                #endregion

            });
            return result;
        }

        /// <summary>
        /// 删除管理员以及对和角色的关联关系
        /// </summary>
        /// <param name="id">管理员id</param>
        /// <returns></returns>
        public int DeleteAdmin(int id)
        {

            int result = -1;
            result = BeginTransaction(() =>
            {
                SysAdmin sysAdmin = new SysAdmin() { Id = id };
                Connection.Delete<SysAdmin>(sysAdmin);
                
                string sqlDelete = "DELETE FROM Sys_AdminRole WHERE AdminId=@AdminId";

                var paramDelete = new DynamicParameters();
                paramDelete.Add(name: "@AdminId", value: id, dbType: DbType.String, direction: ParameterDirection.Input);
                result = Connection.Execute(sqlDelete, paramDelete);
                

            });
            return result;
        }
    }
}
