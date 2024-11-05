using Ems.Entity.Accounds;
using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.Entity.Estates;

public class Estate : EditedBaseEntity
{
    public Estate() => UploadedFiles = new List<UploadedFile>();

    public string Title { get; set; }
    public int Area { get; set; }
    public  string Address { get; set; }
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<UploadedFile> UploadedFiles { get; set; }
    public int UserId{ get; set; }
    public User Users { get; set; }

}
