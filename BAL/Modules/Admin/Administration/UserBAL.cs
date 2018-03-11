using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.Objects;
using System.Linq.Expressions;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class UserBAL : BaseBAL
    {
        public UserBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(UserModel userModel)
        {
            if (dbContext.AspNetUsers.Any(x => x.UserName == userModel.UserName && x.Id != userModel.Id.ToString()))
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.UserNameExists, userModel.nameof(x => x.UserName));

            if (dbContext.AspNetUsers.Any(x => x.Email == userModel.Email && x.Id != userModel.Id.ToString()))
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.EmailExists, userModel.nameof(x => x.Email));

            if (dbContext.AspNetUsers.Any(x => x.PhoneNumber == userModel.PhoneNumber && x.Id != userModel.Id.ToString()))
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.PhoneNumberExists, userModel.nameof(x => x.PhoneNumber));
            }

            //if (dbContext.AspNetUsers.Count() == 1)
            //    base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.Resources.ThisIsTheLastRecord);

            return userModel.HasErrorByType();
        }

        private bool ValidateDelete(UserModel userModel)
        {
            if (dbContext.AspNetUsers.Count() == 1)
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.ThisIsTheLastRecord);

            return userModel.HasErrorByType();
        }

        public void PrepareUserModel(UserModel userModel)
        {
            try
            {
                //if (!base.UserHasPermision(userModel))
                //    return;

                userModel.AspNetRolesListItems = dbContext.AspNetRoles.Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = x.AspNetUsers.Any(a => a.Id == userModel.Id) }).ToList();
            }
            catch (Exception ex)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetUserModel(UserModel userModel)
        {
            try
            {
                AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.Id == userModel.Id).FirstOrDefault();

                if (aspNetUser == null)
                {
                    base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    UserMapper.Map(dbContext, aspNetUser, userModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

        }

        public void PrepareListUserModel(GenericListModel<UserModel> baseListModel)
        {
            try
            {
                //if (!base.UserHasPermision(userModel))
                //    return;

                foreach (UserModel userModel in baseListModel.List)
                {
                    PrepareUserModel(userModel);
                    userModel.AspNetRolesListItems = dbContext.AspNetRoles.Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = userModel.RolesIDs.Any(y => y == x.Id) }).ToList();
                }
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<UserModel> GetSearchUsersList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<UserModel> baseListModel = new GenericListModel<UserModel>();

            try
            {
                //if (!base.UserHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<AspNetUser> aspNetUsers = dbContext.AspNetUsers.OrderBy(x => x.CreatedAt);

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    aspNetUsers = aspNetUsers.Where(x =>
                                                    x.UserName.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                                                    x.EnglishName.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                                                    x.ArabicName.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                                                    x.Email.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                                                    x.PhoneNumber.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                                                    x.AspNetRoles.Any(a => a.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()))
                                                   );
                }

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                aspNetUsers = aspNetUsers.OrderBy(c => c.UserName);
                            else
                                aspNetUsers = aspNetUsers.OrderByDescending(c => c.UserName);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                aspNetUsers = CommonLayer.Helpers.Utilites.GetLocalizedOrderBy(aspNetUsers.OrderBy(c => c.EnglishName), aspNetUsers.OrderBy(c => c.ArabicName));
                            else
                                aspNetUsers = CommonLayer.Helpers.Utilites.GetLocalizedOrderBy(aspNetUsers.OrderByDescending(c => c.EnglishName), aspNetUsers.OrderByDescending(c => c.ArabicName));
                            break;
                        case 2:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                aspNetUsers = aspNetUsers.OrderBy(c => c.Email);
                            else
                                aspNetUsers = aspNetUsers.OrderByDescending(c => c.Email);
                            break;
                        case 3:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                aspNetUsers = aspNetUsers.OrderBy(c => c.PhoneNumber);
                            else
                                aspNetUsers = aspNetUsers.OrderByDescending(c => c.PhoneNumber);
                            break;
                    }
                }

                baseListModel.Total = aspNetUsers.Count();
                aspNetUsers = aspNetUsers.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    aspNetUsers = aspNetUsers.Take(dataTableSearchParameters.length);

                UserMapper.Map(dbContext, aspNetUsers.ToList(), baseListModel.List);

                PrepareListUserModel(baseListModel);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(UserModel userModel, CommonLayer.Entities.ITemplate regestrationCompleted)
        {
            try
            {
                //if (!base.UserHasPermision(userModel))
                //    return;

                //example of tasks
                //for (int i = 0; i < 10000000; i++)
                //    base.AddWaitingAction((async () =>
                //    {
                //        System.Threading.Thread.Sleep(1000);
                //    }));

                if (Validate(userModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        AspNetUser aspNetUser = new AspNetUser();
                        UserMapper.Map(dbContext, userModel, aspNetUser);
                        aspNetUser.PasswordHash = Hasher.HashString(userModel.Password);
                        aspNetUser.Id = Guid.NewGuid().ToString();
                        //aspNetUser.AspNetRoles = userModel.RolesIDs.Select(x => dbContext.AspNetRoles.Find(x)).ToList();
                        aspNetUser.EmailConfirmed = true;
                        aspNetUser.SecurityStamp = Guid.NewGuid().ToString();
                        dbContext.AspNetUsers.Add(aspNetUser);

                        base.SaveChanges();
                        transaction.Commit();

                        userModel.AddSuccess(Resources.UserAddedSuccessfully, LookUps.SuccessType.Full);

                        CommonLayer.TemplateLists.Regestration regestration = new CommonLayer.TemplateLists.Regestration();

                        regestration.ID = aspNetUser.Id;
                        regestration.UserName = aspNetUser.UserName;

                        //call method in base call (IEmail,to,cc,title)
                        //if (regestrationCompleted != null)
                        //{
                        //    //pass the parameters and then send the email from here
                        //    //regestrationCompleted.PrepareTemplate(regestration); 
                        //}
                        //base.SendEmail()
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
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(UserModel userModel)
        {
            try
            {
                //if (!base.UserHasPermision(userModel))
                //    return;

                AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.Id == userModel.Id).FirstOrDefault();

                if (aspNetUser == null)
                {
                    base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(userModel))
                    return;

                UserMapper.Map(dbContext, userModel, aspNetUser);

                base.SaveChanges();

                userModel.AddSuccess(Resources.UserUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(UserModel userModel)
        {
            try
            {
                if (ValidateDelete(userModel))
                    return;

                AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.Id == userModel.Id).FirstOrDefault();

                if (aspNetUser == null)
                {
                    base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                aspNetUser.AspNetRoles.Clear();
                dbContext.AspNetRoles.RemoveRange(aspNetUser.AspNetRoles);
                dbContext.AspNetUsers.Remove(aspNetUser);

                base.SaveChanges();

                userModel.AddSuccess(Resources.UserDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Login(LoginViewModel model)
        {
            AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.UserName == model.UserName).ToList().Where(x => Hasher.ValidateHash(x.PasswordHash, model.Password)).FirstOrDefault();

            if (aspNetUser == null)
            {
                base.HandleError(model, CommonLayer.LookUps.ErrorType.Business, null, Resources.WrongUserNameOrPassword);
                return;
            }
            else
            {
                model.Id = aspNetUser.Id;
            }
        }

        public void ChangePassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                if (!base.UserHasPermision(changePasswordModel))
                    return;

                AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.Id == changePasswordModel.Id).FirstOrDefault();

                if (aspNetUser == null)
                {
                    base.HandleError(changePasswordModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                aspNetUser.PasswordHash = Hasher.HashString(changePasswordModel.Password);

                base.SaveChanges();

                changePasswordModel.AddSuccess(Resources.UserPasswordChangedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(changePasswordModel, CommonLayer.LookUps.ErrorType.Exception, ex);
            }
        }

        public void ChangePassword(UserModel userModel)
        {
            try
            {
                if (!base.UserHasPermision(userModel))
                    return;

                AspNetUser aspNetUser = dbContext.AspNetUsers.Where(x => x.Id == userModel.Id).FirstOrDefault();

                if (aspNetUser == null)
                {
                    base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                aspNetUser.PasswordHash = Hasher.HashString(userModel.Password);

                base.SaveChanges();

                userModel.AddSuccess(Resources.UserPasswordChangedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(userModel, CommonLayer.LookUps.ErrorType.Exception, ex);
            }
        }

        public void addOrUpdateConnection(AspNetUserConnectionModel aspNetUserConnectionModel)
        {
            try
            {

                AspNetUser_Connections aspNetUser_Connections = dbContext.AspNetUser_Connections.Where(x => x.ConnectionID == aspNetUserConnectionModel.ConnectionID).FirstOrDefault();
                if (aspNetUser_Connections == null)
                {
                    aspNetUser_Connections = new AspNetUser_Connections();
                    aspNetUserConnectionModel.ID = Guid.NewGuid().ToString();
                    UserMapper.Map(dbContext, aspNetUserConnectionModel, aspNetUser_Connections);

                    dbContext.AspNetUser_Connections.Add(aspNetUser_Connections);
                }
                else
                {
                    //update
                    UserMapper.Map(dbContext, aspNetUserConnectionModel, aspNetUser_Connections);
                }

                base.SaveChanges(false);
            }
            catch (Exception ex)
            {

                base.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
            }

        }

        public void removeConnection(string connectionID)
        {
            try
            {
                AspNetUser_Connections aspNetUser_Connections = dbContext.AspNetUser_Connections.Where(x => x.ConnectionID == connectionID).FirstOrDefault();
                if (aspNetUser_Connections != null)
                {
                    dbContext.AspNetUser_Connections.Remove(aspNetUser_Connections);

                    base.SaveChanges(false);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
            }
        }

        public void SetOfflineDisconnectedUsers(List<string> lifeConnections)
        {
            try
            {
                //remove all connections where life connections not in them
                dbContext.AspNetUser_Connections.RemoveRange(dbContext.AspNetUser_Connections.Where(x => !lifeConnections.Any(l => l == x.ConnectionID)).ToList());

                base.SaveChanges(false);
            }
            catch (Exception ex)
            {

                base.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
            }

        }
        //public override void Dispose()
        //{
        //    base.Dispose();
        //}
    }
}
