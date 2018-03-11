using System.Collections.Generic;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.USAState;
using LMYFrameWorkMVC.Common.DAL;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class USAStateMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, USAStateModel src, USAState dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, USAState src, USAStateModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<USAState> src, List<USAStateModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (USAState uSAState in src)
            {
                USAStateModel uSAStateModel = new USAStateModel();
                Map(dbContext, uSAState, uSAStateModel);
                dest.Add(uSAStateModel);
            }
        }
    }
}
