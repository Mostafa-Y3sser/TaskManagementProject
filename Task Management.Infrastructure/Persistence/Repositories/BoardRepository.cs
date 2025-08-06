using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Persistence.Repositories
{
    public class BoardRepository : IRepository<Board>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Board> DbSet;

        public BoardRepository(AppDbContext context)
        {
            this._context = context;
            DbSet = context.Set<Board>();
        }

        public async Task<IEnumerable<Board>> GetAllAsync(Expression<Func<Board, bool>>? Filter, string? IncludeProperties)
        {
            IQueryable<Board> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.ToListAsync();
        }

        public async Task<Board> GetAsync(Expression<Func<Board, bool>> Filter, string? IncludeProperties = null)
        {
            IQueryable<Board> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.FirstOrDefaultAsync() ?? throw new ArgumentException("Entity not found with the provided filter.");
        }

        public async Task AddAsync(Board Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            await DbSet.AddAsync(Entity);
        }

        public void Update(Board Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            DbSet.Update(Entity);
        }

        public async Task RemoveAsync(int ID)
        {
            if (ID <= 0)
                throw new ArgumentException("UserID must be greater than 0", nameof(ID));

            Board Board = await GetAsync(c => c.ID == ID);
            DbSet.Remove(Board);
        }

        public async Task RemoveRangeAsync(IEnumerable<Board> Entities)
        {
            if (Entities == null || !Entities.Any())
                throw new ArgumentException("No Boards to remove.", nameof(Entities));

            DbSet.RemoveRange(Entities);
            await Task.CompletedTask;
        }
    }
}