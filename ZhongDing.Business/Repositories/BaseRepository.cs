using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories
{
    public class BaseRepository<TEntity> where TEntity : class, IEntityExtendedProperty, new()
    {
        #region Consts
        private const string ENTITY_PROPERTY_CREATEDON = "CreatedOn";
        private const string ENTITY_PROPERTY_CREATEDBY = "CreatedBy";
        private const string ENTITY_PROPERTY_LASTMODIFIEDON = "LastModifiedOn";
        private const string ENTITY_PROPERTY_LASTMODIFIEDBY = "LastModifiedBy";
        private const string ENTITY_PROPERTY_ISDELETED = "IsDeleted";
        private const string ENTITY_PROPERTY_DELETEDON = "DeletedOn";
        private const string ENTITY_PROPERTY_ID = "ID";
        #endregion

        /// <summary>
        /// The db
        /// </summary>
        private DbModelContainer db = null;

        static BaseRepository()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        public BaseRepository()
        {
            db = new DbModelContainer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="dc">The dc.</param>
        public BaseRepository(DbModelContainer dc)
        {
            db = dc;
        }

        /// <summary>
        /// Sets the db model.
        /// </summary>
        /// <param name="dc">The dc.</param>
        public void SetDbModel(DbModelContainer dc)
        {
            db = dc;
        }

        /// <summary>
        /// Gets the DB.
        /// </summary>
        /// <value>The DB.</value>
        protected internal DbModelContainer DB
        {
            get
            {
                if (db == null)
                {
                    db = new DbModelContainer();
                    db.Database.Log += DBLog;
                }

                return db;
            }
        }
        private void DBLog(string log)
        {
            Debug.WriteLine("SQL:");
            Debug.WriteLine(log);
        }
        /// <summary>
        /// Gets user id from current http context.
        /// </summary>
        /// <value>The User ID.</value>
        protected internal int ContextUserID
        {
            get
            {
                var httpContext = System.Web.HttpContext.Current;

                if (httpContext == null)
                    return -2;

                if (httpContext.Session != null && httpContext.Session["UserID"] != null)
                    return (int)(httpContext.Session["UserID"]);

                return -1;
            }
        }

        /// <summary>
        /// Gets user full name from current http context.
        /// </summary>
        /// <value>The User FullName.</value>
        protected internal string ContextUserName
        {
            get
            {
                var httpContext = System.Web.HttpContext.Current;

                if (httpContext != null && httpContext.Session != null && httpContext.Session["UserName"] != null)
                {
                    return httpContext.Session["UserName"].ToString();
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }


        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// News the entity.
        /// </summary>
        /// <returns>`0.</returns>
        public TEntity NewEntity()
        {
            TEntity entity = new TEntity();
            DB.Set<TEntity>().Add(entity);

            return entity;
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>`0.</returns>
        public TEntity GetByID(object id)
        {
            return DB.Set<TEntity>().Find(id);

        }

        /// <summary>
        /// Deletes the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records">The records.</param>
        public void DeleteRecords<T>(ICollection<T> records) where T : class, IEntityExtendedProperty, new()
        {
            var q = records.ToList();

            foreach (var item in q)
            {
                DB.Entry(DB.Set<T>().Find(item)).State = EntityState.Deleted;
            }
        }

        /// <summary>
        /// Bases the list.
        /// </summary>
        /// <returns>IQueryable{`0}.</returns>
        protected IQueryable<TEntity> BaseList()
        {
            IQueryable<TEntity> query = DB.Set<TEntity>();

            TEntity entity = new TEntity();
            if (entity.HasColumnIsDeleted)
            {
                query = query.Where("IsDeleted == NULL || IsDeleted==false");
            }

            return query;
        }

        /// <summary>
        /// Base_s the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IQueryable{``0}.</returns>
        protected IQueryable<T> Base_List<T>() where T : class, IEntityExtendedProperty, new()
        {
            IQueryable<T> query = DB.Set<T>();

            T entity = new T();
            if (entity.HasColumnIsDeleted)
            {
                query = query.Where("IsDeleted == NULL || IsDeleted==false");
            }

            return query;
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns>List{`0}.</returns>
        public virtual List<TEntity> GetList()
        {
            return this.BaseList().ToList();
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> whereFunc, string orderBy = null)
        {
            TEntity entity = new TEntity();
            if (orderBy == null)
            {
                return this.BaseList().Where(whereFunc);
            }
            return this.BaseList().OrderBy(orderBy).Where(whereFunc);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="whereFuncs">The where FUNCS.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy = null)
        {
            TEntity entity = new TEntity();


            var query = this.BaseList();
            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            if (orderBy == null)
            {
                return query;
            }
            return query.OrderBy(orderBy);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, string orderBy)
        {

            TEntity entity = new TEntity();
            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }
            return this.BaseList().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize)
        {
            TEntity entity = new TEntity();
            return this.BaseList().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc)
        {

            TEntity entity = new TEntity();

            return this.BaseList().Where(whereFunc).AsQueryable().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where funcs.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs)
        {

            TEntity entity = new TEntity();

            var query = this.BaseList();
            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            return query.AsQueryable().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, out int total)
        {

            TEntity entity = new TEntity();

            var query = this.BaseList();
            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            total = query.Count();

            return query.AsQueryable().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, string orderBy)
        {

            TEntity entity = new TEntity();

            return this.BaseList().Where(whereFunc).AsQueryable().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy)
        {

            TEntity entity = new TEntity();

            var query = this.BaseList();
            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            if (orderBy == null)
                orderBy = entity.DefaultOrderColumnName;

            return query.AsQueryable().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }


        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, out int total)
        {

            TEntity entity = new TEntity();

            total = this.BaseList().Count();

            return this.BaseList().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, out int total)
        {

            TEntity entity = new TEntity();

            total = this.BaseList().Where(whereFunc).Count();

            return this.BaseList().Where(whereFunc).AsQueryable().OrderBy(entity.DefaultOrderColumnName).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, string orderBy, out int total)
        {

            TEntity entity = new TEntity();

            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }
            total = this.BaseList().Count();

            return this.BaseList().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFunc">The where func.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> whereFunc, string orderBy, out int total)
        {

            TEntity entity = new TEntity();

            total = this.BaseList().Where(whereFunc).Count();
            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }

            return this.BaseList().Where(whereFunc).AsQueryable().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where FUNCS.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy, out int total)
        {

            TEntity entity = new TEntity();

            IQueryable<TEntity> query = this.BaseList();

            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            total = query.Count();
            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }

            return query.OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
        }

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="where">The where.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="total">The total.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetList(int pageIndex, int pageSize, string where, string orderBy, out int total)
        {

            TEntity entity = new TEntity();



            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }
            if (string.IsNullOrEmpty(where))
            {
                total = this.BaseList().Count();
                return this.BaseList().OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
            }
            else
            {
                total = this.BaseList().Where(where).Count();
                return this.BaseList().Where(where).OrderBy(orderBy).Skip(pageSize * pageIndex).Take(pageSize);
            }
        }

        /// <summary>
        /// Gets the list for rotator.
        /// </summary>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="whereFuncs">The where FUNCS.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns>IQueryable{`0}.</returns>
        public virtual IQueryable<TEntity> GetListForRotator(int itemIndex, int pageSize, List<Expression<Func<TEntity, bool>>> whereFuncs, string orderBy)
        {

            TEntity entity = new TEntity();

            IQueryable<TEntity> query = this.BaseList();

            foreach (var item in whereFuncs)
            {
                query = query.Where(item);
            }

            if (orderBy == null)
            {
                orderBy = entity.DefaultOrderColumnName;
            }

            return query.OrderBy(orderBy).Skip(itemIndex).Take(pageSize);

        }

        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>`0.</returns>
        public TEntity Add(TEntity entity)
        {
            return DB.Set<TEntity>().Add(entity);

        }


        /// <summary>
        /// Saves this instance.
        /// </summary>
        public virtual void Save()
        {
            var entries = DB.ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                var baseType = entry.Entity.GetType().BaseType;

                if (baseType != null)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:

                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_ISDELETED))
                                entry.CurrentValues[ENTITY_PROPERTY_ISDELETED] = false;

                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_CREATEDBY))
                                entry.CurrentValues[ENTITY_PROPERTY_CREATEDBY] = this.ContextUserID;

                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_CREATEDON))
                                entry.CurrentValues[ENTITY_PROPERTY_CREATEDON] = DateTime.Now;

                            break;

                        case EntityState.Modified:
                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDBY))
                                entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDBY] = this.ContextUserID;

                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDON))
                                entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDON] = DateTime.Now;

                            break;

                        case EntityState.Deleted:
                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_DELETEDON))
                                entry.CurrentValues[ENTITY_PROPERTY_DELETEDON] = DateTime.Now;

                            break;
                    }
                }
            }
            try
            {
                DB.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                Debug.WriteLine(dbEx.ToString());
                throw dbEx;
            }

        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (entity.HasColumnIsDeleted)
            {
                var entry = DB.Entry(entity);

                if (entry != null)
                {
                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDBY))
                        entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDBY] = this.ContextUserID;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDON))
                        entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDON] = DateTime.Now;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_ISDELETED))
                        entry.CurrentValues[ENTITY_PROPERTY_ISDELETED] = true;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_DELETEDON))
                        entry.CurrentValues[ENTITY_PROPERTY_DELETEDON] = DateTime.Now;
                }

            }
            else
                DB.Set<TEntity>().Remove(entity);
        }

        /// <summary>
        /// Deletes the by ID.
        /// </summary>
        /// <param name="id">The id.</param>
        public virtual void DeleteByID(object id)
        {
            TEntity entity = this.GetByID(id);

            if (entity != null && entity.HasColumnIsDeleted)
            {
                var entry = DB.Entry(entity);

                if (entry != null)
                {
                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDBY))
                        entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDBY] = this.ContextUserID;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDON))
                        entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDON] = DateTime.Now;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_ISDELETED))
                        entry.CurrentValues[ENTITY_PROPERTY_ISDELETED] = true;

                    if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_DELETEDON))
                        entry.CurrentValues[ENTITY_PROPERTY_DELETEDON] = DateTime.Now;
                }

            }
            else
                DB.Set<TEntity>().Remove(entity);
        }

    }
}
