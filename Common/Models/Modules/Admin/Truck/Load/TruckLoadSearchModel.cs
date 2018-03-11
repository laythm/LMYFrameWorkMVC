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
    public class TruckLoadSearchModel : BaseModel
    {
        public TruckLoadSearchModel()
        {
            this.StatusesList = new List<SelectListItem>();
            this.TruckModel = new TruckModel(false);
            this.DriverModel = new DriverModel(false);
            this.CompanyModel = new DriverModel(false);
        }

        public TruckLoadSearchModel(bool init)
        {
        }
        
        public TruckModel TruckModel { get; set; }
        [Display(Name = "Truck", ResourceType = typeof(Resources.Resources))]
        public string TruckId { get; set; }

        public DriverModel DriverModel { get; set; }
        [Display(Name = "Driver", ResourceType = typeof(Resources.Resources))]
        public string DriverId { get; set; }

        public DriverModel CompanyModel { get; set; }
        [Display(Name = "Company", ResourceType = typeof(Resources.Resources))]
        public string CompanyId { get; set; }

        public List<SelectListItem> StatusesList { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Resources))]
        public string StatusId { get; set; }

        public Nullable<DateTime> FromDate { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string FromDateString { get; set; }

        public Nullable<DateTime> ToDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string ToDateString { get; set; }
    }
}
