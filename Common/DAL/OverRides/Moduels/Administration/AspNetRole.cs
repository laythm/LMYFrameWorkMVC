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
    public partial class AspNetRole : IAuditedDatesEntity, IAuditedUsersEntity, IAuditWrapper,  ICacheClear
    {
        public string GetJSONData()
        {
            AspNetRole tmpAspNetRole = new AspNetRole();
            tmpAspNetRole.CopyPropertyValues(this);

            //tmpAspNetRole.AspNetRole_SiteMapNodes = new List<AspNetRole_NodesKeys>();
            ////to solve the issue of dynamic proxy which load huge data from database
            //foreach (AspNetRole_NodesKeys aspNetRole_NodesKeys in this.AspNetRole_NodesKeys)
            //{
            //    AspNetRole_NodesKeys tmpAspNetRole_NodesKeys = new AspNetRole_NodesKeys();

            //    tmpAspNetRole_NodesKeys.CopyPropertyValues(aspNetRole_NodesKeys, new List<string>() { tmpAspNetRole_NodesKeys.nameof(x => x.AspNetRole), tmpAspNetRole_NodesKeys.nameof(x => x.AspNetRole_NodesKeys1), tmpAspNetRole_NodesKeys.nameof(x => x.AspNetRole_NodesKeys2) });
            //    tmpAspNetRole.AspNetRole_NodesKeys.Add(tmpAspNetRole_NodesKeys);
            //}

            //string json = JsonConvert.SerializeObject(tmpAspNetRole);

            //tmpAspNetRole = null;

            // return json;

            return "";
        }

        public string GetPrimaryKey()
        {
            return this.Id;
        }

        public void SetDefaultValues()
        {
            this.IsDeleteAble = true;
        }

        public void ClearCache()
        {
            Common.Helpers.CacheHelper.RemoveCache(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.Roles, ObjectId = this.Id });
        }
    }
}
