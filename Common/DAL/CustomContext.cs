using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.Common.DAL
{
    public partial class LMYFrameWorkMVCEntities : DbContext
    {
        private bool prepared;
        //private bool enableAuditTrail;
        public LMYFrameWorkMVCEntities(bool prepared = true)
            : this()
        {
            
            this.prepared = prepared;
            //Configuration.ProxyCreationEnabled = false;
            ///Configuration.LazyLoadingEnabled = true;
            //Configuration.ProxyCreationEnabled = true;
            //Configuration.LazyLoadingEnabled = true;
            //Configuration.AutoDetectChangesEnabled = true;
        }

        public int SaveChanges(string userID, bool enableAuditTrail)
        {
            if (!this.prepared)
                throw new Exception("Please call prepare context before call save changes");

            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(x => x.Entity is IAuditedDatesEntity || x.Entity is IAuditedUsersEntity))
            {
                if (entry.State != EntityState.Deleted && entry.State != EntityState.Unchanged && entry.State != EntityState.Detached)
                {
                    IAuditedDatesEntity iAuditedDatesEntity = null;
                    IAuditedUsersEntity iAuditedUsersEntity = null;

                    if (entry.Entity is IAuditedDatesEntity)
                        iAuditedDatesEntity = (IAuditedDatesEntity)entry.Entity;
                    if (entry.Entity is IAuditedUsersEntity)
                        iAuditedUsersEntity = (IAuditedUsersEntity)entry.Entity;

                    if (!string.IsNullOrEmpty(userID) && iAuditedUsersEntity != null)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            iAuditedUsersEntity.CreatedBy = userID;
                        }

                        iAuditedUsersEntity.LastModifiedBy = userID;
                    }

                    if (iAuditedDatesEntity != null)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            iAuditedDatesEntity.CreatedAt = DateTime.Now;
                        }
                        iAuditedDatesEntity.LastModifiedAt = DateTime.Now;
                    }
                }
            }

            //foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(x => x.Entity is ICacheClear))
            //{
            //    if (entry.Entity != null && (entry.Entity is ICacheClear))
            //        ((ICacheClear)entry.Entity).ClearCache();
            //}

            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(x => x.Entity is IAuditWrapper))
            {
                if (enableAuditTrail && entry.Entity != null && entry.State != EntityState.Unchanged && entry.State != EntityState.Detached && !(entry.Entity is DBAudit))
                {
                    AddAuditTrail(
                           entry.State != EntityState.Deleted ? ((IAuditWrapper)entry.Entity).GetJSONData() : null,
                          ((IAuditWrapper)entry.Entity).GetPrimaryKey(),
                           userID,
                           GetTableName(entry.Entity.GetType()),
                           entry.State
                           );
                }
            }

            foreach (DbEntityEntry entry in ChangeTracker.Entries().Where(x => x.Entity is ICacheClear))
            {
                if (entry.Entity != null && entry.State != EntityState.Unchanged && entry.State != EntityState.Detached)
                    ((ICacheClear)entry.Entity).ClearCache();
            }

            return base.SaveChanges();
        }

        public override int SaveChanges()
        {
            return this.SaveChanges(null, true);
        }

        private string GetTableName(Type type)
        {
            if (type.BaseType != null && type.BaseType.Namespace != "System")
            {
                return type.BaseType.Name;
            }

            return type.Name;
        }

        private void AddAuditTrail(string jsondata, string parentID, string userID, string tableName, EntityState entityState)
        {
            this.DBAudits.Add(new DBAudit
            {
                AuditId = Guid.NewGuid().ToString(),
                ParentID = parentID,
                RevisionStamp = DateTime.UtcNow,
                TableName = tableName,
                UserId = userID,
                Action = entityState.ToString(),
                JSONData = jsondata
            });
        }
    }
}
