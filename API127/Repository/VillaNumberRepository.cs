using API127.Data;
using API127.Repository.IRepository;
using API127.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API127.Repository
{
    public class VillaNumberRepository : Repository<API127.Models.VillaNumber>, IVillaNumberRepository
    {
        private readonly  ApplicationDbContext _context;
        public VillaNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       
    }
}
