using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMYFrameWorkMVC.Common;
using System.Data.Entity.Infrastructure;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Entities;
namespace LMYFrameWorkMVC.Common.DAL
{
    public partial class AspNetUser : IAuditedDatesEntity, IAuditedUsersEntity, IAuditWrapper
    {
        public string GetJSONData()
        {
            AspNetUser tmpAspNetUser = new AspNetUser();
            tmpAspNetUser.CopyPropertyValues(this);
            //to solve the issue of dynamic proxy which load huge data from database
            tmpAspNetUser.AspNetRoles = new List<AspNetRole>();
            //to solve the issue of dynamic proxy which load huge data from database
            foreach (AspNetRole aspNetRole in this.AspNetRoles)
            {
                AspNetRole tmpAspNetRole = new AspNetRole();
                tmpAspNetRole.CopyPropertyValues(aspNetRole);
                tmpAspNetUser.AspNetRoles.Add(tmpAspNetRole);
            }

            foreach (AspNetUser_Connections aspNetUser_Connections in this.AspNetUser_Connections)
            {
                AspNetUser_Connections tmpAspNetUser_Connections = new AspNetUser_Connections();
                tmpAspNetUser_Connections.CopyPropertyValues(aspNetUser_Connections);
                tmpAspNetUser_Connections.AspNetUser = null;
                tmpAspNetUser.AspNetUser_Connections.Add(tmpAspNetUser_Connections);
            }

           // string json = JsonConvert.SerializeObject(tmpAspNetUser);

            tmpAspNetUser = null;

            return "";
        }

        public string GetPrimaryKey()
        {
            return this.Id;
        }
 
    }
}
