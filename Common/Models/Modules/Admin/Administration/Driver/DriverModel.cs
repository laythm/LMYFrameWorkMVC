
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
using LMYFrameWorkMVC.Common.Models.Modules.Admin.Truck;

namespace LMYFrameWorkMVC.Common.Models.Modules.Admin.Administration.Driver
{
    public class DriverModel : BaseModel
    {
        public DriverModel()
        {
            TruckModels = new List<TruckModel>();
        }

        public DriverModel(bool init)
        {
        }

        [Display(Name = "Id")]
        public string Id { get; set; }

        [MaxLength(300, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public new string Name { get; set; }

        public Nullable<DateTime> StartDate { get; set; }

        [Display(Name = "StartDate", ResourceType = typeof(Resources.Resources))]
        [DateAttribute(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationDate")]
        public string StartDateString { get; set; }

        [MaxLength(1000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Required(ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationRequired")]
        [Display(Name = "ContactNumber", ResourceType = typeof(Resources.Resources))]
        public string ContactNumber { get; set; }

        [MaxLength(3000, ErrorMessageResourceType = typeof(Resources.Resources), ErrorMessageResourceName = "ValidationMaxLength")]
        [Display(Name = "Notes", ResourceType = typeof(Resources.Resources))]
        public string Notes { get; set; }

        [Display(Name = "Trucks", ResourceType = typeof(Resources.Resources))]
        public List<TruckModel> TruckModels = new List<TruckModel>();
    }
}