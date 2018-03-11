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
    public class TruckExpenseModel : BaseModel
    {
        public TruckExpenseModel()
        {
            TruckExpensesTypesList=new List<SelectListItem>();
            ExpenseTypeModel=new ExpenseTypeModel(false);
            TruckModel = new TruckModel(false);
        }

        public TruckExpenseModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        public TruckModel TruckModel { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Truck", ResourceType = typeof(Resources.Resources))]
        public string TruckId { get; set; }

        public ExpenseTypeModel ExpenseTypeModel { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "ExpenseType", ResourceType = typeof(Resources.Resources))]
        public string ExpenseTypeId { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Price", ResourceType = typeof(Resources.Resources))]
        public double Price { get; set; }

        public Nullable<DateTime> AtDate { get; set; }
        [Display(Name = "AtDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string AtDateString { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resources))]
        public string Notes { get; set; }

        public List<SelectListItem> TruckExpensesTypesList { get; set; }
    }
}
