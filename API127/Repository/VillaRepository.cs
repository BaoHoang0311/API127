using API127.Data;
using API127.Models;
using API127.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace API127.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly  ApplicationDbContext _context;
        public VillaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Villa entity)
        {
            await _context.AddAsync(entity);
            await SaveAsync();
        }
        public async Task<List<Villa>> GetAllAsync(Expression<Func<Villa, bool>> filter = null)
        {
            IQueryable<Villa> query = _context.Villas;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        {
            IQueryable<Villa> query = _context.Villas;
            if(!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }



        public async Task RemoveAsync(Villa entity)
        {
            _context.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Villa entity)
        {
           _context.Update(entity);
            await SaveAsync();
        }
    }
}
