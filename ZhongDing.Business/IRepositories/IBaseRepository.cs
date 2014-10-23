using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntityExtendedProperty
    {

        /// <summary>
        /// News the entity.
        /// </summary>
        /// <returns>`0.</returns>
        TEntity NewEntity();

        /// <summary>
        /// Sets the db model.
        /// </summary>
        /// <param name="dc">The dc.</param>
        void SetDbModel(DbModelContainer dc);

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>`0.</returns>
        TEntity GetByID(object id);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>List{`0}.</returns>
        List<TEntity> GetList();

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> whereFunc, string orderBy = null);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy = null);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, string orderBy);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, out int total);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, string orderBy);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy);

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy, out int total);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, out int total);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, out int total);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, string orderBy, out int total);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, string orderBy, out int total);
        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetList(int pageIndex, int pageSize, string where, string orderBy, out int total);
        /// <summary>
        /// Gets the list for rotator.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where FUNCS.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        IQueryable<TEntity> GetListForRotator(int itemIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy);
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>`0.</returns>
        TEntity Add(TEntity entity);
        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);
        /// <summary>
        /// Deletes the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        void DeleteByID(object id);
    }
}
