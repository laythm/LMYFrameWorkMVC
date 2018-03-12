using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Security.Principal;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;

using System.Reflection;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.Common.Helpers
{
    public class BaseBAL : IDisposable
    {
        #region protected Properties

        protected LMYFrameWorkMVCEntities dbContext;
        protected ContextInfo contextInfo;

        #endregion

        #region private Properties

        private List<Action> waitingActions;
        private List<Notification> notifications;
        private bool hasWaitingActions
        {
            get
            {
                return this.waitingActions != null && this.waitingActions.Count() > 0;
            }
        }

        #endregion

        #region constuctors

        ~BaseBAL()
        {
            Dispose();
        }

        public BaseBAL(ContextInfo contextInfo)
        {
            this.dbContext = new LMYFrameWorkMVCEntities(true);
            this.contextInfo = contextInfo;
            notifications = new List<Notification>();
        }

        #endregion

        #region dispose

        public virtual void Dispose()
        {
            if (!this.hasWaitingActions)
            {
                // Dispose managed resources.
                if (dbContext != null)
                    dbContext.Dispose();

                // if (contextInfo != null)
                //   contextInfo.Dispose();

                waitingActions = null;
                notifications = null;

                GC.SuppressFinalize(this);
            }
            else
            {
                this.SetTasksAndStart();
            }
        }

        #endregion

        protected void SaveChanges(bool enableAuditTrailForThisSave = true)
        {
            if (enableAuditTrailForThisSave == true)
                enableAuditTrailForThisSave = Utilites.GetBoolSettingValue(LookUps.SettingsKeys.EnableAuditTrail);

            dbContext.SaveChanges(contextInfo.UserID, enableAuditTrailForThisSave);
        }

        protected void HandleError(BaseModel baseModel = null, LookUps.ErrorType errorType = LookUps.ErrorType.Exception, Exception ex = null, string message = null, string propertyType = "")
        {
            if (baseModel != null)
            {
                if (ex != null)
                    baseModel.AddError(ex.Message, errorType, propertyType);
                else
                    baseModel.AddError(message, errorType, propertyType);
            }

            if (ex != null)
                contextInfo.ErrorHelper.LogError(ex);
        }

        protected bool UserHasPermision(BaseModel baseModel, bool addError = true, string customMessage = null)
        {
            if (string.IsNullOrEmpty(customMessage))
                customMessage = Resources.Resources.LoginFirstToCompleteOperation;

            if (!contextInfo.IsUserAuthenticated && addError)
            {
                this.HandleError(baseModel, LookUps.ErrorType.Critical, null, customMessage);
                return false;
            }

            return true;
        }

        protected void SendEmail(string subject, string body, List<MailAddress> to, List<MailAddress> cc = null)
        {
            EmailInfo emailInfo = new EmailInfo();
            emailInfo.To = to;
            emailInfo.CC = cc;
            emailInfo.Subject = subject;
            emailInfo.Body = body;

            //set smtp info
            //emailInfo.SMTPInfo.From;

            Helpers.EmailHelper emailHelper = new Helpers.EmailHelper();
            Action action = (async () =>
                   {
                       try
                       {
                           //IQueryable<AspNetUser> AspNetUsers = dbContext.AspNetUsers.Where(x => to.Any(y => y.Email == x.Email));
                           await emailHelper.SendEmail(emailInfo);
                       }
                       catch (Exception ex)
                       {
                           this.HandleError(null, LookUps.ErrorType.Exception, ex);
                       }
                   });

            this.AddAfterExecutionAction(action);
        }

        protected void AddAfterExecutionAction(Action action, bool startNowAndWait = false)
        {
            if (startNowAndWait)
            {
                Task task = new Task(action);

                task.Start();
                task.Wait();
            }
            else
            {
                if (waitingActions == null)
                    waitingActions = new List<Action>();

                waitingActions.Add(action);
            }
        }

        private void SetTasksAndStart()
        {
            this.SetNotificationTask();
            this.RunPendingActions();
        }

        private void RunPendingActions()
        {
            if (this.hasWaitingActions)
            {
                Task task = new Task(waitingActions.First());

                task.ContinueWith(TaskEnd);
                task.Start();
                waitingActions.Remove(waitingActions.First());
            }
            else
            {
                this.Dispose();
            }
        }

        private void TaskEnd(Task previousTask)
        {
            previousTask.Dispose();
            RunPendingActions();
        }

        protected void AddNotification(string message, string toGroup = null, List<string> toUsers = null)
        {
            Notification notfication = new Notification(message, toUsers);
            notifications.Add(notfication);
        }

        private void SetNotificationTask()
        {
            if (notifications != null)
            {
                Action action = (async () =>
                {
                    try
                    {
                        foreach (var notification in this.notifications)
                        {
                            //save notifications here and call the notification engine in the use "signalR"
                        }

                        //this.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        this.HandleError(null, LookUps.ErrorType.Exception, ex);
                    }
                });

                this.AddAfterExecutionAction(action);
            }
        }

        protected void UndoUpdates()
        {
            dbContext = new LMYFrameWorkMVCEntities(true);
        }
    }
}
