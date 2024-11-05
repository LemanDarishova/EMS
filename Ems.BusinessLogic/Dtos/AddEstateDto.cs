using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Dtos;

public class AddEstateDto
{
    public AddEstateDto()
    {
        UploadedFilesDtos = new HashSet<UploadedFileDto>();
    }
    public string Title { get; set; }
    public int Area { get; set; }
    public string Address { get; set; }
    public int Price { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
    public ICollection<UploadedFileDto> UploadedFilesDtos { get; set; }
}
