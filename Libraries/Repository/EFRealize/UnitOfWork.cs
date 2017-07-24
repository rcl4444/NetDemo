using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EFRealize
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly IDbContext _dbContext;
        private DbContextTransaction _transaction;
        private bool _isCommit;

        public UnitOfWork(IDbContext dbContext, DbContextTransaction transaction)
        {
            this._dbContext = dbContext;
            this._transaction = transaction;
        }

        private void Committing()
        {
            this._dbContext.SaveChanges();
            this._transaction.Commit();
        }

        public virtual void Commit()
        {
            this.Committing();
            this._isCommit = true;
        }

        public virtual void Rollback()
        {
            this._isCommit = true;
            this._transaction.Rollback();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (!_isCommit)
                    {
                        this.Committing();
                    }
                    this._dbContext.UnitOfWorkOver();
                    this._transaction.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
