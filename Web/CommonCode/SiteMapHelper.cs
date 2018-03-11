using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Entities;
using LMYFrameWorkMVC.Common.Helpers;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Role;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Html.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LMYFrameWorkMVC.Web.CommonCode
{
    public static class SiteMapHelper
    {
        public static void PrepareSiteMapModel(SiteMapModel siteMapModel, string role = null, ISiteMapNode siteMapNode = null)
        {
            if (siteMapNode == null)
                siteMapNode = MvcSiteMapProvider.SiteMaps.GetSiteMap().RootNode;

            if (IsRolesEditable(siteMapNode))
            {
                siteMapModel.Title = siteMapNode.Title;
                siteMapModel.Key = siteMapNode.Key;
                siteMapModel.Selected = _GetNodeRoles(siteMapNode).Any(x => x == role);
            }

            foreach (ISiteMapNode tmpSiteMapNode in siteMapNode.ChildNodes)
            {
                SiteMapModel tmpSiteMapModel = new SiteMapModel();
                PrepareSiteMapModel(tmpSiteMapModel, role, tmpSiteMapNode);
                if (!string.IsNullOrEmpty(tmpSiteMapModel.Key))
                    siteMapModel.SiteMapModels.Add(tmpSiteMapModel);
            }
        }

        //default true
        public static bool ShowInSideBar(ISiteMapNode siteMapNode)
        {
            if (siteMapNode != null)
            {
                if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_ShowInSideBar, ObjectId = siteMapNode.Key }) is bool))
                    return (bool)CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_ShowInSideBar, ObjectId = siteMapNode.Key });

                if (!siteMapNode.Attributes.Any(x => x.Key == "showInSideBar"))
                {
                    return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_ShowInSideBar, ObjectId = siteMapNode.Key }, ShowInSideBar(siteMapNode.ParentNode));
                }
                else if (siteMapNode.Attributes.Any(x => x.Key == "showInSideBar" && x.Value.ToString().ToLower() == "false"))
                {
                    return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_ShowInSideBar, ObjectId = siteMapNode.Key }, false);
                }

                return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_ShowInSideBar, ObjectId = siteMapNode.Key }, true);
            }
            return true;
        }

        //default true
        public static bool IsRolesEditable(ISiteMapNode siteMapNode)
        {
            if (siteMapNode != null)
            {
                if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_IsRolesEditable, ObjectId = siteMapNode.Key }) is bool))
                    return (bool)CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_IsRolesEditable, ObjectId = siteMapNode.Key });

                if (!siteMapNode.Attributes.Any(x => x.Key == "isRolesEditable"))
                {
                    return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_IsRolesEditable, ObjectId = siteMapNode.Key }, IsRolesEditable(siteMapNode.ParentNode));
                }
                else if (siteMapNode.Attributes.Any(x => x.Key == "isRolesEditable" && x.Value.ToString().ToLower() == "false"))
                {
                    return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_IsRolesEditable, ObjectId = siteMapNode.Key }, false);
                }

                return (bool)CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_IsRolesEditable, ObjectId = siteMapNode.Key }, true);
            }
            return true;
        }

        //default false
        public static bool IsCurrentNode(ISiteMapNode siteMapNode)
        {
            if (siteMapNode.IsInCurrentPath())
                if (siteMapNode.Url == HttpContext.Current.Request.Url.ToString())
                    return true;

            return false;
        }

        //default ""
        public static string Class(ISiteMapNode siteMapNode)
        {
            if (siteMapNode != null)
            {
                if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Class, ObjectId = siteMapNode.Key }) is string))
                    return CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Class, ObjectId = siteMapNode.Key }).ToString();

                if (!siteMapNode.Attributes.Any(x => x.Key == "class"))
                {
                    return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Class, ObjectId = siteMapNode.Key }, Class(siteMapNode.ParentNode)).ToString();
                }
                else if (siteMapNode.Attributes.Any(x => x.Key == "class"))
                {
                    return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Class, ObjectId = siteMapNode.Key }, siteMapNode.Attributes.First(x => x.Key == "class").Value.ToString()).ToString();
                }

                return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_Class, ObjectId = siteMapNode.Key }, "").ToString();
            }

            return "";
        }

        //default false
        public static bool AllowAnnonAccess(ISiteMapNode siteMapNode)
        {
            if (siteMapNode != null)
            {
                if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_AllowAnnonAccess, ObjectId = siteMapNode.Key }) is bool))
                    return (bool)CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_AllowAnnonAccess, ObjectId = siteMapNode.Key });

                if (!siteMapNode.Attributes.Any(x => x.Key == "allowAnnonAccess"))
                {
                    return  Convert.ToBoolean(CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_AllowAnnonAccess, ObjectId = siteMapNode.Key }, AllowAnnonAccess(siteMapNode.ParentNode)));
                }
                else if (siteMapNode.Attributes.Any(x => x.Key == "allowAnnonAccess" && x.Value.ToString().ToLower() == "true"))
                {
                    return Convert.ToBoolean(CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_AllowAnnonAccess, ObjectId = siteMapNode.Key }, siteMapNode.Attributes.First(x => x.Key == "allowAnnonAccess").Value));
                }

                return Convert.ToBoolean(CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_AllowAnnonAccess, ObjectId = siteMapNode.Key }, false));
            }


            //if (siteMapNode != null)
            //{
            //    if (!siteMapNode.Attributes.Any(x => x.Key == "allowAnnonAccess"))
            //        return AllowAnnonAccess(siteMapNode.ParentNode);
            //    else if (siteMapNode.Attributes.Any(x => x.Key == "allowAnnonAccess" && x.Value.ToString().ToLower() == "true"))
            //        return true;
            //}

            return false;
        }

        private static bool _UserHasAccessToNode(IPrincipal principal, List<string> Roles)
        {
            if (Roles != null && Roles.Count() > 0)
            {
                //* allow access to all
                if (Roles.Any(x => x == "*"))
                    return true;

                ////? allow access for authenticated user
                //if (Roles.All(x => x == "?"))
                //    if (principal != null && principal.Identity.IsAuthenticated)
                //        return true;
                //    else
                //        return false;

                if (principal == null)
                    return false;

                foreach (string role in Roles)
                {
                    if (principal.IsInRole(role))
                        return true;
                }
            }

            return false;
        }

        private static List<string> _GetNodeRoles(ISiteMapNode siteMapNode)
        {
            if (siteMapNode != null)
            {
                if ((CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_NodeRoles, ObjectId = siteMapNode.Key }) is List<string>))
                    return CacheHelper.GetValue(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_NodeRoles, ObjectId = siteMapNode.Key }) as List<string>;

                List<string> rolesList;
                // rolesList = new List<string>() { "*" };
                if (!SiteMapHelper.AllowAnnonAccess(siteMapNode) && IsRolesEditable(siteMapNode))
                    rolesList = LMYFrameWorkMVC.Common.Helpers.Utilites.GetNodeRolesByKey(siteMapNode.Key);
                else if (!SiteMapHelper.AllowAnnonAccess(siteMapNode) && !SiteMapHelper.IsRolesEditable(siteMapNode))
                    rolesList = _GetNodeRoles(siteMapNode.ParentNode);
                else if (SiteMapHelper.AllowAnnonAccess(siteMapNode))
                    rolesList = new List<string>() { "*" };
                else
                    rolesList = new List<string>() { "?" };

                List<CacheMemberKey> dependOnObjects = new List<CacheMemberKey>() { new CacheMemberKey { CacheKey = LookUps.CacheKeys.Roles, ObjectId = LookUps.CacheKeys.Roles.ToString() } };

                //this roles list depend on the roles if roles are changed 
                return CacheHelper.Insert(new CacheMemberKey() { CacheKey = LookUps.CacheKeys.SiteMap_NodeRoles, ObjectId = siteMapNode.Key }, rolesList, dependOnObjects) as List<string>;
            }

            return null;
        }

        public static bool HasAccessToNode(this IPrincipal principal, ISiteMapNode siteMapNode)
        {
            return _UserHasAccessToNode(principal, _GetNodeRoles(siteMapNode));
        }

        public static bool HasAccessToNode(this IPrincipal principal, SiteMapNodeModel siteMapNodeModel)
        {
            return _UserHasAccessToNode(principal, _GetNodeRoles(MvcSiteMapProvider.SiteMaps.Current.FindSiteMapNodeFromKey(siteMapNodeModel.Key)));
        }

        public static bool HasAccessToNode(this IPrincipal principal, string url)
        {
            return _UserHasAccessToNode(principal, _GetNodeRoles(MvcSiteMapProvider.SiteMaps.Current.FindSiteMapNode(url)));
        }

        public static List<string> GetAuthorizedNodesList(this IPrincipal principal, ISiteMapNode siteMapNode, bool all = true)
        {
            List<string> lst = new List<string>();

            if (siteMapNode != null)
            {
                if (siteMapNode.Clickable && (all || IsRolesEditable(siteMapNode)) && HasAccessToNode(principal, siteMapNode))
                    lst.Add(siteMapNode.Key);

                foreach (ISiteMapNode childNode in siteMapNode.ChildNodes)
                {
                    lst.AddRange(GetAuthorizedNodesList(principal, childNode, all));
                }
            }

            return lst;
        }

        public static string GetUrlByKey(string key)
        {
            return MvcSiteMapProvider.SiteMaps.Current.FindSiteMapNodeFromKey(key).Url;
        }
    }
}