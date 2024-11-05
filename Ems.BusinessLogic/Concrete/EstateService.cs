using AutoMapper;
using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Dtos;
using Ems.BusinessLogic.Validations;
using Ems.Core.Enums;
using Ems.Core.Extensions;
using Ems.Core.Wrappers.Concrete;
using Ems.Core.Wrappers.Interfaces;
using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
using Ems.Entity.Commons;
using Ems.Entity.Estates;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Ems.BusinessLogic.Concrete;

public class EstateService : ServiceBase,  IEstateService
{
    private readonly IMapper _mapper;
    private readonly IEstateRepository _estateRepository;
    private readonly IValidator<AddEstateDto> _addEstateDtoValidator;

    public EstateService(IUserRepository userRepository, IUserService userService, IMapper mapper, IEstateRepository estateRepository, IValidator<AddEstateDto> addEstateDtoValidator) : base(userRepository, userService)
    {
        _mapper = mapper;
        _estateRepository = estateRepository;
        _addEstateDtoValidator = addEstateDtoValidator;
    }


    public async Task<IResponseDataResult<bool>> AddAsync(AddEstateDto estateDto)
    {
        var result = await _addEstateDtoValidator.ValidateAsync(estateDto);

        if (result.IsValid is false)
        {
            return new ResponseDataResult<bool>(result.ToResponseValidationResults());
        }

        var estateEntity = _mapper.Map<Estate>(estateDto);
        var uploadedFilesEntity = _mapper.Map<ICollection<UploadedFile>>(estateDto.UploadedFilesDtos);

        estateEntity.UploadedFiles = uploadedFilesEntity;

        await _estateRepository.AddAsync(estateEntity);

        await SaveChangesAsync();

        return new ResponseDataResult<bool>(ResponseType.SuccessResult);

    }

    public async Task<IResponseDataResult<IEnumerable<GetEstateViewDto>>> GetAllAsync()
    {
        var estate = await _estateRepository.GetEstateWithDetailsAsync();

        return new ResponseDataResult<IEnumerable<GetEstateViewDto>>
            (ResponseType.SuccessResult, _mapper.Map<IEnumerable<GetEstateViewDto>>(estate));

    }

    public async Task<IResponseDataResult<bool>> RemoveAsync(int id)
    {
        var estate = await _estateRepository.GetByIdAsync(id);
        if (estate is null)
        {
            return new ResponseDataResult<bool>(ResponseType.NotFound, "Estate  not found to delete");
        }

        _estateRepository.Remove(estate);
        await SaveChangesAsync();
        return new ResponseDataResult<bool>(ResponseType.SuccessResult);
    }

    public async Task<IResponseDataResult<bool>> UpdateAsync(int id)
    {
        var estate = await _estateRepository.GetByIdAsync(id);

        if (estate == null)
        {
            return new ResponseDataResult<bool>(ResponseType.NotFound, "Estate not fount");
        }

        _estateRepository.Update(estate);
        await SaveChangesAsync();
        return new ResponseDataResult<bool>(ResponseType.SuccessResult);

    }
}
