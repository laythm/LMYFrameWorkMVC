using System.Collections.Generic;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;

namespace LMYFrameWorkMVC.BAL.Mappers.Admin.Administration
{
    public class DriverMapper
    {
        public static void Map(LMYFrameWorkMVCEntities dbContext, DriverModel src, Driver dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            if (!string.IsNullOrEmpty(src.StartDateString))
            {
                dest.StartDate = src.StartDateString.ConvertToDateTime();
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, Driver src, DriverModel dest)
        {
            if (src == null || dest == null)
                return;

            dest.CopyPropertyValues(src);

            dest.StartDateString = src.StartDate.ToStringDateFormat();

            foreach (Truck truck in src.Trucks)
            {
                TruckModel truckModel = new TruckModel();
                truckModel.CopyPropertyValues(truck);
                dest.TruckModels.Add(truckModel);
            }
        }

        public static void Map(LMYFrameWorkMVCEntities dbContext, List<Driver> src, List<DriverModel> dest)
        {
            if (src == null || dest == null)
                return;

            foreach (Driver driver in src)
            {
                DriverModel driverModel = new DriverModel();
                Map(dbContext, driver, driverModel);
                dest.Add(driverModel);
            }
        }
    }
}
