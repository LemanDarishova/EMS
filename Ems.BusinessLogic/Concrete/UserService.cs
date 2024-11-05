using AutoMapper;
using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Dtos;
using Ems.BusinessLogic.Validations;
using Ems.Core.Enums;
using Ems.Core.Extensions;
using Ems.Core.Helpers;
using Ems.Core.Wrappers.Concrete;
using Ems.Core.Wrappers.Interfaces;
using Ems.DataAccessLayer.Abstract;
using Ems.Entity.Accounds;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ems.BusinessLogic.Concrete;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserDetailRepository _userDetailRepository;
    private readonly IValidator<CreateUserDto> _createUserValidator;
    private readonly IValidator<SigninUserDto> _signinUserValidator;
    private readonly IMapper _mapper;
    private RegisterUserDto _registerUser;

    public UserService(IUserRepository userRepository, IUserDetailRepository userDetailRepository, IValidator<CreateUserDto> createUserValidator, IMapper mapper, IValidator<SigninUserDto> signinUserValidator)
    {
        _userRepository = userRepository;
        _userDetailRepository = userDetailRepository;
        _createUserValidator = createUserValidator; 
        _signinUserValidator = signinUserValidator;
        _mapper = mapper;
        
    }



    public async Task<IResponseDataResult<bool>> CheckConfirmCodeAsync(int confirmCode, int userId)
    {
        var user = await _userDetailRepository
           .GetWhereAsync(x => x.UserId == userId && x.ConfirmCode == confirmCode)
           .FirstOrDefaultAsync();

        return new ResponseDataResult<bool>(ResponseType.SuccessResult, user != null);
    }




    public async Task<IResponseDataResult<RegisterUserDto>> CreateUserAsync(CreateUserDto userDto)
    {
        var result = await _createUserValidator.ValidateAsync(userDto);
        if (result.IsValid is false)
        {
            return new ResponseDataResult<RegisterUserDto>(result.ToResponseValidationResults());
        }

        var user = await _userRepository
            .GetWhereAsync(x => x.Email == userDto.Email)
            .FirstOrDefaultAsync();

        if (user is not null)
        {
            return new ResponseDataResult<RegisterUserDto>(
                [new() { ErrorMessage = "Mail is Registered", PropertyName = "Email" }]);
        }

        var userEntity = _mapper.Map<User>(userDto);
        userEntity.UserDetail = _mapper.Map<UserDetail>(userDto);

        byte[] passwordHash;
        byte[] passwordSalt;

        HashHelper.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);
        userEntity.PasswordSalt = passwordSalt.ByteToString();
        userEntity.PassworHash = passwordHash.ByteToString();
        userEntity.UserDetail.ConfirmCode = CodeGenerator.GenerateConfirmCode();
        userEntity.UserDetail.StatusId = (int)RegisterStatusEnum.Register;
        userEntity.UserRoles =
            [
                new() {RoleId = 3}
            ];

        await _userRepository.AddAsync(userEntity);
        await _userRepository.SaveChangesAsync();

        return new ResponseDataResult<RegisterUserDto>(ResponseType.SuccessResult, new RegisterUserDto()
        {

            Id = userEntity.Id,
            Email = userEntity.Email,
            ConfirmCode = userEntity.UserDetail.ConfirmCode,
            FirstName = userEntity.UserDetail.FirstName,
            LastName = userEntity.UserDetail.LastName,
        });

    }


    public async Task<IResponseResult> ChangeUserStatusAsync(RegisterStatusEnum registerStatus, int userId)
    {
        var user = await _userDetailRepository
             .GetWhereAsync(x => x.UserId == userId)
             .SingleOrDefaultAsync();

        if (user is not null)
        {
            user.StatusId = (int)registerStatus;

            await _userDetailRepository.SaveChangesAsync();

            return new ResponseResult(ResponseType.SuccessResult);
        }

        return new ResponseResult(ResponseType.NotFound);
    }





    public async Task<IResponseDataResult<RegisterUserDto>> CheckUserAsync(SigninUserDto userDto)
    {
        var result = await _signinUserValidator.ValidateAsync(userDto);
        if (result.IsValid is false)
        {
            return new ResponseDataResult<RegisterUserDto>(result.ToResponseValidationResults());
        }

        var user = await _userRepository.GetSigninUserWithDetailAsync(userDto.Email);
        if (user is null)
        {
            return new ResponseDataResult<RegisterUserDto>(
                [new() { ErrorMessage = "User is not found by email", PropertyName = "Email" }]);
        }


        if (user.UserDetail.StatusId == (int)RegisterStatusEnum.Register)
        {
            return new ResponseDataResult<RegisterUserDto>(
                [new() { ErrorMessage = "Email is not confirmed", PropertyName = "Email" }]);
        }

        bool isCorrectPassword = HashHelper.VerifyPasswordHash(userDto.Password,
                                                               user.PassworHash.ToByte(),
                                                               user.PasswordSalt.ToByte());

        if (isCorrectPassword is false)
        {
            return new ResponseDataResult<RegisterUserDto>(
                [new() { ErrorMessage = "Password is incorrect", PropertyName = "Password" }]);
        }

        return new ResponseDataResult<RegisterUserDto>(ResponseType.SuccessResult, new RegisterUserDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.UserDetail.FirstName,
            LastName = user.UserDetail.LastName,
            ConfirmCode = user.UserDetail.ConfirmCode,
        });
    }




    public RegisterUserDto GetRegisterUserDto()
    {
        return _registerUser;
    }



    public void SetRegisterUser(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
        var firstName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name));
        var lastName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname));

        _registerUser = new RegisterUserDto
        {
            Id = Convert.ToInt32(userId.Value),
            FirstName = firstName.Value.ToString(),
            LastName = lastName.Value,
        };
    }


    public async Task<IResponseResult> UpdatePasswordAsync(IdentifyNewPassDto identifyNewPassDto)
    {
        var user = await _userRepository.FindByEmailAsync(identifyNewPassDto.Email);
        if (user == null)
        {
            return new ResponseResult(ResponseType.NotFound);
        }

        byte[] passwordHash;
        byte[] passwordSalt;
        HashHelper.CreatePasswordHash(identifyNewPassDto.NewPassword, out passwordHash, out passwordSalt);

        user.PassworHash = passwordHash.ByteToString();
        user.PasswordSalt = passwordSalt.ByteToString();


        _userRepository.Update(user);

        var resultDto = _mapper.Map<RegisterUserDto>(user);
        return new ResponseResult(ResponseType.SuccessResult);
    }
}
