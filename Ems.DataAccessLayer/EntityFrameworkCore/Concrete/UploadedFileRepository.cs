using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Contexts;
using Ems.Entity.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ems.DataAccessLayer.EntityFrameworkCore.Concrete;

public class UploadedFileRepository : GenericRepository<UploadedFile>, IUploadedFileRepository
{
    public UploadedFileRepository(EmsContext emsContext) : base(emsContext)
    {
    }
}
