using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using LMYFrameWorkMVC.BAL.Modules.Admin.Administration;
using LMYFrameWorkMVC.Web.Areas.Admin.Controllers;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using LMYFrameWorkMVC.BAL.Modules.Admin.Message;
using Microsoft.AspNet.SignalR.Transports;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Extensions;

namespace LMYFrameWorkMVC.Web.CommonCode.SignalR
{
    public class AppHub : Hub
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<AppHub>();

        protected ContextInfo ContextInfo { get; set; }

        public static void Init()
        {
            GlobalHost.HubPipeline.AddModule(new LMYFrameWorkMVC.Web.CommonCode.SignalR.MyHubPipelineModule());

            //this function to check if there is user saved is online but it is not connected
            //this case happens mostly when iis stops suddnly
            ITransportHeartbeat heartBeat = GlobalHost.DependencyResolver.Resolve<ITransportHeartbeat>();

            using (UserBAL userBAL = new UserBAL(new ContextInfo()))
            {
                userBAL.SetOfflineDisconnectedUsers(heartBeat.GetConnections().Where(x => x.IsAlive).Select(x => x.ConnectionId).ToList());
            }
        }

        private AspNetUserConnectionModel GetAspNetUserConnectionModel(HttpBrowserCapabilitiesBase httpBrowserCapabilitiesBase, string userId, string connectionID, string ip,string sessionID)
        {
            AspNetUserConnectionModel aspNetUserConnectionModel = new AspNetUserConnectionModel();
            aspNetUserConnectionModel.UserId = userId;
            aspNetUserConnectionModel.ConnectionID = connectionID;
            aspNetUserConnectionModel.IP = ip;
            aspNetUserConnectionModel.CopyPropertyValues(httpBrowserCapabilitiesBase);
            aspNetUserConnectionModel.SessionID = sessionID;

            return aspNetUserConnectionModel;
        }

        public override Task OnConnected()
        {
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);
 
