using AutoMapper;
using BaikeAsp.Dto;
using BaikeAsp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaikeAsp.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<BKCommentsViewModel, BkComments>()
                .ForMember(dest => dest.UId, opt => opt.MapFrom(src => src.Uid));
        }
    }
}
