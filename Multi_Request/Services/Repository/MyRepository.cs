using Microsoft.EntityFrameworkCore;
using Multi_Request.Data;
using Multi_Request.Models;

namespace Multi_Request.Services.Repository
{
    public class MyRepository<TEntity> where TEntity : class
    {
        private readonly DataContext _context;
        public MyRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<List<TEntity>>> GetAll()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return new ServiceResponse<List<TEntity>> { Data = entities, Success = true };
        }
    }
}
