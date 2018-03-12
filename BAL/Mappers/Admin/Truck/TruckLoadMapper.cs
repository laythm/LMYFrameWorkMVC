using LMYFrameWorkMVC.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.Load;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class TruckLoadMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, TruckLoadModel src, TruckLoad dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            if (!string.IsNullOrEmpty(src.FromDateString))
            {
                dest.FromDate = src.FromDateString.ConvertToDateTime();
            }

            if (!string.IsNullOrEmpty(src.ToDateString))
            {
                dest.ToDate = src.ToDateString.ConvertToDateTime();
            }

            dest.TotalIncome = dest.Price;
            dest.TotalCosts = dest.DriverCost + dest.FuelCost + dest.OtherCosts;
            dest.TotalProfit = dest.TotalIncome - dest.TotalCosts;
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, TruckLoad src, TruckLoadModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            dest.ToDateString = src.ToDate.ToStringDateFormat();
            dest.FromDateString = src.FromDate.ToStringDateFormat();

            if (src.Truck != null)
            {
                dest.TruckModel.CopyPropertyValues(src.Truck);
            }

            if (src.Driver != null)
            {
                dest.DriverModel.CopyPropertyValues(src.Driver);
            }

            if (src.Company != null)
            {
                dest.CompanyModel.CopyPropertyValues(src.Company);
            }

            if (src.TruckLoadStatus != null)
            {
                dest.StatusModel.CopyPropertyValues(src.TruckLoadStatus);
            }

            if (src.USAState != null)
            {
                dest.FromStateModel.CopyPropertyValues(src.USAState);
            }

            if (src.USAState1 != null)
            {
                dest.ToStateModel.CopyPropertyValues(src.USAState1);
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<TruckLoad> src, List<TruckLoadModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (TruckLoad truckLoad in src)
            {
                TruckLoadModel truckLoadModel = new TruckLoadModel();
                Map(dbContext, truckLoad, truckLoadModel);
                dest.Add(truckLoadModel);
            }
        }
    }
}
