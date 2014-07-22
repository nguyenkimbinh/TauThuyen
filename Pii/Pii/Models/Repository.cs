using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Pii.Models
{
    public abstract class Repository<T> where T : class
    {
        private PiiContext _dataContext;
        private readonly IDbSet<T> _dbset;
        private HttpContextBase _httpContext;
        protected Repository(PiiContext dataContext, HttpContextBase httpContext)
        {
            _dataContext = dataContext;
            _dbset = DataContext.Set<T>();
            _httpContext = httpContext;
        }

        protected PiiContext DataContext
        {
            get { return _dataContext ?? (_dataContext = new PiiContext()); }
        }

        protected HttpContextBase HttpContext
        {
            get { return _httpContext; }
        }
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault<T>();
        }
    }
}