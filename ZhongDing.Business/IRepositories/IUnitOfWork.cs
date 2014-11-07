using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    /// <summary>
    /// 接口：IUnitOfWork
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 获取当前DB Model Container
        /// </summary>
        /// <returns>DbModelContainer.</returns>
        DbModelContainer GetDbModel();

        /// <summary>
        /// 设置DB Model Container
        /// </summary>
        /// <param name="dc">The dc.</param>
        void SetDbModel(DbModelContainer dc);

        /// <summary>
        /// 提交.
        /// </summary>
        void SaveChanges();
    }
}
