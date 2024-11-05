using Ems.BusinessLogic.Abstract;
using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Concrete;

public class ServiceBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public ServiceBase(IUserRepository userRepository, IUserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    public async Task SaveChangesAsync()
    {
        await _userRepository.SaveChangesAsync(_userService.GetRegisterUserDto().Id);
    }
}
