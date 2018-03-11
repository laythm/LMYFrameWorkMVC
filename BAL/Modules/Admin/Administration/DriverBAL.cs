using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Common;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class DriverBAL : BaseBAL
    {
        public DriverBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private IQueryable<Driver> prepareSearch(string value)
        {
            IQueryable<Driver> drivers = dbContext.Drivers.OrderBy(x => x.CreatedAt);

            if (!string.IsNullOrEmpty(value))
            {

                drivers = drivers.Where(x =>
                           x.Name.ToLower().Contains(value.ToLower()) ||
                           x.ContactNumber.ToLower().Contains(value.ToLower()) ||
                           (
                               !x.StartDate.HasValue || x.StartDate.Value.ToString().ToLower().Contains(value.ToLower())
                           ) ||
                           x.Notes.ToLower().Contains(value.ToLower())
                        );
            }

            return drivers;
        }

        private bool Validate(DriverModel driverModel)
        {
            if (dbContext.Drivers.Any(x => x.Name == driverModel.Name && x.Id != driverModel.Id.ToString()))
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.AlreadyExists, driverModel.GetDisplayName(x => x.Name)), driverModel.nameof(x => x.Name));

            return driverModel.HasErrorByType();
        }

        private bool ValidateDelete(DriverModel driverModel)
        {
            return driverModel.HasErrorByType();
        }

        public void PrepareDriverModel(DriverModel driverModel)
        {
            try
            {
            }
            catch (Exception ex)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetDriverModel(DriverModel driverModel)
        {
            try
            {
                Driver driver = dbContext.Drivers.Where(x => x.Id == driverModel.Id).FirstOrDefault();

                if (driver == null)
                {
                    base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    DriverMapper.Map(dbContext, driver, driverModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

        }

        public GenericListModel<DriverModel> GetSearchDriversList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<DriverModel> baseListModel = new GenericListModel<DriverModel>();

            try
            {
                //if (!base.DriverHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<Driver> drivers = prepareSearch(dataTableSearchParameters.search.value);
     
                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                drivers = drivers.OrderBy(c => c.Name);
                            else
                                drivers = drivers.OrderByDescending(c => c.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                drivers = drivers.OrderBy(c => c.StartDate);
                            else
                                drivers = drivers.OrderByDescending(c => c.StartDate);
                            break;
                        case 2:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                drivers = drivers.OrderBy(c => c.ContactNumber);
                            else
                                drivers = drivers.OrderByDescending(c => c.ContactNumber);
                            break;
                    }
                }

                baseListModel.Total = drivers.Count();
                drivers = drivers.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    drivers = drivers.Take(dataTableSearchParameters.length);

                DriverMapper.Map(dbContext, drivers.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(DriverModel driverModel)
        {
            try
            {
                if (Validate(driverModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Driver driver = new Driver();
                        DriverMapper.Map(dbContext, driverModel, driver);

                        driver.Id = Guid.NewGuid().ToString();

                        dbContext.Drivers.Add(driver);

                        base.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    driverModel.AddSuccess(Resources.DriverAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(DriverModel driverModel)
        {
            try
            {
                Driver driver = dbContext.Drivers.Where(x => x.Id == driverModel.Id).FirstOrDefault();

                if (driver == null)
                {
                    base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(driverModel))
                    return;

                DriverMapper.Map(dbContext, driverModel, driver);

                base.SaveChanges();

                driverModel.AddSuccess(Resources.DriverUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(DriverModel driverModel)
        {
            try
            {
                if (ValidateDelete(driverModel))
                    return;

                Driver driver = dbContext.Drivers.Where(x => x.Id == driverModel.Id).FirstOrDefault();

                if (driver == null)
                {
                    base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.Drivers.Remove(driver);

                base.SaveChanges();

                driverModel.AddSuccess(Resources.DriverDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(driverModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public GenericListModel<DriverModel> GetDrivers(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<DriverModel> baseListModel = new GenericListModel<DriverModel>();

            try
            {
                IQueryable<Driver> drivers = prepareSearch(select2Parameters.text);

           
                drivers = drivers.OrderBy(x => x.CreatedAt);

                baseListModel.Total = drivers.Count();
                drivers = drivers.Skip(select2Parameters.start);
                drivers = drivers.Take(select2Parameters.pageSize);

               DriverMapper.Map(dbContext, drivers.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }
    }
}
