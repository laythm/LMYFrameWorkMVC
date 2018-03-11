using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Globalization;
using LMYFrameWorkMVC.Common.Models;
using LMYFrameWorkMVC.Web.CommonCode;
using System.Threading;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using LMYFrameWorkMVC.BAL.Modules.Admin.Message;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Web.Areas.Common.Controllers;

namespace LMYFrameWorkMVC.Web.Areas.Admin.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class MessageController : BaseController
    {
        // GET: User
        public ActionResult Index(BaseModel baseModel)
        {
             
            return View( );
        }

        public JsonResult ListUsersWithLastMessageByCurrentUser()
        {
            //Thread.Sleep(10000) ;
            GenericListModel<UserModel> baseListModel = new GenericListModel<UserModel>();

            using (MessageBAL userBAL = new MessageBAL(ContextInfo))
            {
                baseListModel = userBAL.ListUsersWithLastMessageByCurrentUser();
            }

            return Json(new
            {
                baseModel = baseListModel,
                NotReadCount = baseListModel.List.Where(x => x.LastMessageModel.NotFromMeAndNotRead).Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMyMessagesByOtherUser(string userID, int page, int pageSize)
        {
            GenericListModel<MessageModel> baseListModel = new GenericListModel<MessageModel>();

            using (MessageBAL userBAL = new MessageBAL(ContextInfo))
            {
                baseListModel = userBAL.GetMyMessagesByOtherUser(userID, page, pageSize);
            }

            return Json(new
            {
                baseModel = baseListModel
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendMessage(string toUserID, string messageText)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.ToUserID = toUserID;
            messageModel.MessageText = messageText;
            messageModel.FromUserID = ContextInfo.UserID;

            using (MessageBAL messageBAL = new MessageBAL(ContextInfo))
            {
                messageBAL.Create(messageModel);

                messageBAL.GetMessageModel(messageModel, ContextInfo.UserID);
            }

            return Json(new
            {
                baseModel = messageModel
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: User/Create
        public ActionResult Create(BaseModel baseModel)
        {
            MessageModel messageModel = new MessageModel();
            messageModel.CopyBaseModel(baseModel);

            using (MessageBAL messageBAL = new MessageBAL(ContextInfo))
            {
                messageBAL.PrepareMessageModel(messageModel);
            }

            return View(messageModel);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MessageModel messageModel)
        {
            messageModel.FromUserID = ContextInfo.UserID;

            ModelState.Remove(messageModel.nameof(x => x.FromUserID));
            if (ModelState.IsValid)
            {
                messageModel.AddSuccess(LMYFrameWorkMVC.Common.Resources.Resources.MessageSent, LMYFrameWorkMVC.Common.LookUps.SuccessType.Full);
            }

            return View(messageModel);
        }
    }
}
