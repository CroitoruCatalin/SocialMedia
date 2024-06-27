using SocialMedia.Models;
using SocialMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SocialMedia.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected SocialContext _SocialContext { get; set; }

        public RepositoryBase(SocialContext socialContext)
        {
            this._SocialContext = socialContext;
        }

        public IQueryable<T> FindAll()
        {
            return this._SocialContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this._SocialContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this._SocialContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this._SocialContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this._SocialContext.Set<T>().Remove(entity);
        }
    }
}
