using Dot.Application.Interfaces;
using Dot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dot.Infrastructure.Services
{
    public class EntityRepository : EntityRepositoryReadOnly, IRepository
    {
        private readonly ApplicationDbContext _context;
        public EntityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public virtual void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class
        {
            _context.Set<TEntity>().Add(entity);
        }

        public virtual void Delete<TEntity>(object id) where TEntity : class
        {
            TEntity entity = _context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = _context.Set<TEntity>();
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                ThrowEnhancedValidationException(e);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                ThrowEnhancedValidationException(e);
            }

            return Task.FromResult(0);
        }

        public virtual void Update<TEntity>(TEntity entity, string createdBy = null) where TEntity : class
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        protected virtual void ThrowEnhancedValidationException(DbUpdateException e)
        {
            throw new DbUpdateException(e.Message, e.InnerException);
        }
    }
}
