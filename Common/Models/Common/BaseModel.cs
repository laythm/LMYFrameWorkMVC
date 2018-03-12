using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
namespace LMYFrameWorkMVC.Common.Models.Common
{
    public class BaseModel
    {
        public BaseModel()
        {
            SuccessesList = new List<Success>();
            ErrorsList = new List<Error>();
            InfoList = new List<Info>();
            WarningList = new List<Warning>();
        }

        #region Properties

        public bool HasErrorByType(LookUps.ErrorType? errorType = null)
        {
            if (errorType == null)
                return ErrorsList.Count() > 0;
            else
                return ErrorsList.Where(x => x.ErrorType == errorType).Count() > 0;
        }

        public bool HasSuccess(LookUps.SuccessType? successType = null)
        {
            if (successType == null)
                return SuccessesList.Count() > 0;
            else
                return SuccessesList.Where(x => x.SuccessType == successType).Count() > 0;
        }

        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public string Name { set; get; }

        #endregion

        #region Messages

        public List<Success> SuccessesList { get; set; }
        public List<Error> ErrorsList { get; set; }
        public List<Info> InfoList { get; set; }
        public List<Warning> WarningList { get; set; }
        public bool HasError
        {
            get
            {
                return HasErrorByType();
            }
        }

        public void AddError(string message, LookUps.ErrorType errorType, string propertyName = "")
        {
            ErrorsList.Add(new Error
            {
                Message = message,
                PropertyName = propertyName,
                ErrorType = errorType
            });
        }

        public void AddSuccess(string message, LookUps.SuccessType successType)
        {
            SuccessesList.Add(new Success
            {
                Message = message,
                SuccessType = successType
            });
        }

        public void AddInfo(string message)
        {
            InfoList.Add(new Info
            {
                Message = message
            });
        }

        public void AddWarning(string message)
        {
            WarningList.Add(new Warning
            {
                Message = message
            });
        }

        #endregion
    }

    public class Error
    {
        public string Message { get; set; }
        public string PropertyName { get; set; }
        public LookUps.ErrorType ErrorType { get; set; }
    }

    public class Success
    {
        public string Message { get; set; }
        public LookUps.SuccessType SuccessType { get; set; }
    }

    public class Info
    {
        public string Message { get; set; }
    }

    public class Warning
    {
        public string Message { get; set; }
    }
}
