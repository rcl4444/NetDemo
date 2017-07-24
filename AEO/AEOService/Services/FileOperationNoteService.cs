using AEOPoco.Domain;
using AEOService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Interface;

namespace AEOService.Services
{
    public class FileOperationNoteService: BaseService<FileOperationNote>,IFileOperationNoteService
    {
        public FileOperationNoteService(IRepository<FileOperationNote> fileOperationNoteRepository) : base(fileOperationNoteRepository)
        {

        }

        public override FileOperationNote GetByID(object id)
        {
            return this.Query.Where(o=>o.Id == (int)id).FirstOrDefault();
        }
    }
}
