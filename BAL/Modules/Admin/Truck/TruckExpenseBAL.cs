using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.TruckExpense;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.TruckExpenses
{
    public class TruckExpenseBAL : BaseBAL
    {
        public TruckExpenseBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(TruckExpenseModel truckExpenseModel)
        {
            return truckExpenseModel.HasErrorByType();
        }

        private bool ValidateDelete(TruckExpenseModel truckExpenseModel)
        {
            return truckExpenseModel.HasErrorByType();
        }

        public void PrepareTruckExpenseModel(TruckExpenseModel truckExpenseModel)
        {
            try
            {
                truckExpenseModel.TruckExpensesTypesList = dbContext.TruckExpensTypes.
                     Select(x => new
                     {
                         x.Id,
                         x.Name,
                     }).ToList().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void PrepareTruckExpenseSearchModel(TruckExpenseSearchModel truckExpenseSearchModel)
        {
            try
            {
                truckExpenseSearchModel.TruckExpensesTypesList = dbContext.TruckExpensTypes.
                     Select(x => new
                     {
                         x.Id,
                         x.Name,
                     }).ToList().OrderBy(x => x.Name).Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseSearchModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }
        public void GetTruckExpenseModel(TruckExpenseModel truckExpenseModel)
        {
            try
            {
                Common.DAL.TruckExpens truckExpens = dbContext.TruckExpenses.Where(x => x.Id == truckExpenseModel.Id).FirstOrDefault();

                if (truckExpens == null)
                {
                    base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    TruckExpenseMapper.Map(dbContext, truckExpens, truckExpenseModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public GenericListModel<TruckExpenseModel> GetSearchTruckExpensessList(DataTableSearchParameters<TruckExpenseSearchModel> dataTableSearchParameters)
        {
            GenericListModel<TruckExpenseModel> baseListModel = new GenericListModel<TruckExpenseModel>();

            try
            {
                //if (!base.TruckExpensesHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<Common.DAL.TruckExpens> truckExpenses = dbContext.TruckExpenses.OrderBy(x => x.CreatedAt);

                if (dataTableSearchParameters.CustomSearchObject != null)
                {
                    if (!string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.AtDateString))
                    {
                        dataTableSearchParameters.CustomSearchObject.AtDate = dataTableSearchParameters.CustomSearchObject.AtDateString.ConvertToDateTime();
                    }

                    truckExpenses = truckExpenses.Where(x =>
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.TruckId) || x.TruckId == dataTableSearchParameters.CustomSearchObject.TruckId) &&
                            (string.IsNullOrEmpty(dataTableSearchParameters.CustomSearchObject.ExpenseTypeId) || x.ExpenseTypeId == dataTableSearchParameters.CustomSearchObject.ExpenseTypeId) &&
                            (dataTableSearchParameters.CustomSearchObject.AtDate == null || x.AtDate == dataTableSearchParameters.CustomSearchObject.AtDate.Value)
                    );
                }

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    truckExpenses = truckExpenses.Where(x =>
                        (x.ExpenseTypeId == null || string.IsNullOrEmpty(x.TruckExpensType.Name) || x.TruckExpensType.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.TruckId == null || string.IsNullOrEmpty(x.Truck.Name) || x.Truck.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.Price == null || x.Price.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (x.AtDate == null || x.AtDate.ToString().ToLower().Contains(dataTableSearchParameters.search.value.ToLower())) ||
                        (string.IsNullOrEmpty(x.Notes) || x.Notes.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()))
                    );
                }

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                truckExpenses = truckExpenses.OrderBy(c => c.TruckId == null ? "" : c.Truck.Name);
                            else
                                truckExpenses = truckExpenses.OrderByDescending(c => c.TruckId == null ? "" : c.Truck.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                truckExpenses = truckExpenses.OrderBy(c => c.ExpenseTypeId == null ? "" : c.TruckExpensType.Name);
                            else
                                truckExpenses = truckExpenses.OrderByDescending(c => c.ExpenseTypeId == null ? "" : c.TruckExpensType.Name);
                            break;
                        case 2:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                truckExpenses = truckExpenses.OrderBy(c => c.Price);
                            else
                                truckExpenses = truckExpenses.OrderByDescending(c => c.Price);
                            break;
                        case 3:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                truckExpenses = truckExpenses.OrderBy(c => c.AtDate);
                            else
                                truckExpenses = truckExpenses.OrderByDescending(c => c.AtDate);
                            break;
                    }
                }

                baseListModel.Total = truckExpenses.Count();
                truckExpenses = truckExpenses.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    truckExpenses = truckExpenses.Take(dataTableSearchParameters.length);

                TruckExpenseMapper.Map(dbContext, truckExpenses.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(TruckExpenseModel truckExpenseModel)
        {
            try
            {
                if (Validate(truckExpenseModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Common.DAL.TruckExpens truckExpens = new Common.DAL.TruckExpens();
                        TruckExpenseMapper.Map(dbContext, truckExpenseModel, truckExpens);

                        truckExpens.Id = Guid.NewGuid().ToString();

                        dbContext.TruckExpenses.Add(truckExpens);

                        base.SaveChanges();
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    truckExpenseModel.AddSuccess(Resources.TruckExpenseAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(TruckExpenseModel truckExpenseModel)
        {
            try
            {
                Common.DAL.TruckExpens truckExpens = dbContext.TruckExpenses.Where(x => x.Id == truckExpenseModel.Id).FirstOrDefault();

                if (truckExpens == null)
                {
                    base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(truckExpenseModel))
                    return;

                TruckExpenseMapper.Map(dbContext, truckExpenseModel, truckExpens);

                base.SaveChanges();

                truckExpenseModel.AddSuccess(Resources.TruckExpenseUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(TruckExpenseModel truckExpenseModel)
        {
            try
            {
                if (ValidateDelete(truckExpenseModel))
                    return;

                Common.DAL.TruckExpens truckExpens = dbContext.TruckExpenses.Where(x => x.Id == truckExpenseModel.Id).FirstOrDefault();

                if (truckExpens == null)
                {
                    base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.TruckExpenses.Remove(truckExpens);

                base.SaveChanges();

                truckExpenseModel.AddSuccess(Resources.TruckExpenseDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(truckExpenseModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }
    }
}
