using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Dtos;

public class RegisterUserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public int? ConfirmCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role {get; set; }
}
