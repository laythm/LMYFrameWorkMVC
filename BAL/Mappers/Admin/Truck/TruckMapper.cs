using LMYFrameWorkMVC.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class TruckMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, TruckModel src, Truck dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            if (!string.IsNullOrEmpty(src.OnBuyDateString))
            {
                dest.OnBuyDate = src.OnBuyDateString.ConvertToDateTime();
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, Truck src, TruckModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            dest.OnBuyDateString = src.OnBuyDate.ToStringDateFormat();

            if (src.Driver != null)
            {
                dest.DriverModel.CopyPropertyValues(src.Driver);
            }

            Nullable<double> truckExpensesCosts = src.TruckExpenses.Where(x => x.Price != null).Sum(x => x.Price);
            Nullable<double> truckLoadsIncome = src.TruckLoads.Where(x => x.TotalIncome != null && x.StatusId == Lookups.TruckLoadStatus.Completed).Sum(x => x.TotalIncome);
            dest.TotalCosts = src.OnBuyExpenses + (truckExpensesCosts == null ? 0 : (double)truckExpensesCosts);
            dest.TotalIncome = (truckLoadsIncome == null ? 0 : (double)truckLoadsIncome);
            dest.TotalProfit = dest.TotalIncome - dest.TotalCosts;
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<Truck> src, List<TruckModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (Truck truck in src)
            {
                TruckModel truckModel = new TruckModel();
                Map(dbContext, truck, truckModel);
                dest.Add(truckModel);
            }
        }
    }
}
