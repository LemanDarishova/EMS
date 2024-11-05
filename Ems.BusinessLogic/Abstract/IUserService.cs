using Ems.BusinessLogic.Dtos;
using Ems.Core.Enums;
using Ems.Core.Wrappers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Abstract;

public interface IUserService
{
    Task<IResponseDataResult<RegisterUserDto>> CreateUserAsync(CreateUserDto userDto);
    Task<IResponseResult> ChangeUserStatusAsync(RegisterStatusEnum registerStatus, int userId);
    Task<IResponseDataResult<bool>> CheckConfirmCodeAsync(int confirmCode, int userId);
    Task<IResponseDataResult<RegisterUserDto>> CheckUserAsync(SigninUserDto userDto);

    void SetRegisterUser(ClaimsPrincipal claimsPrincipal);
    RegisterUserDto GetRegisterUserDto();
    Task<IResponseResult> UpdatePasswordAsync(IdentifyNewPassDto identifyNewPassDto);
}
