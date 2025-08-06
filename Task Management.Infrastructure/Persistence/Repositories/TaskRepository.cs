using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : IRepository<TaskItem>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TaskItem> DbSet;

        public TaskRepository(AppDbContext context)
        {
            this._context = context;
            DbSet = context.Set<TaskItem>();
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(Expression<Func<TaskItem, bool>>? Filter, string? IncludeProperties)
        {
            IQueryable<TaskItem> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.ToListAsync();
        }

        public async Task<TaskItem> GetAsync(Expression<Func<TaskItem, bool>> Filter, string? IncludeProperties = null)
        {
            IQueryable<TaskItem> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.FirstOrDefaultAsync() ?? throw new ArgumentException("Entity not found with the provided filter.");
        }

        public async Task AddAsync(TaskItem Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            await DbSet.AddAsync(Entity);
        }

        public void Update(TaskItem Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            DbSet.Update(Entity);
        }

        public async Task RemoveAsync(int ID)
        {
            if (ID <= 0)
                throw new ArgumentException("UserID must be greater than 0", nameof(ID));

            TaskItem TaskItem = await GetAsync(c => c.ID == ID);
            DbSet.Remove(TaskItem);
        }

        public async Task RemoveRangeAsync(IEnumerable<TaskItem> Entities)
        {
            if (Entities == null || !Entities.Any())
                throw new ArgumentException("No TaskItems to remove.", nameof(Entities));

            DbSet.RemoveRange(Entities);
            await Task.CompletedTask;
        }
    }
}
