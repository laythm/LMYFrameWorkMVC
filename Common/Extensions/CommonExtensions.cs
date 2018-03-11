using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LMYFrameWorkMVC.Common.Extensions
{
    public static class CommonExtensions
    {
        static CommonExtensions()
        {
        }

        public static string nameof<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            if (propertyAccessor.Body.NodeType == ExpressionType.MemberAccess)
            {
                var mem = propertyAccessor.Body as MemberExpression;
                if (mem == null)
                    return null;

                return mem.Member.Name;
            }

            return null;
        }

        //public static void CopyPropertyValues(this object destination, object source, List<string> execludeList = null, bool execludeGeneric = true)
        //{
        //    if (source != null)
        //    {
        //        System.Reflection.PropertyInfo[] destProperties = destination.GetType().GetProperties();

        //        foreach (System.Reflection.PropertyInfo sourceProperty in source.GetType().GetProperties().Where(x => (execludeGeneric && x.PropertyType.GetGenericArguments().Count() < 1) || !execludeGeneric))
        //        {
        //            if (execludeList == null || execludeList.All(x => x != sourceProperty.Name))
        //            {
        //                foreach (System.Reflection.PropertyInfo destProperty in destProperties)
        //                {
        //                    if (destProperty.Name == sourceProperty.Name &&
        //                destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
        //                    {
        //                        if (destProperty.CanWrite)
        //                            destProperty.SetValue(destination, sourceProperty.GetValue(
        //                                source, new object[] { }), new object[] { });

        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static void CopyPropertyValues(this object destination, object source, List<string> execludeList = null, bool execludeGeneric = true)
        {
            if (source != null)
            {
                System.Reflection.PropertyInfo[] destProperties = destination.GetType().GetProperties();

                foreach (System.Reflection.PropertyInfo sourceProperty in source.GetType().GetProperties().Where(x => (execludeGeneric && x.PropertyType.GetGenericArguments().Count() < 1) || !execludeGeneric))
                {
                    if (execludeList == null || execludeList.All(x => x != sourceProperty.Name))
                    {
                        foreach (System.Reflection.PropertyInfo destProperty in destProperties)
                        {
                            if (destProperty.Name == sourceProperty.Name &&
                        destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                            {
                                if (destProperty.CanWrite)
                                    destProperty.SetValue(destination, sourceProperty.GetValue(
                                        source, new object[] { }), new object[] { });

                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void CopyBaseModel(this BaseModel destination, BaseModel source)
        {
            destination.ErrorsList.AddRange(source.ErrorsList);
            destination.SuccessesList.AddRange(source.SuccessesList);
            destination.WarningList.AddRange(source.WarningList);
            destination.InfoList.AddRange(source.InfoList);
        }

        ////public static void ClearByModelName(this ModelStateDictionary modelStateDictionary, string subModelName)
        //{
        //    foreach (var modelError in modelStateDictionary)
        //    {
        //        string propertyName = modelError.Key;

        //        if (propertyName.Contains(subModelName))
        //        {
        //            modelStateDictionary[propertyName].Errors.Clear();
        //        }
        //    }
        //}\

        public static void Clear<TModel>(this ModelStateDictionary modelState, Expression<Func<TModel, object>> expression)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);

            if (expressionText.Contains("CS$<>8__locals"))
            {
                int i = 1;
                bool loop = true;

                while (loop)
                {
                    expressionText = expressionText.Replace("CS$<>8__locals" + i.ToString() + ".", "");
                    if (!expressionText.Contains("CS$<>8__locals"))
                        loop = false;
                    else
                        i++;
                }
            }
            expressionText = expressionText.Substring(expressionText.IndexOf(".") + 1);

            foreach (var ms in modelState.ToArray())
            {
                if (ms.Key.StartsWith(expressionText + ".") || ms.Key == expressionText)
                {
                    modelState[ms.Key].Errors.Clear();
                }
            }


            //sample
            //ModelState.Clear< ContactModel>( x => branchModel.ManagerModel);
            //ModelState.Clear< ContactModel>(  x => branchModel.SecretaryModel);
            //ModelState.Clear< ContactModel>(  x => branchModel.TellerModel);

            //List<ContactWorkHistoryModel> contactWorkHistoryModels = new List<ContactWorkHistoryModel>();
            //for (int i = 0; i < contactModel.ContactWorkHistoryModels.Count(); i++)
            //{
            //    if (contactModel.ContactWorkHistoryModels[i].Exists == true)
            //    {
            //        contactWorkHistoryModels.Add(contactModel.ContactWorkHistoryModels[i]);

            //        ModelState.Clear<PositionModel>(x => contactModel.ContactWorkHistoryModels[i].PositionModel);
            //        ModelState.Clear<WorkInstitutionModel>(x => contactModel.ContactWorkHistoryModels[i].WorkInstitutionModel);
            //    }
            //    else
            //    {
            //        ModelState.Clear<ContactWorkHistoryModel>(x => contactModel.ContactWorkHistoryModels[i]);
            //    }
            //}
        }

        public static void ClearAllExcept<TModel>(this ModelStateDictionary modelState, List<string> validateOnlyList)
        {
            foreach (PropertyInfo prop in typeof(TModel).GetProperties())
            {
                if (!validateOnlyList.Any(x => x == prop.Name))
                    modelState.Remove(prop.Name);
            }
        }
        public static bool IsValidFor<TModel>(this ModelStateDictionary modelState, System.Linq.Expressions.Expression<Func<TModel, object>> expression)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);

            return modelState.IsValidField(expressionText);
        }

        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            return ModelMetadata.FromLambdaExpression<TModel, TProperty>(
                expression,
                new ViewDataDictionary<TModel>(model)
                ).DisplayName;
        }

    }
}
