using LMYFrameWorkMVC.Common.Attributes;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Message;
using LMYFrameWorkMVC.Common.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Company;
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.USAState;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.TruckExpense
{
    public class TruckExpenseSearchModel : BaseModel
    {
        public TruckExpenseSearchModel()
        {
            this.TruckExpensesTypesList = new List<SelectListItem>();
            this.TruckModel = new TruckModel(false);
        }

        public TruckExpenseSearchModel(bool init)
        {
        }
        
        public TruckModel TruckModel { get; set; }
        [Display(Name = "Truck", ResourceType = typeof(Resources.Resources))]
        public string TruckId { get; set; }
         
        public List<SelectListItem> TruckExpensesTypesList { get; set; }
        [Display(Name = "ExpenseType", ResourceType = typeof(Resources.Resources))]
        public string ExpenseTypeId { get; set; }

        public Nullable<DateTime> AtDate { get; set; }
        [Display(Name = "AtDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string AtDateString { get; set; }
    }
}
