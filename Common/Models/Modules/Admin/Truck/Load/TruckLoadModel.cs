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

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck.Load
{
    public class TruckLoadModel : BaseModel
    {
        public TruckLoadModel()
        {
            TruckModel = new TruckModel(false);
            DriverModel = new DriverModel(false);
            CompanyModel = new CompanyModel(false);
            StatusModel = new StatusModel(false);
            FromStateModel = new USAStateModel(false);
            ToStateModel = new USAStateModel(false);
            StatesList = new List<SelectListItem>();
            StatusesList = new List<SelectListItem>();
        }

        public TruckLoadModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        public TruckModel TruckModel { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Truck", ResourceType = typeof(Resources.Resources))]
        public string TruckId { get; set; }

        public DriverModel DriverModel { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Driver", ResourceType = typeof(Resources.Resources))]
        public string DriverId { get; set; }

        public CompanyModel CompanyModel { get; set; }
        [Display(Name = "Company", ResourceType = typeof(Resources.Resources))]
        public string CompanyId { get; set; }

        public List<SelectListItem> StatusesList { get; set; }
        public StatusModel StatusModel { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Resources))]
        public string StatusId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(Int64.MinValue, Int64.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"[0-9]*", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "Milage", ResourceType = typeof(Resources.Resources))]
        public int Milage { get; set; }

        public List<SelectListItem> StatesList { get; set; }
        public USAStateModel FromStateModel { get; set; }
        [Display(Name = "FromState", ResourceType = typeof(Resources.Resources))]
        public string FromStateId { get; set; }

        public USAStateModel ToStateModel { get; set; }
        [Display(Name = "ToState", ResourceType = typeof(Resources.Resources))]
        public string ToStateId { get; set; }

        public Nullable<DateTime> FromDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string FromDateString { get; set; }

        public Nullable<DateTime> ToDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string ToDateString { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "FromNotes", ResourceType = typeof(Resources.Resources))]
        public string FromNotes { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "ToNotes", ResourceType = typeof(Resources.Resources))]
        public string ToNotes { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resources))]
        public string Notes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "Price", ResourceType = typeof(Resources.Resources))]
        public double Price { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "PricePerMile", ResourceType = typeof(Resources.Resources))]
        public double PricePerMile { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "FuelCost", ResourceType = typeof(Resources.Resources))]
        public double FuelCost { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "DriverPerMileCost", ResourceType = typeof(Resources.Resources))]
        public double DriverPerMileCost { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "DriverCost", ResourceType = typeof(Resources.Resources))]
        public double DriverCost { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "OtherCosts", ResourceType = typeof(Resources.Resources))]
        public double OtherCosts { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "OtherCostsNotes", ResourceType = typeof(Resources.Resources))]
        public string OtherCostsNotes { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalCosts", ResourceType = typeof(Resources.Resources))]
        public double TotalCosts { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalIncome", ResourceType = typeof(Resources.Resources))]
        public double TotalIncome { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Range(double.MinValue, double.MaxValue, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRange")]
        [RegularExpression(@"-?(?:\d*[\,\.])?\d+", ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationNumber")]
        [Display(Name = "TotalProfit", ResourceType = typeof(Resources.Resources))]
        public double TotalProfit { get; set; }
    }
}
