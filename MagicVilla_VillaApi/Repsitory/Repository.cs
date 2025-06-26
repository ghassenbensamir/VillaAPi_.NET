using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Repsitory.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaApi.Repsitory
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public ApplicationDBContext _db { get; }
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDBContext db)
        {
            this._db = db;
            this._dbSet = _db.Set<T>();

        }
        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
           await Save();
        }

        public async Task<T> Get(Expression<Func<T,bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
             }

            if (filter != null)
            {
                query = query.Where(filter);
            }
           
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null,int pageSize=3,int pageNumber=1)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }
                query=query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            await Save();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        
    }
}