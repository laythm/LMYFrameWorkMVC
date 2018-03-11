using System.Collections.Generic;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class CompanyMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, CompanyModel src, Company dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, Company src, CompanyModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<Company> src, List<CompanyModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (Company company in src)
            {
                CompanyModel companyModel = new CompanyModel();
                Map(dbContext, company, companyModel);
                dest.Add(companyModel);
            }
        }
    }
}
