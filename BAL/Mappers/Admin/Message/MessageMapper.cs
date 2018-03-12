using LMYFrameWorkMVC.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Message
{
    public class MessageMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, MessageModel src, LMYFrameWorkMVC.Common.DAL.Message dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src, new List<string>() { src.nameof(x => x.CreatedAt) });
            //the below code is important for audittrail and other matters such as get message after add it directly so the navigation properties be available 
            dest.AspNetUser = dbContext.AspNetUsers.Where(x => x.Id == src.FromUserID).FirstOrDefault();
            dest.AspNetUser1 = dbContext.AspNetUsers.Where(x => x.Id == src.ToUserID).FirstOrDefault();
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, string fromUserId, LMYFrameWorkMVC.Common.DAL.Message src, MessageModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
            UserMapper.Map(dbContext, src.AspNetUser, dest.FromUserModel);
            UserMapper.Map(dbContext, src.AspNetUser1, dest.ToUserModel);
            //dest.ToUsersListItems = dbContext.AspNetUsers.Select(x => new UserModel { Id = x.Id, EnglishName = x.EnglishName, ArabicName = x.ArabicName,ro }).ToList().Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = dest.ToUsersIDs.Any(y => y == x.Id) }).ToList();
            dest.IsFromMe = fromUserId == dest.FromUserID;
            dest.NotFromMeAndNotRead = !dest.IsFromMe && !dest.Read;
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, string fromUserId, List<LMYFrameWorkMVC.Common.DAL.Message> src, List<MessageModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (LMYFrameWorkMVC.Common.DAL.Message chatMessage in src)
            {
                MessageModel messageModel = new MessageModel();
                Map(dbContext, fromUserId, chatMessage, messageModel);
                dest.Add(messageModel);
            }
        }
    }
}
