using Ems.Entity.Commons;
using Ems.Entity.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Entity.Accounds;

public class User : BaseEntity
{
    public User()
    {
        UserRoles = new List<UserRole>();
        UploadedFiles = new List<UploadedFile>();
        Estates = new List<Estate>();
    }
    public string Email { get; set; }
    public string PassworHash { get; set; }
    public string PasswordSalt { get; set; }
    public UserDetail? UserDetail { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UploadedFile> UploadedFiles { get; set; }
    public ICollection<Estate> Estates { get; set; }
}
