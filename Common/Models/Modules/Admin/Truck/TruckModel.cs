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

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck
{
    public class TruckModel : BaseModel
    {
        public TruckModel()
        {
            DriverModel = new DriverModel();
        }

        public TruckModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "OnBuyPrice", ResourceType = typeof(Resources.Resources))]
        public double OnBuyPrice { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "OnBuyExpenses", ResourceType = typeof(Resources.Resources))]
        public double OnBuyExpenses { get; set; }

        public Nullable<DateTime> OnBuyDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "OnBuyDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string OnBuyDateString { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "OnBuyNotes", ResourceType = typeof(Resources.Resources))]
        public string OnBuyNotes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [RegularExpression("[0-9]*", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "OnBuyMilage", ResourceType = typeof(Resources.Resources))]
        public int OnBuyMilage { get; set; }

        public DriverModel DriverModel { get; set; }
        [Display(Name = "Driver", ResourceType = typeof(Resources.Resources))]
        public string DriverId { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resources))]
        public string Notes { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalCosts", ResourceType = typeof(Resources.Resources))]
        public double TotalCosts { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalIncome", ResourceType = typeof(Resources.Resources))]
        public double TotalIncome { get; set; }

        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalProfit", ResourceType = typeof(Resources.Resources))]
        public double TotalProfit { get; set; }
    }
}
