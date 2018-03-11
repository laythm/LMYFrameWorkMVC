using LMYFrameWorkMVC.Common;
using LMYFrameWorkMVC.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMYFrameWorkMVC.DAL
{
    public interface IAuditedDatesEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime LastModifiedAt { get; set; }
    }

    public interface IAuditedUsersEntity
    {
        string CreatedBy { get; set; }
        string LastModifiedBy { get; set; }
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    public interface IAuditWrapper
    {
        string GetJSONData();
        string GetPrimaryKey();
    }

    public interface IDefaultValues
    {
        void SetDefaultValues();
    }

    public interface ICacheClear
    {
        void ClearCache();
    }
}
