using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;
using System.Data.SqlClient;

namespace AEOService.Services
{
    public class PreviewTokenService : BaseService<PreviewToken>, IPreviewTokenService
    {
        public PreviewTokenService(IRepository<PreviewToken> selfRepository)
            : base(selfRepository)
        {

        }

    }
}
