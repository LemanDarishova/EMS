using AutoMapper;
using Ems.BusinessLogic.Dtos;
using Ems.Entity.Accounds;
using Ems.Entity.Commons;
using Ems.Entity.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Mappers;
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<CreateUserDto, User>();
        CreateMap<CreateUserDto, UserDetail>();
        CreateMap<AddEstateDto, Estate>();
        CreateMap<UploadedFileDto, UploadedFile>();


        CreateMap<Estate, GetEstateViewDto>()
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.CategoryName)
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(x => x.UploadedFiles != null && x.UploadedFiles.Any()
                ?   x.UploadedFiles.FirstOrDefault().RelativePath
                :null)
            );

        CreateMap<IdentifyNewPassDto, User>();
        CreateMap<IdentifyNewPassDto, UserDetail>();

    }
}