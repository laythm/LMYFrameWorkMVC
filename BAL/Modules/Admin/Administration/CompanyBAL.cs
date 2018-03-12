using System;
using System.Linq;
using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using CommonLayer = LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Models.Common;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company;
using LMYFrameWorkMVC.Common.DAL;
using LMYFrameWorkMVC.Common.Resources;
using LMYFrameWorkMVC.Common.Extensions;
using LMYFrameWorkMVC.BAL.Mappers.Admin.Administration;
using LMYFrameWorkMVC.Common.Models.Common;

namespace LMYFrameWorkMVC.BAL.Modules.Admin.Administration
{
    public class CompanyBAL : BaseBAL
    {
        public CompanyBAL(CommonLayer.Entities.ContextInfo contextInfo)
            : base(contextInfo)
        {
        }

        private IQueryable<Company> prepareSearch(string value)
        {
            IQueryable<Company> companies = dbContext.Companies.OrderBy(x => x.CreatedAt);

            if (!string.IsNullOrEmpty(value))
            {
                companies = companies.Where(x =>
                       x.Name.ToLower().Contains(value.ToLower()) ||
                       x.Notes.ToLower().Contains(value.ToLower())
                   );
            }

            return companies;
        }

        private bool Validate(CompanyModel companyModel)
        {
            if (dbContext.Companies.Any(x => x.Name == companyModel.Name && x.Id != companyModel.Id.ToString()))
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Business, null, string.Format(Resources.AlreadyExists, companyModel.GetDisplayName(x => x.Name)), companyModel.nameof(x => x.Name));

            return companyModel.HasErrorByType();
        }

        private bool ValidateDelete(CompanyModel companyModel)
        {
            return companyModel.HasErrorByType();
        }

        public void PrepareCompanyModel(CompanyModel companyModel)
        {
            try
            {
            }
            catch (Exception ex)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }
        }

        public void GetCompanyModel(CompanyModel companyModel)
        {
            try
            {
                Company company = dbContext.Companies.Where(x => x.Id == companyModel.Id).FirstOrDefault();

                if (company == null)
                {
                    base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                }
                else
                {
                    CompanyMapper.Map(dbContext, company, companyModel);
                }

            }
            catch (Exception ex)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

        }

        public GenericListModel<CompanyModel> GetSearchCompaniesList(DataTableSearchParameters<Nullable<bool>> dataTableSearchParameters)
        {
            GenericListModel<CompanyModel> baseListModel = new GenericListModel<CompanyModel>();

            try
            {
                //if (!base.CompanyHasPermision(baseListModel))
                //    return baseListModel;
                IQueryable<Company> companies = prepareSearch(dataTableSearchParameters.search.value);

                foreach (JQDTColumnOrder order in dataTableSearchParameters.order)
                {
                    switch (order.column)
                    {
                        case 0:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                companies = companies.OrderBy(c => c.Name);
                            else
                                companies = companies.OrderByDescending(c => c.Name);
                            break;
                        case 1:
                            if (order.dir == JQDTColumnOrderDirection.asc)
                                companies = companies.OrderBy(c => c.Notes);
                            else
                                companies = companies.OrderByDescending(c => c.Notes);
                            break;
                    }
                }

                baseListModel.Total = companies.Count();
                companies = companies.Skip(dataTableSearchParameters.start);

                if (dataTableSearchParameters.length != -1)
                    companies = companies.Take(dataTableSearchParameters.length);

                CompanyMapper.Map(dbContext, companies.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }

        public void Create(CompanyModel companyModel)
        {
            try
            {
                if (Validate(companyModel))
                    return;

                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Company company = new Company();
                        CompanyMapper.Map(dbContext, companyModel, company);

                        company.Id = Guid.NewGuid().ToString();

                        dbContext.Companies.Add(company);

                        base.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                    companyModel.AddSuccess(Resources.CompanyAddedSuccessfully, LookUps.SuccessType.Full);
                }
            }
            catch (Exception ex)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Edit(CompanyModel companyModel)
        {
            try
            {
                Company company = dbContext.Companies.Where(x => x.Id == companyModel.Id).FirstOrDefault();

                if (company == null)
                {
                    base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                if (Validate(companyModel))
                    return;

                CompanyMapper.Map(dbContext, companyModel, company);

                base.SaveChanges();

                companyModel.AddSuccess(Resources.CompanyUpdatedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (Exception ex)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public void Delete(CompanyModel companyModel)
        {
            try
            {
                if (ValidateDelete(companyModel))
                    return;

                Company company = dbContext.Companies.Where(x => x.Id == companyModel.Id).FirstOrDefault();

                if (company == null)
                {
                    base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Critical, null, Resources.NotFound);
                    return;
                }

                dbContext.Companies.Remove(company);

                base.SaveChanges();

                companyModel.AddSuccess(Resources.CompanyDeletedSuccessfully, LookUps.SuccessType.Full);
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Business, null, Resources.RefrenceDeleteError);
                base.UndoUpdates();
            }
            catch (Exception ex)
            {
                base.HandleError(companyModel, CommonLayer.LookUps.ErrorType.Exception, ex);
                base.UndoUpdates();
            }
        }

        public GenericListModel<CompanyModel> GetCompanies(Select2Parameters<bool> select2Parameters)
        {
            GenericListModel<CompanyModel> baseListModel = new GenericListModel<CompanyModel>();

            try
            {
                IQueryable<Company> companies = prepareSearch(select2Parameters.text);


                companies = companies.OrderBy(x => x.CreatedAt);

                baseListModel.Total = companies.Count();
                companies = companies.Skip(select2Parameters.start);
                companies = companies.Take(select2Parameters.pageSize);

                CompanyMapper.Map(dbContext, companies.ToList(), baseListModel.List);
            }
            catch (Exception ex)
            {
                base.HandleError(baseListModel, CommonLayer.LookUps.ErrorType.Critical, ex);
            }

            return baseListModel;
        }
    }
}
