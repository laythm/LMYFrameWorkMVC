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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.TruckExpense;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class TruckExpenseMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, TruckExpenseModel src, TruckExpens dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            if (!string.IsNullOrEmpty(src.AtDateString))
            {
                dest.AtDate = src.AtDateString.ConvertToDateTime();
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, TruckExpens src, TruckExpenseModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);
            dest.AtDateString = src.AtDate.ToStringDateFormat();

            if (src.TruckExpensType!=null)
            {
                dest.ExpenseTypeModel.CopyPropertyValues(src.TruckExpensType);
            }

            if (src.Truck != null)
            {
                dest.TruckModel.CopyPropertyValues(src.Truck);
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<TruckExpens> src, List<TruckExpenseModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (TruckExpens truckExpens in src)
            {
                TruckExpenseModel truckExpenseModel = new TruckExpenseModel();
                Map(dbContext, truckExpens, truckExpenseModel);
                dest.Add(truckExpenseModel);
            }
        }
    }
}
