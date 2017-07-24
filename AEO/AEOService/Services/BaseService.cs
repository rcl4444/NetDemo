using AEOService.Interface;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Repository.Interface;

namespace AEOService.Services
{
    public abstract class BaseService<T>: IBaseService<T> where T : BaseEntity
    {
        private readonly IRepository<T> _selfRepository;

        public BaseService(IRepository<T> selfRepository)
        {
            this._selfRepository = selfRepository;
        }

        public IUnitOfWork BeginTransaction()
        {
            return this._selfRepository.GetUnitOfWork();
        }

        public IQueryable<T> NoTrackingQuery
        {
            get { return this._selfRepository.TableNoTracking; }
        }

        public IQueryable<T> Query
        {
            get { return this._selfRepository.Table; }
        }

        public virtual void Add(T obj)
        {
            this._selfRepository.Insert(obj);
        }

        public virtual void Update(T obj)
        {
            this._selfRepository.Update(obj);
        }

        public virtual void Add(IEnumerable<T> objs)
        {
            this._selfRepository.Insert(objs);
        }

        public virtual void Update(IEnumerable<T> objs)
        {
            this._selfRepository.Update(objs);
        }

        public virtual T GetByID(object id)
        {
            return this._selfRepository.GetById(id);
        }

        public virtual IPagedList<T> Search(Expression<Func<T, bool>> where)
        {
            IQueryable<T> query;
            if (where == null)
            {
                query = this._selfRepository.TableNoTracking.OrderBy(o => o.Id);
            }
            else
            {
                query = this._selfRepository.TableNoTracking.Where(where).OrderBy(o => o.Id);
            }
            return new PagedList<T>(query, 0, int.MaxValue);
        }

        public IQueryable<T> FilterQuery(Expression<Func<T, bool>> where)
        {
            IQueryable<T> query;
            if (where == null)
            {
                query = this._selfRepository.TableNoTracking;
            }
            else
            {
                query = this._selfRepository.TableNoTracking.Where(where);
            }
            return query;
        }

        public IEnumerable<T> SqlQuery(string sql, params object[] parameters)
        {
            return this._selfRepository.SqlQuery(sql,parameters).ToList();
        }

        public IEnumerable<Ti> SqlQuery<Ti>(string sql, params object[] parameters)
        {
            return this._selfRepository.SqlQuery<Ti>(sql, parameters).ToList();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return this._selfRepository.ExecuteSqlCommand(sql, parameters);
        }
    }
}
