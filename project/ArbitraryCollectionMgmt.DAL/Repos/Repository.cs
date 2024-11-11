using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ArbitraryCollectionMgmt.DAL.Interfaces;
using ArbitraryCollectionMgmt.DAL.Models;

namespace ArbitraryCollectionMgmt.DAL.Repos
{
    internal class Repository<T> : IRepo<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public T Create(T obj)
        {
            dbSet.Add(obj);
            if (_db.SaveChanges() > 0) return obj;
            return null;
        }

        public bool Delete(T obj)
        {
            dbSet.Remove(obj);
            return _db.SaveChanges() > 0;
        }

        public IEnumerable<T> GetAll(string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking().ToList();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking().ToList();
        }
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.AsNoTracking().FirstOrDefault();
        }

        public bool Update(T obj)
        {   
            dbSet.Update(obj);
            return _db.SaveChanges() > 0;
        }

        public bool Delete(IEnumerable<T> objs)
        {
            dbSet.RemoveRange(objs);
            return _db.SaveChanges() > 0;
        }     
    }
}
