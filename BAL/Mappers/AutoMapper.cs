using AutoMapper;
using LMY.FrameWork.MVC.Common.Models;
using LMY.FrameWork.MVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMY.FrameWork.MVC.BAL.Config
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<AspNetRole, AspNetUserModel>();

            Mapper.CreateMap<AspNetUser, UserModel>()
                 .ForMember(dest => dest.RolesIDs, opts => opts.MapFrom(src =>  src.AspNetRoles.Select(x=>x.Id)));

            Mapper.CreateMap<UserModel, AspNetUser>();
                 //.ForMember(dest => dest.AspNetRoles.Select(x=>x.Id), opts => opts.MapFrom(src => src.RolesIDs));
        }
    }
}