            string id = Context.ConnectionId;
            if (ContextInfo.IsUserAuthenticated)
            {
                try
                {
                    using (UserBAL userBAL = new UserBAL(ContextInfo))
                    {
                        //log out from all other pcs if this option is enabled
                        if (!LMYFrameWorkMVC.Common.Helpers.Utilites.GetBoolSettingValue(LookUps.SettingsKeys.AllowSimultaneousUserLogin))
                        {
                            UserModel userModel = new UserModel();
                            userModel.Id = ContextInfo.UserID;
                            userBAL.GetUserModel(userModel);

                            if (!userModel.HasErrorByType(LookUps.ErrorType.Critical))
                            {
                                //here i use the session because the user can open the system in browser and open many taps
                                //since all taps has one SessionID meanwhile each tap has it own signalr connection id
                                //each browser has its own sessionid even if the session ended on session start again will generate the same id
                                foreach (AspNetUserConnectionModel aspNetUserConnectionModel in userModel.AspNetUserConnectionModelList.Where(x => x.SessionID != ContextInfo.SessionID))
                                {
                                    hubContext.Clients.Client(aspNetUserConnectionModel.ConnectionID).AutomaticLogOut();
                                }
                            }
                        }

                        userBAL.addOrUpdateConnection(GetAspNetUserConnectionModel(Context.Request.GetHttpContext().Request.Browser, ContextInfo.UserID, id, Context.Request.GetHttpContext().Request.UserHostAddress, ContextInfo.SessionID));
                    }
                }
                catch (Exception ex)
                {

                }

                Clients.Client(id).connected(true);
                Clients.All.userIsOnline(ContextInfo.UserID, true);
            }

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            //The OnReconnected event handler in a SignalR Hub can execute directly after OnConnected but not after OnDisconnected for a given client. The reason you can have a reconnection without a disconnection is that there are several ways in which the word "connection" is used in SignalR. 
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);

            string id = Context.ConnectionId;
            if (ContextInfo.IsUserAuthenticated)
            {
                using (UserBAL userBAL = new UserBAL(ContextInfo))
                {
                    userBAL.addOrUpdateConnection(GetAspNetUserConnectionModel(Context.Request.GetHttpContext().Request.Browser, ContextInfo.UserID, id, Context.Request.GetHttpContext().Request.UserHostAddress, ContextInfo.SessionID));
                }
            }

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string id = Context.ConnectionId;
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);

            UserModel userModel = new UserModel();

            if (ContextInfo.IsUserAuthenticated)
            {
                using (UserBAL userBAL = new UserBAL(ContextInfo))
                {
                    userModel.Id = ContextInfo.UserID;
                    userBAL.removeConnection(id);
                    userBAL.GetUserModel(userModel);
                }
                if (userModel.IsOnline == false)
                {
                    Clients.Client(id).disconnected(true);
                    Clients.All.userIsOnline(ContextInfo.UserID, false);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        public MessageModel SendMessage(string toUserID, string messageText)
        {
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);

            if (ContextInfo.IsUserAuthenticated)
            {
                MessageModel messageModelFrom = new MessageModel();
                MessageModel messageModelTo = new MessageModel();

                messageModelFrom.ToUserID = toUserID;
                messageModelFrom.MessageText = messageText;
                messageModelFrom.FromUserID = ContextInfo.UserID;

                using (MessageBAL messageBAL = new MessageBAL(ContextInfo))
                {
                    messageBAL.Create(messageModelFrom);
                    messageModelTo.ID = messageModelFrom.ID;
                    messageModelFrom.SuccessesList.Clear();

                    messageBAL.GetMessageModel(messageModelFrom, messageModelFrom.FromUserID);
                    messageBAL.GetMessageModel(messageModelTo, messageModelTo.ToUserID);

                    //notify the sender that the message sent successfully
                    foreach (AspNetUserConnectionModel aspNetUserConnectionModel in messageModelFrom.FromUserModel.AspNetUserConnectionModelList)
                    {
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).messageSentSuccessfully(messageModelFrom);
                    }

                    //notify the reciever that new message sent to him
                    foreach (AspNetUserConnectionModel aspNetUserConnectionModel in messageModelTo.ToUserModel.AspNetUserConnectionModelList)
                    {
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).recieveMessage(messageModelTo);
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).updateNotReadCount(messageBAL.GetNotFromMeAndNotReadCountByUserID(messageModelTo.ToUserID));
                    }
                }

                //this will return to from user
                return messageModelFrom;
            }
            return null;
        }

        public void SetMessageAsRead(string id)
        {
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);

            if (ContextInfo.IsUserAuthenticated)
            {
                MessageModel messageModel = new MessageModel( );//from ;; this who will recieve read update
                // MessageModel messageModelTo = new MessageModel();//to ; this how read the message

                using (MessageBAL messageBAL = new MessageBAL(ContextInfo))
                {
                    messageBAL.SetMessageAsRead(id);
                    messageModel.ID = id;

                    messageBAL.GetMessageModel(messageModel, ContextInfo.UserID);
                    //  messageBAL.GetMessageModel(messageModelFrom, messageModelTo.FromUserID);

                    //notify the message sender that his message is read by the reader
                    foreach (AspNetUserConnectionModel aspNetUserConnectionModel in messageModel.FromUserModel.AspNetUserConnectionModelList)
                    {
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).ReadMessageUpdated(id);
                    }

                    //notify the reader that the message set as read successfully
                    foreach (AspNetUserConnectionModel aspNetUserConnectionModel in messageModel.ToUserModel.AspNetUserConnectionModelList)
                    {
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).messageSetAsReadSuccessfully(id);
                        Clients.Client(aspNetUserConnectionModel.ConnectionID).updateNotReadCount(messageBAL.GetNotFromMeAndNotReadCountByUserID(messageModel.ToUserID));
                    }
                }
            }
        }

        public void FireUpdateNotReadCountForCurrentConnection()
        {
            if (ContextInfo == null)
                ContextInfo = ContextInfoInitilizer.GetContextInfo(Context.Request.GetHttpContext(), Context.User, Context.QueryString["SessionID"]);

            using (MessageBAL messageBAL = new MessageBAL(ContextInfo))
            {
                Clients.Client(Context.ConnectionId).updateNotReadCount(messageBAL.GetNotFromMeAndNotReadCountByUserID(ContextInfo.UserID));
            }
        }
        ///my methods
    }
}