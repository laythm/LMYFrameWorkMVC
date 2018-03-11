using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using System.Data.Entity.Validation;
using LMYFrameWorkMVC.Common.Models;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Role;
using System.Data.Entity;
using System.Linq.Expressions;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class RoleBAL : BaseBAL
    {
        public RoleBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(RoleModel roleModel)
        {
            if (dbContext.AspNetRoles.Any(x => x.Name.ToLower() == roleModel.Name.ToLower() && x.Id != roleModel.Id.ToString()))
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.NameExists, roleModel.nameof(x => x.Name));

            return roleModel.HasErrorByType();
        }

        private bool ValidateDelete(RoleModel roleModel)
        {
            if (!dbContext.AspNetRoles.Find(roleModel.Id).IsDeleteAble)
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.ThisIsNotDeletAble);

            if (dbContext.AspNetRoles.Count() == 1)
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.ThisIsTheLastRecord);

            return roleModel.HasErrorByType();
        }

        public void PrepareRoleModel(RoleModel roleModel)
        {
            try
            {

            }
            catch (Exception ex)
            {
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetRoleModel(RoleModel roleModel)
        {
            try
            {
                AspNetRole aspNetRole = dbContext.AspNetRoles.Where(x => x.Id == roleModel.Id).FirstOrDefault();

                if (aspNetRole == null)
                {
                    base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    RoleMapper.Map(aspNetRole, roleModel);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<RoleModel> GetSearchRolesList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<RoleModel> baseListModel = new GenericListModel<RoleModel>();
            try
            {
                IQueryable<AspNetRole> aspNetRoles = dbContext.AspNetRoles.OrderBy(x => x.CreatedAt);

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    aspNetRoles = aspNetRoles.Where(x => x.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()));
                }


                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                aspNetRoles = aspNetRoles.OrderBy(c => c.Name);
                            else
                                aspNetRoles = aspNetRoles.OrderByDescending(c => c.Name);
                            break;
                    }
                }

                baseListModel.Total = aspNetRoles.Count();
                aspNetRoles = aspNetRoles.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    aspNetRoles = aspNetRoles.Take(dataTableSearchParameters.length);

                RoleMapper.Map(aspNetRoles.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(RoleModel roleModel)
        {
            try
            {
                if (Validate(roleModel))
                    return;
                //using (var transaction = dbContext.Database.BeginTransaction())
                //{
                AspNetRole aspNetRole = new AspNetRole();
                roleModel.Id = Guid.NewGuid().ToString();
                aspNetRole.SetDefaultValues();
                RoleMapper.Map(roleModel, aspNetRole);
                dbContext.AspNetRoles.Add(aspNetRole);

                base.SaveChanges();
                // transaction.Commit();
                //remove all roles cache with sitemap nodes cache
                Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Roles, ObjectId = LookUps.CacheKeys.Roles.ToString() });

                roleModel.AddSuccess(Resources.RoleAddedSuccessfully, LookUps.SuccessType.Full);
                // }
            }
            catch (Exception ex)
            {
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(RoleModel roleModel)
        {
            try
            {
                if (Validate(roleModel))
                    return;

                AspNetRole aspNetRole = dbContext.AspNetRoles.Where(x => x.Id == roleModel.Id).FirstOrDefault();

                if (aspNetRole == null)
                {
                    base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                //no need for below because cascading applied
                dbContext.AspNetRoleSiteMapNodes.RemoveRange(aspNetRole.AspNetRoleSiteMapNodes);
                aspNetRole.AspNetRoleSiteMapNodes.Clear();
                RoleMapper.Map(roleModel, aspNetRole);

                dbContext.Entry(aspNetRole).State = EntityState.Modified;

                base.SaveChanges();

                //remove all roles cache with sitemap nodes cache
                Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Roles, ObjectId = LookUps.CacheKeys.Roles.ToString() });

                roleModel.AddSuccess(Resources.RoleUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(RoleModel roleModel)
        {
            try
            {
                if (ValidateDelete(roleModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        AspNetRole aspNetRole = dbContext.AspNetRoles.Where(x => x.Id == roleModel.Id).FirstOrDefault();

                        if (aspNetRole == null)
                        {
                            base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                            return;
                        }

                        dbContext.AspNetRoleSiteMapNodes.RemoveRange(aspNetRole.AspNetRoleSiteMapNodes);
                        dbContext.AspNetRoles.Remove(aspNetRole);

                        base.SaveChanges();
                        transaction.Commit();

                        roleModel.AddSuccess(Resources.RoleDeletedSuccessfully, LookUps.SuccessType.Full);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            catch (Exception ex)
            {
                base.HandleError(roleModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }
 
        //public override void Dispose()
        //{
        //    base.Dispose();
        //}
    }
}
