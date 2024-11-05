using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.BusinessLogic.Dtos;

public class GetEstateViewDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Area { get; set; }
    public string Address { get; set; }
    public int Price { get; set; }
    public IFormFile Image { get; set; }
    public string Category { get; set; }
}
