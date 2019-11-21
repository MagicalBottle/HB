using HB.Admin.Models;
using HB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HB.Admin.Services
{

    public interface ISysAdminService : IBaseService<SysAdmin>
    {

        /// <summary>
        ///  更新一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns></returns>
        int UpdateAdmin(SysAdmin admin, List<int> roleIds);

        /// <summary>
        /// 根据用户名获取管理员账号信息，包含角色、菜单
        /// </summary>
        /// <param name="userName">登录名</param>
        /// <returns></returns>
        SysAdmin GetAdminAllInfoByUserName(string userName);


        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="qParam">查询参数实体</param>
        /// <returns></returns>
        PagedList<SysAdmin> GetAdmins(AdminQueryParamInput qParam);

        /// <summary>
        /// 是否存在账号名称
        /// </summary>
        /// <param name="userName">账号名称</param>
        /// <param name="excludeId">排除那个Id的名称</param>
        /// <returns>true 存在，false 不存在</returns>
        bool ExistAdminUserName(string userName, int excludeId = 0);

        /// <summary>
        ///  新增一个账号
        /// </summary>
        /// <param name="admin">账号实体</param>
        /// <param name="roleIds">账号分配了的角色id</param>
        /// <returns>-1,实体插入失败，-2角色关系插入失败</returns>
        int AddAdmin(SysAdmin admin, List<int> roleIds);

        /// <summary>
        /// 删除管理员以及对和角色的关联关系
        /// </summary>
        /// <param name="id">管理员id</param>
        /// <returns></returns>
        int DeleteAdmin(int id);

        /// <summary>
        /// 根据用户名获取管理员账号信息，包含角色
        /// </summary>
        /// <param name="id">管理员id</param>
        /// <returns></returns>
        SysAdmin GetAdminWithRoles(int id);
    }
}
