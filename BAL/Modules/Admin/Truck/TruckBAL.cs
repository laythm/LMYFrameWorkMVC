using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Common;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Truck
{
    public class TruckBAL : BaseBAL
    {
        public TruckBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(TruckModel truckModel)
        {
            if (dbContext.Trucks.Any(x => x.Name == truckModel.Name && x.Id != truckModel.Id.ToString()))
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.AlreadyExists, truckModel.GetDisplayName(x => x.Name)), truckModel.nameof(x => x.Name));

            return truckModel.HasErrorByType();
        }

        private bool ValidateDelete(TruckModel truckModel)
        {
            return truckModel.HasErrorByType();
        }

        public void PrepareTruckModel(TruckModel truckModel)
        {
            try
            {
            }
            catch (Exception ex)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetTruckModel(TruckModel truckModel)
        {
            try
            {
                Common.DAL.Truck truck = dbContext.Trucks.Where(x => x.Id == truckModel.Id).FirstOrDefault();

                if (truck == null)
                {
                    base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    TruckMapper.Map(dbContext, truck, truckModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

        }

        private IQueryable<Common.DAL.Truck> prepareSearch(string value)
        {
            IQueryable<Common.DAL.Truck> trucks = dbContext.Trucks.OrderBy(x => x.CreatedAt);

            if (!string.IsNullOrEmpty(value))
            {
                trucks = trucks.Where(x =>
                        (string.IsNullOrEmpty(x.Name) || x.Name.ToLower().Contains(value.ToLower())) ||
                        (x.OnBuyPrice == null || x.OnBuyPrice.ToString().ToLower().Contains(value.ToLower())) ||
                        (x.OnBuyExpenses == null || x.OnBuyExpenses.ToString().ToLower().Contains(value.ToLower())) ||
                        (x.OnBuyDate == null || x.OnBuyDate.ToString().ToLower().Contains(value.ToLower())) ||
                        (string.IsNullOrEmpty(x.OnBuyNotes) == null || x.OnBuyNotes.ToString().ToLower().Contains(value.ToLower())) ||
                        (x.OnBuyMilage == null || x.OnBuyMilage.ToString().ToLower().Contains(value.ToLower())) ||
                        (x.DriverId == null || string.IsNullOrEmpty(x.Driver.Name) || x.Driver.Name.ToLower().Contains(value.ToLower())) ||
                        (string.IsNullOrEmpty(x.Notes) || x.Notes.ToLower().Contains(value.ToLower())));
            }

            return trucks;
        }

        public GenericListModel<TruckModel> GetSearchTrucksList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<TruckModel> baseListModel = new GenericListModel<TruckModel>();

            try
            {
                IQueryable<Common.DAL.Truck> trucks = prepareSearch(dataTableSearchParameters.search.value);
 
                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                trucks = trucks.OrderBy(c => c.Name);
                            else
                                trucks = trucks.OrderByDescending(c => c.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                trucks = trucks.OrderBy(c => c.OnBuyPrice);
                            else
                                trucks = trucks.OrderByDescending(c => c.OnBuyPrice);
                            break;
                        case 2:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                trucks = trucks.OrderBy(c => c.OnBuyMilage);
                            else
                                trucks = trucks.OrderByDescending(c => c.OnBuyMilage);
                            break;
                        case 3:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                trucks = trucks.OrderBy(c => c.DriverId == null ? "" : c.Driver.Name);
                            else
                                trucks = trucks.OrderByDescending(c => c.DriverId == null ? "" : c.Driver.Name);
                            break;
                    }
                }

                baseListModel.Total = trucks.Count();
                trucks = trucks.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    trucks = trucks.Take(dataTableSearchParameters.length);

                TruckMapper.Map(dbContext, trucks.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public GenericListModel<TruckModel> GetTrucks(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<TruckModel> baseListModel = new GenericListModel<TruckModel>();

            try
            {
                IQueryable<Common.DAL.Truck> trucks = prepareSearch(select2Parameters.text);
 
                trucks = trucks.OrderBy(x => x.CreatedAt);

                baseListModel.Total = trucks.Count();
                trucks = trucks.Skip(select2Parameters.start);
                trucks = trucks.Take(select2Parameters.pageSize);

                TruckMapper.Map(dbContext, trucks.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(TruckModel truckModel)
        {
            try
            {
                if (Validate(truckModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Common.DAL.Truck truck = new Common.DAL.Truck();
                        TruckMapper.Map(dbContext, truckModel, truck);

                        truck.Id = Guid.NewGuid().ToString();

                        dbContext.Trucks.Add(truck);

                        base.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    truckModel.AddSuccess(Resources.TruckAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(TruckModel truckModel)
        {
            try
            {
                Common.DAL.Truck truck = dbContext.Trucks.Where(x => x.Id == truckModel.Id).FirstOrDefault();

                if (truck == null)
                {
                    base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(truckModel))
                    return;

                TruckMapper.Map(dbContext, truckModel, truck);

                base.SaveChanges();

                truckModel.AddSuccess(Resources.TruckUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(TruckModel truckModel)
        {
            try
            {
                if (ValidateDelete(truckModel))
                    return;

                Common.DAL.Truck truck = dbContext.Trucks.Where(x => x.Id == truckModel.Id).FirstOrDefault();

                if (truck == null)
                {
                    base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.Trucks.Remove(truck);

                base.SaveChanges();

                truckModel.AddSuccess(Resources.TruckDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(truckModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }
    }
}
