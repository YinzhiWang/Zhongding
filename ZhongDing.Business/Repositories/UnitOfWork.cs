using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.Repositories
{
    public class UnitOfWork : IUnitOfWork
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

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        public UnitOfWork()
        {
            db = new DbModelContainer();
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
        /// Gets the db model.
        /// </summary>
        /// <returns>DbModelContainer.</returns>
        public DbModelContainer GetDbModel()
        {
            return this.DB;
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
                    db = new DbModelContainer();

                return db;
            }
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
                    return -1;

                if (httpContext.Session != null && httpContext.Session["UserID"] != null)
                    return (int)(httpContext.Session["UserID"]);

                return -1;
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
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SaveChanges()
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
                                entry.CurrentValues[ENTITY_PROPERTY_CREATEDON] = DateTime.UtcNow;

                            break;

                        case EntityState.Modified:
                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDBY))
                                entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDBY] = this.ContextUserID;

                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_LASTMODIFIEDON))
                                entry.CurrentValues[ENTITY_PROPERTY_LASTMODIFIEDON] = DateTime.UtcNow;

                            break;

                        case EntityState.Deleted:
                            if (entry.CurrentValues.PropertyNames.Contains(ENTITY_PROPERTY_DELETEDON))
                                entry.CurrentValues[ENTITY_PROPERTY_DELETEDON] = DateTime.UtcNow;

                            break;
                    }
                }
            }

            DB.SaveChanges();
        }

    }
}
