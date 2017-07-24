using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Linq.Expressions;
using Repository.Interface;

namespace AEOService.Interface
{
    public interface IBaseService<T> where T : BaseEntity
    {
        IQueryable<T> NoTrackingQuery { get; }

        IQueryable<T> Query { get; }

        IUnitOfWork BeginTransaction();

        T GetByID(object id);

        void Add(T obj);

        void Update(T obj);

        void Add(IEnumerable<T> objs);

        void Update(IEnumerable<T> objs);

        IPagedList<T> Search(Expression<Func<T, bool>> where);

        IQueryable<T> FilterQuery(Expression<Func<T, bool>> where);

        IEnumerable<T> SqlQuery(string sql, params object[] parameters);

        IEnumerable<Ti> SqlQuery<Ti>(string sql, params object[] parameters);

        int ExecuteSqlCommand(string sql, params object[] parameters);
    }
}
