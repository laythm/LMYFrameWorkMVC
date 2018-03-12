using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.Load;
using System.Web.Mvc;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Load
{
    public class TruckLoadBAL : BaseBAL
    {
        public TruckLoadBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(TruckLoadModel truckLoadModel)
        {
            if (dbContext.TruckLoads.Any(x =>
            x.TruckId == truckLoadModel.TruckId &&
            x.Id != truckLoadModel.Id.ToString() &&
                (
                    x.FromDate.HasValue && x.ToDate.HasValue &&
                    truckLoadModel.FromDate.HasValue &&
                    !(truckLoadModel.FromDate.Value > x.ToDate.Value || truckLoadModel.FromDate.Value < x.FromDate.Value)
                ) &&
                (
                    x.FromDate.HasValue && x.ToDate.HasValue &&
                    truckLoadModel.ToDate.HasValue &&
                    !(truckLoadModel.ToDate.Value > x.ToDate.Value || truckLoadModel.ToDate.Value < x.FromDate.Value)
                )
            ))
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.DurationIsOverLappedWithOtherLoad, truckLoadModel.GetDisplayName(x => x.Name)), truckLoadModel.nameof(x => x.Name));

            return truckLoadModel.HasErrorByType();
        }

        private bool ValidateDelete(TruckLoadModel truckLoadModel)
        {
            return truckLoadModel.HasErrorByType();
        }

        public void PrepareTruckLoadModel(TruckLoadModel truckLoadModel)
        {
            try
            {
                truckLoadModel.StatesList = dbContext.USAStates.
                  Select(x => new
                  {
                      x.Id,
                      x.Name,
                  }).ToList().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

                truckLoadModel.StatusesList = dbContext.TruckLoadStatuses.
                Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();


                //IF THIS LOAD IS NEW THEN SET THE TRUCK DRIVER AS THIS LOAD DRIVER 
                if (!dbContext.TruckLoads.Any(x => x.Id == truckLoadModel.Id))
                {
                    Common.DAL.Driver driver = dbContext.Drivers.Where(x => x.Id == truckLoadModel.TruckId).FirstOrDefault();

                    if (driver != null)
                        truckLoadModel.DriverModel.CopyPropertyValues(driver);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void PrepareTruckLoadSearchModel(TruckLoadSearchModel truckLoadSearchModel)
        {
            try
            {
                truckLoadSearchModel.StatusesList = dbContext.TruckLoadStatuses.
                Select(x => new
                {
                    x.Id,
                    x.Name,
                }).ToList().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadSearchModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }
        public void GetTruckLoadModel(TruckLoadModel truckLoadModel)
        {
            try
            {
                Common.DAL.TruckLoad load = dbContext.TruckLoads.Where(x => x.Id == truckLoadModel.Id).FirstOrDefault();

                if (load == null)
                {
                    base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    TruckLoadMapper.Map(dbContext, load, truckLoadModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<TruckLoadModel> GetSearchTrucksLoadsList(DataTableSearchParameters<TruckLoadSearchModel> dataTableSearchParameters)
        {
            GenericListModel<TruckLoadModel> baseListModel = new GenericListModel<TruckLoadModel>();

            try
            {
                //if (!base.LoadHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<Common.DAL.TruckLoad> loads = dbContext.TruckLoads.OrderBy(x => x.CreatedAt);

                if (dataTableSearchParameters.CustomSearchObject != null)
                {
                    if (!string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.FromDateString))
                    {
                        dataTableSearchParameters.CustomSearchObject.FromDate = dataTableSearchParameters.CustomSearchObject.FromDateString.ConvertToDateTime();
                    }
                    if (!string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.ToDateString))
                    {
                        dataTableSearchParameters.CustomSearchObject.ToDate = dataTableSearchParameters.CustomSearchObject.ToDateString.ConvertToDateTime();
                    }

                    loads = loads.Where(x =>
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.TruckId) || x.TruckId == dataTableSearchParameters.CustomSearchObject.TruckId) &&
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.DriverId) || x.DriverId == dataTableSearchParameters.CustomSearchObject.DriverId) &&
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.CompanyId) || x.DriverId == dataTableSearchParameters.CustomSearchObject.CompanyId) &&
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.StatusId) || x.DriverId == dataTableSearchParameters.CustomSearchObject.StatusId) &&
                            (dataTableSearchParameters.CustomSearchObject.FromDate == null ||( x.FromDate.HasValue && x.FromDate.Value>= dataTableSearchParameters.CustomSearchObject.FromDate.Value)) &&
                            (dataTableSearchParameters.CustomSearchObject.ToDate == null || (x.ToDate.HasValue && x.ToDate.Value <= dataTableSearchParameters.CustomSearchObject.ToDate.Value))  
                    );
                }

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    loads = loads.Where(x =>
                        (x.TruckId == null || string.IsNullOrEmpty(x.Truck.Name) || x.Truck.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.DriverId == null || string.IsNullOrEmpty(x.Driver.Name) || x.Driver.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.CompanyId == null || string.IsNullOrEmpty(x.Company.Name) || x.Company.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.StatusId == null || string.IsNullOrEmpty(x.TruckLoadStatus.Name) || x.TruckLoadStatus.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.Milage == null || x.Milage.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.FromStateId == null || string.IsNullOrEmpty(x.USAState.Name) || x.USAState.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.ToStateId == null || string.IsNullOrEmpty(x.USAState1.Name) || x.USAState1.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (string.IsNullOrEmpty(x.FromNotes) || x.FromNotes.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (string.IsNullOrEmpty(x.ToNotes) || x.ToNotes.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (string.IsNullOrEmpty(x.Notes) || x.Notes.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.Price == null || x.Price.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.PricePerMile == null || x.PricePerMile.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.FromDate == null || x.FromDate.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.ToDate == null || x.ToDate.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.FuelCost == null || x.FuelCost.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.DriverPerMileCost == null || x.PricePerMile.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.OtherCosts == null || x.PricePerMile.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (string.IsNullOrEmpty(x.OtherCostsNotes) || x.OtherCostsNotes.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()))
                    );
                }

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.TruckId == null ? "" : c.Truck.Name);
                            else
                                loads = loads.OrderByDescending(c => c.TruckId == null ? "" : c.Truck.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.DriverId == null ? "" : c.Driver.Name);
                            else
                                loads = loads.OrderByDescending(c => c.DriverId == null ? "" : c.Driver.Name);
                            break;
                        case 2:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.CompanyId == null ? "" : c.Company.Name);
                            else
                                loads = loads.OrderByDescending(c => c.CompanyId == null ? "" : c.Company.Name);
                            break;
                        case 3:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.StatusId == null ? "" : c.TruckLoadStatus.Name);
                            else
                                loads = loads.OrderByDescending(c => c.StatusId == null ? "" : c.TruckLoadStatus.Name);
                            break;
                        case 4:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.Milage);
                            else
                                loads = loads.OrderByDescending(c => c.Milage);
                            break;
                        case 5:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.Price);
                            else
                                loads = loads.OrderByDescending(c => c.Price);
                            break;
                        case 6:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                loads = loads.OrderBy(c => c.PricePerMile);
                            else
                                loads = loads.OrderByDescending(c => c.PricePerMile);
                            break;
                    }
                }

                baseListModel.Total = loads.Count();
                loads = loads.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    loads = loads.Take(dataTableSearchParameters.length);

                TruckLoadMapper.Map(dbContext, loads.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(TruckLoadModel truckLoadModel)
        {
            try
            {
                if (Validate(truckLoadModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Common.DAL.TruckLoad truckLoad = new Common.DAL.TruckLoad();

                        TruckLoadMapper.Map(dbContext, truckLoadModel, truckLoad);

                        truckLoad.Id = Guid.NewGuid().ToString();

                        dbContext.TruckLoads.Add(truckLoad);

                        base.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    truckLoadModel.AddSuccess(Resources.LoadAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(TruckLoadModel truckLoadModel)
        {
            try
            {
                Common.DAL.TruckLoad load = dbContext.TruckLoads.Where(x => x.Id == truckLoadModel.Id).FirstOrDefault();

                if (load == null)
                {
                    base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(truckLoadModel))
                    return;

                TruckLoadMapper.Map(dbContext, truckLoadModel, load);

                base.SaveChanges();

                truckLoadModel.AddSuccess(Resources.LoadUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(TruckLoadModel truckLoadModel)
        {
            try
            {
                if (ValidateDelete(truckLoadModel))
                    return;

                Common.DAL.TruckLoad load = dbContext.TruckLoads.Where(x => x.Id == truckLoadModel.Id).FirstOrDefault();

                if (load == null)
                {
                    base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.TruckLoads.Remove(load);

                base.SaveChanges();

                truckLoadModel.AddSuccess(Resources.LoadDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(truckLoadModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }
    }
}
