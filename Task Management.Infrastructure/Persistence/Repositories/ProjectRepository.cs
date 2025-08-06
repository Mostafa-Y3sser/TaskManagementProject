using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IRepository<Project>
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Project> DbSet;

        public ProjectRepository(AppDbContext context)
        {
            this._context = context;
            DbSet = context.Set<Project>();
        }

        public async Task<IEnumerable<Project>> GetAllAsync(Expression<Func<Project, bool>>? Filter, string? IncludeProperties)
        {
            IQueryable<Project> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.ToListAsync();
        }

        public async Task<Project> GetAsync(Expression<Func<Project, bool>> Filter, string? IncludeProperties = null)
        {
            IQueryable<Project> Query = DbSet;

            if (Filter != null)
                Query = Query.Where(Filter);

            if (!String.IsNullOrWhiteSpace(IncludeProperties))
                foreach (string IncludeProp in IncludeProperties!.Split(",", StringSplitOptions.RemoveEmptyEntries))
                    Query = Query.Include(IncludeProp.Trim());

            return await Query.FirstOrDefaultAsync() ?? throw new ArgumentException("Entity not found with the provided filter.");
        }

        public async Task AddAsync(Project Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            await DbSet.AddAsync(Entity);
        }

        public void Update(Project Entity)
        {
            if (Entity == null)
                throw new ArgumentNullException(nameof(Entity));

            DbSet.Update(Entity);
        }

        public async Task RemoveAsync(int ID)
        {
            if (ID <= 0)
                throw new ArgumentException("UserID must be greater than 0", nameof(ID));

            Project Project = await GetAsync(c => c.ID == ID);
            DbSet.Remove(Project);
        }

        public async Task RemoveRangeAsync(IEnumerable<Project> Entities)
        {
            if (Entities == null || !Entities.Any())
                throw new ArgumentException("No Projects to remove.", nameof(Entities));

            DbSet.RemoveRange(Entities);
            await Task.CompletedTask;
        }
    }
}
