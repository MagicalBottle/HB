using Dapper.Contrib.Extensions;
using HB.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HB.Services
{

    public abstract class BaseService<T> : IBaseService<T> where T : class, new()
    {
        public BaseService(IHBDbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public IHBDbContext DbContext { get; }

        /// <summary>
        /// 数据库连接,操作sql用这个
        /// </summary>
        protected IDbConnection Connection
        {
            get { return DbContext.Connection; }
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>返回实体</returns>
        public T Get(int id)
        {
            return Connection.Get<T>(id);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <returns>返回实体</returns>
        public Task<T> GetAsync(int id)
        {
            return Connection.GetAsync<T>(id);
        }

        /// <summary>
        /// 获取实体所有数据
        /// </summary>
        /// <returns>返回实体所有数据</returns>
        public IEnumerable<T> GetAll()
        {
            return Connection.GetAll<T>();
        }

        /// <summary>
        /// 获取实体所有数据
        /// </summary>
        /// <returns>返回实体所有数据</returns>
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Connection.GetAllAsync<T>();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        public bool Delete(T entityToDelete)
        {
            return Connection.Delete(entityToDelete);
        }


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entityToDelete">要删除的实体</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(T entityToDelete)
        {
            return Connection.DeleteAsync(entityToDelete);
        }

        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            return Connection.DeleteAll<T>();
        }


        /// <summary>
        /// 清空表
        /// </summary>
        /// <returns></returns>
        public Task<bool> DeleteAllAsync()
        {
            return Connection.DeleteAllAsync<T>();
        }

        /// <summary>
        /// 插入实体 
        /// </summary>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        public long Insert(T entityToInsert)
        {
            return Connection.Insert<T>(entityToInsert);
        }


        /// <summary>
        /// 插入实体 
        /// </summary>
        /// <param name="entityToInsert">要插入的实体</param>
        /// <returns></returns>
        public Task<int> InsertlAsync(T entityToInsert)
        {
            return Connection.InsertAsync<T>(entityToInsert);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        /// <returns></returns>
        public bool Update(T entityToUpdate)
        {
            return Connection.Update<T>(entityToUpdate);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entityToUpdate">要更新的实体</param>
        /// <returns></returns>
        public Task<bool> UpdateAsync(T entityToUpdate)
        {
            return Connection.UpdateAsync<T>(entityToUpdate);
        }

        /// <summary>
        /// 开启事务执行
        /// </summary>
        /// <param name="action"></param>
        /// <returns>result 大于0成功</returns>
        public int BeginTransaction(Action action)
        {
            IDbTransaction transaction = null;
            int result = -1;
            try
            {
                Connection.Open();
                transaction = Connection.BeginTransaction();
                action.Invoke();
                transaction.Commit();
                result = 1;
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                Connection.Close();
            }
            return result;
        }

    }
}
