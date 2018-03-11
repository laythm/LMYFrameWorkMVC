using LMYFrameWorkMVC.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.User;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class UserMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, UserModel src, AspNetUser dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src, new List<string>() { src.nameof(x => x.CreatedAt) });

            dest.AspNetRoles.Clear();
            dest.AspNetRoles = src.RolesIDs.Select(x => dbContext.AspNetRoles.Find(x)).ToList();
            dest.AspNetUser_Connections = src.AspNetUserConnectionModelList.Select(x => dbContext.AspNetUser_Connections.Find(x.ID)).ToList();
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, AspNetUser src, UserModel dest)
        {
            if (src == null || dest == null)
                return;

            //dest.Id = src.Id;
            //dest.PhoneNumber = src.PhoneNumber;
            //dest.UserName = src.UserName;
            //dest.ArabicName = src.ArabicName;
            //dest.EnglishName = src.EnglishName;
            //dest.Email = src.Email;
            //dest.CreatedAt = src.CreatedAt;
            dest.CopyPropertyValues(src);
            RoleMapper.Map(src.AspNetRoles.ToList(), dest.AspNetRolesListModel);

            if (dest.AspNetRolesListModel != null)
                dest.RolesIDs = dest.AspNetRolesListModel.Select(x => x.Id).ToList();

            if (dest.AspNetUserConnectionModelList != null)
                Map(dbContext, src.AspNetUser_Connections.ToList(), dest.AspNetUserConnectionModelList);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<AspNetUser> src, List<UserModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (AspNetUser aspNetUser in src)
            {
                UserModel userModel = new UserModel();
                Map(dbContext, aspNetUser, userModel);
                dest.Add(userModel);
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, AspNetUser_Connections src, AspNetUserConnectionModel dest)
        {
            dest.CopyPropertyValues(src);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, AspNetUserConnectionModel src, AspNetUser_Connections dest)
        {
            dest.CopyPropertyValues(src);
            dest.AspNetUser = dbContext.AspNetUsers.Find(src.UserId);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<AspNetUser_Connections> src, List<AspNetUserConnectionModel> dest)
        {
            foreach (AspNetUser_Connections aspNetUser_Connections in src)
            {
                AspNetUserConnectionModel aspNetUserConnectionModel = new AspNetUserConnectionModel();

                Map(dbContext, aspNetUser_Connections, aspNetUserConnectionModel);
                dest.Add(aspNetUserConnectionModel);
            }
        }
    }
}
