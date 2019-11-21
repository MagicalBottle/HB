using HB.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services
{

    public interface IBaseService<T> where T : class, new()
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        IHBDbContext DbContext { get; }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>返回实体</returns>
        T Get(int id);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>返回实体</returns>
        Task<T> GetAsync(int id);

        /// <summary>
        /// 获取实体所有数据
        /// </summary>
        /// <returns>返回实体所有数据</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// 获取实体所有数据
        /// </summary>
        /// <returns>返回实体所有数据</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        bool Delete(T entityToDelete);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entityToDelete);

        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        bool DeleteAll();

        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAllAsync();

        /// <summary>
        /// 插入实体 
        /// </summary>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        long Insert(T entityToInsert);


        /// <summary>
        /// 插入实体 
        /// </summary>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        Task<int> InsertlAsync(T entityToInsert);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        /// <returns></returns>
        bool Update(T entityToUpdate);

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entityToUpdate);

        /// <summary>
        /// 开启事务执行
        /// </summary>
        /// <param name="action"></param>
        /// <returns>result 大于0成功</returns>
        int BeginTransaction(Action action);
    }
}
