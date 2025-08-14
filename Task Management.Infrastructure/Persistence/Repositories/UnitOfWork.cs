using Task_Management.Domain.Entities;
using Task_Management.Domain.Interfaces;
using Task_Management.Infrastructure.Persistence.Data;

namespace Task_Management.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Project> ProjectRepository { get; private set; }
        public IRepository<Board> BoardRepository { get; private set; }
        public IRepository<TaskItem> TaskRepository { get; private set; }

        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            ProjectRepository = new ProjectRepository(context);
            BoardRepository = new BoardRepository(context);
            TaskRepository = new TaskRepository(context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}