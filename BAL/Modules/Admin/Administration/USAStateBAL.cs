using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.USAState;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class USAStateBAL : BaseBAL
    {
        public USAStateBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private bool Validate(USAStateModel uSAStateModel)
        {
            if (dbContext.USAStates.Any(x => x.Name == uSAStateModel.Name && x.Id != uSAStateModel.Id.ToString()))
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.AlreadyExists, uSAStateModel.GetDisplayName(x => x.Name)), uSAStateModel.nameof(x => x.Name));


            if (dbContext.USAStates.Any(x => x.StateCode.ToLower() == uSAStateModel.StateCode.ToLower() && x.Id != uSAStateModel.Id.ToString()))
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.AlreadyExists, uSAStateModel.GetDisplayName(x => x.StateCode)), uSAStateModel.nameof(x => x.StateCode));

            return uSAStateModel.HasErrorByType();
        }

        private bool ValidateDelete(USAStateModel uSAStateModel)
        {
            return uSAStateModel.HasErrorByType();
        }

        public void PrepareUSAStateModel(USAStateModel uSAStateModel)
        {
            try
            {
            }
            catch (Exception ex)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetUSAStateModel(USAStateModel uSAStateModel)
        {
            try
            {
                USAState uSAState = dbContext.USAStates.Where(x => x.Id == uSAStateModel.Id).FirstOrDefault();

                if (uSAState == null)
                {
                    base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    USAStateMapper.Map(dbContext, uSAState, uSAStateModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

        }

        public GenericListModel<USAStateModel> GetSearchUSAStatesList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<USAStateModel> baseListModel = new GenericListModel<USAStateModel>();

            try
            {
                //if (!base.USAStateHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<USAState> uSAStates = dbContext.USAStates.OrderBy(x => x.CreatedAt);

                if (!string.IsNullOrEmpty(dataTableSearchParameters.search.value))
                {
                    uSAStates = uSAStates.Where(x =>
                        x.Name.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                        x.StateCode.ToLower().Contains(dataTableSearchParameters.search.value.ToLower()) ||
                        x.Notes.ToLower().Contains(dataTableSearchParameters.search.value.ToLower())
                    );
                }

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                uSAStates = uSAStates.OrderBy(c => c.Name);
                            else
                                uSAStates = uSAStates.OrderByDescending(c => c.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                uSAStates = uSAStates.OrderBy(c => c.StateCode);
                            else
                                uSAStates = uSAStates.OrderByDescending(c => c.StateCode);
                            break;
                    }
                }

                baseListModel.Total = uSAStates.Count();
                uSAStates = uSAStates.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    uSAStates = uSAStates.Take(dataTableSearchParameters.length);

                USAStateMapper.Map(dbContext, uSAStates.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(USAStateModel uSAStateModel)
        {
            try
            {
                if (Validate(uSAStateModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        USAState uSAState = new USAState();
                        USAStateMapper.Map(dbContext, uSAStateModel, uSAState);

                        uSAState.Id = Guid.NewGuid().ToString();

                        dbContext.USAStates.Add(uSAState);

                        base.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    uSAStateModel.AddSuccess(Resources.USAStateAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(USAStateModel uSAStateModel)
        {
            try
            {
                USAState uSAState = dbContext.USAStates.Where(x => x.Id == uSAStateModel.Id).FirstOrDefault();

                if (uSAState == null)
                {
                    base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(uSAStateModel))
                    return;

                USAStateMapper.Map(dbContext, uSAStateModel, uSAState);

                base.SaveChanges();

                uSAStateModel.AddSuccess(Resources.USAStateUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(USAStateModel uSAStateModel)
        {
            try
            {
                if (ValidateDelete(uSAStateModel))
                    return;

                USAState uSAState = dbContext.USAStates.Where(x => x.Id == uSAStateModel.Id).FirstOrDefault();

                if (uSAState == null)
                {
                    base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.USAStates.Remove(uSAState);

                base.SaveChanges();

                uSAStateModel.AddSuccess(Resources.USAStateDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(uSAStateModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }
    }
}
