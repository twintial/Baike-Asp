using AutoMapper;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Profiles
{
    public class UserInfoProfile : Profile
    {
        public UserInfoProfile()
        {
            CreateMap<BkUserInfo, BKSearchUser>()
                .ForMember(dest => dest.iconURL, opt => opt.MapFrom(src => src.Icon));
        }
    }
}
