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
using LMYFrameWorkMVC.Common.Helpers;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.Models;
using System.Data.Entity.Validation;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Message;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using System.Data.Entity;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Message
{
    public class MessageBAL : BaseBAL
    {
        public MessageBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        public void PrepareMessageModel(MessageModel messageModel)
        {
            try
            {
                //  LMYFrameWorkMVC.DAL.Message message = dbContext.Messages.Where(x => x.ID == messageModel.Id).FirstOrDefault();
                //MessageMapper.Map(dbContext, messageModel.FromUserID, message, messageModel);

                IQueryable<AspNetUser> aspNetUsers = dbContext.AspNetUsers.Where(x => x.Id != contextInfo.UserID);
                UserMapper.Map(dbContext, aspNetUsers.ToList(), messageModel.ToUsersListModel);
                messageModel.ToUsersListItems = messageModel.ToUsersListModel.Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                messageModel.Roles = dbContext.AspNetRoles.ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            }
            catch (Exception ex)
            {
                base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetMessageModel(MessageModel messageModel, string MessageViewRequestUserID)
        {
            try
            {
                //the below include is because a bug in EF6
                LMYFrameWorkMVC.Common.DAL.Message message = dbContext.Messages.Where(x => x.ID == messageModel.ID).FirstOrDefault();

                if (message == null)
                {
                    base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    MessageMapper.Map(dbContext, MessageViewRequestUserID, message, messageModel);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        private bool Validate(MessageModel messageModel)
        {
            if (string.IsNullOrEmpty(messageModel.MessageText))
                base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.ThisIsTheLastRecord);

            return messageModel.HasErrorByType();
        }

        public GenericListModel<MessageModel> GetMyMessagesByOtherUser(string userID, int page, int pageSize)
        {
            GenericListModel<MessageModel> baseListModel = new GenericListModel<MessageModel>();
            try
            {
                IQueryable<LMYFrameWorkMVC.Common.DAL.Message> messages = dbContext.Messages.Where(x =>
                    (x.FromUserID == contextInfo.UserID && x.ToUserID == userID) ||
                    (x.FromUserID == userID && x.ToUserID == contextInfo.UserID)
                    ).OrderByDescending(x => x.CreatedAt);

                baseListModel.Total = messages.Count();

                messages = messages.Skip(pageSize * (page - 1)).Take(pageSize);

                MessageMapper.Map(dbContext, contextInfo.UserID, messages.ToList(), baseListModel.List);

                //Action action = (() =>
                //{
                //    using (var transaction = dbContext.Database.BeginTransaction())
                //    {
                //        try
                //        {
                //            //update all other users messages
                //            messages.Where(x => x.FromUserID != contextInfo.UserID).ToList().ForEach(x => x.Read = true);

                //            base.SaveChanges();
                //        }
                //        catch (Exception ex)
                //        {
                //            this.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
                //        }
                //        transaction.Commit();
                //    }
                //});
                //base.AddWaitingAction(action);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void SetMessageAsRead(string messageID)
        {
            try
            {
                //Action action = (() =>
                //{
                    using (var transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //update all other users messages
                            dbContext.Messages.Where(x => x.ID == messageID).FirstOrDefault().Read = true;

                            base.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            this.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
                        }
                        transaction.Commit();
                    }
                //});
                //base.AddWaitingAction(action);

            }
            catch (Exception ex)
            {
                base.HandleError(null, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<UserModel> ListUsersWithLastMessageByCurrentUser()
        {
            GenericListModel<UserModel> baseListModel = new GenericListModel<UserModel>();
            try
            {
                //all users that have conversations with current user
                IQueryable<AspNetUser> aspNetUsers = dbContext.AspNetUsers.Where(x =>
                        x.Id != contextInfo.UserID &&
                        (
                            x.Messages.Any(c => c.ToUserID == contextInfo.UserID || c.FromUserID == contextInfo.UserID) ||
                            x.Messages1.Any(c => c.ToUserID == contextInfo.UserID || c.FromUserID == contextInfo.UserID)
                        )
                    );

                UserMapper.Map(dbContext, aspNetUsers.ToList(), baseListModel.List);

                foreach (UserModel userModel in baseListModel.List)
                {
                    //get last message between two users
                    LMYFrameWorkMVC.Common.DAL.Message message = dbContext.Messages.Where(x =>
                    (x.FromUserID == contextInfo.UserID || x.ToUserID == contextInfo.UserID) &&
                    (x.FromUserID == userModel.Id || x.ToUserID == userModel.Id)
                    ).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

                    MessageMapper.Map(dbContext, contextInfo.UserID, message, userModel.LastMessageModel);
                }

                baseListModel.List = baseListModel.List.OrderByDescending(x => x.LastMessageModel.CreatedAt).ToList();
                baseListModel.Total = aspNetUsers.Count();
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void CreateToMany(MessageModel messageModel)
        {
            //try
            //{
            //    if (Validate(messageModel))
            //        return;

            //    //Action action = (() =>
            //    //{
            //    using (var transaction = dbContext.Database.BeginTransaction())
            //    {
            //        try
            //        {
            //            foreach (string toUuserId in messageModel.ToUsersIDs)
            //            {
            //                LMYFrameWorkMVC.DAL.Message message = new LMYFrameWorkMVC.DAL.Message();
            //                MessageMapper.Map(dbContext, messageModel, message);
            //                message.ToUserID = toUuserId;
            //                message.ID = Guid.NewGuid().ToString();
            //                dbContext.Messages.Add(message);
            //            }

            //            base.SaveChanges();
            //        }
            //        catch (Exception ex)
            //        {
            //            this.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
            //        }
            //        transaction.Commit();
            //    }
            //    //});
            //    //base.AddWaitingAction(action);
            //}
            //catch (Exception ex)
            //{
            //    base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Exception, ex);
            //}
        }

        public void Create(MessageModel messageModel)
        {
            try
            {
                if (Validate(messageModel))
                    return;

                // Action action = (() =>
                // {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        LMYFrameWorkMVC.Common.DAL.Message message = new LMYFrameWorkMVC.Common.DAL.Message();
                        messageModel.ID = Guid.NewGuid().ToString();
                        MessageMapper.Map(dbContext, messageModel, message);

                        dbContext.Messages.Add(message);

                        base.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        this.HandleError(null, CommonLayer.LookUps.ErrorType.Exception, ex);
                    }
                    transaction.Commit();
                }

                messageModel.AddSuccess(Resources.MessageSent, LookUps.SuccessType.Full);

                // });
                //base.AddWaitingAction(action);
            }
            catch (Exception ex)
            {
                base.HandleError(messageModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public int GetNotFromMeAndNotReadCountByUserID(string userId)
        {
            try
            {
                int count = 0;
                //the below include is because a bug in EF6
                List<string> aspNetUsersIDs = dbContext.AspNetUsers.Where(x =>
                           x.Id != contextInfo.UserID &&
                           (
                               x.Messages.Any(c => c.ToUserID == userId || c.FromUserID == userId) ||
                               x.Messages1.Any(c => c.ToUserID == userId || c.FromUserID == userId)
                           )
                       ).Select(x => x.Id).ToList();

                foreach (string aspNetUserID in aspNetUsersIDs)
                {
                    //get last message between two users
                    LMYFrameWorkMVC.Common.DAL.Message message = dbContext.Messages.Where(x =>
                    (x.FromUserID == userId || x.ToUserID == userId) &&
                    (x.FromUserID == aspNetUserID || x.ToUserID == aspNetUserID)
                    ).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                    if (message != null && message.FromUserID!=userId && message.Read == false)
                    {
                        count++;
                    }
                }

                return count;
            }
            catch (Exception ex)
            {
                base.HandleError(null, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return 0;
        }
        //public override void Dispose()
        //{
        //    base.Dispose();
        //}
    }
}
