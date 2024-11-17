using AutoMapper;
using Ems.BusinessLogic.Dtos;
using Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
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
        CreateMap<CreateUserDto, UserDetail>();
        CreateMap<AddEstateDto, Estate>();
        CreateMap<UploadedFileDto, UploadedFile>();
        CreateMap<ResetPasswordDto, User>();
        CreateMap<ResetPasswordDto, UserDetail>();
        CreateMap<Estate, GetEstateViewDto>()
            .ForMember(
                dest => dest.Category,
                opt => opt.MapFrom(src => src.Category.CategoryName)
            )
            .ForMember(
                dest => dest.Image,
                opt => opt.MapFrom(x => x.UploadedFiles.FirstOrDefault().FileName)
            );

        CreateMap <CreateUserDto, User> ()
            .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => new List<UserRole>
            {
                new UserRole
                {
                    Role = new Role { RoleName = src.Role }
                }
            }));

    }
}