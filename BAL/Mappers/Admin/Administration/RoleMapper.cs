using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class RoleMapper
    {
        private static void perpareNodeKeys(string roleID, SiteMapModel siteMapModel, ICollection<AspNetRoleSiteMapNode> AspNetRole_NodesKeys, string parentID = null)
        {
            if (siteMapModel.Selected)
            {
                string id = Guid.NewGuid().ToString();
                AspNetRole_NodesKeys.Add(new AspNetRoleSiteMapNode() { ID = id, NodeKey = siteMapModel.Key, AspNetRoleID = roleID, ParentID = parentID });

                foreach (SiteMapModel tmpSiteMapModel in siteMapModel.SiteMapModels)
                {
                    perpareNodeKeys(roleID, tmpSiteMapModel, AspNetRole_NodesKeys, id);
                }
            }
        }

        public static void Map(RoleModel src, AspNetRole dest)
        {
            if (src == null || dest == null)
                return;

            dest.AspNetRoleSiteMapNodes.Clear();
            perpareNodeKeys(src.Id, src.SiteMapModel, dest.AspNetRoleSiteMapNodes);

            dest.CopyPropertyValues(src);
        }

        public static void Map(AspNetRole src, RoleModel dest)
        {
            if (src == null || dest == null)
                return;

            //dest.Id = src.Id;
            //dest.Key = src.Key;
            //dest.Name = src.Name;
            dest.CopyPropertyValues(src);

        }

        public static void Map(List<AspNetRole> src, List<RoleModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (AspNetRole aspNetRole in src)
            {
                RoleModel roleModel = new RoleModel();
                Map(aspNetRole, roleModel);
                dest.Add(roleModel);
            }
        }
    }
}
