﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMYFrameWorkMVC.Common;
using System.Data.Entity.Infrastructure;
using LMYFrameWorkMVC.Common.Helpers;
using LMYFrameWorkMVC.Common.Entities;
namespace LMYFrameWorkMVC.Common.DAL
{
    public partial class Message : IAuditedDatesEntity, IAuditedUsersEntity
    {

    }
}