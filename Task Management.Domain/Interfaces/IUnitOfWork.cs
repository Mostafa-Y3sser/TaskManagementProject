using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Management.Domain.Entities;

namespace Task_Management.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Project> ProjectRepository { get; }
        IRepository<Board> BoardRepository { get; }
        IRepository<TaskItem> TaskRepository { get; }

        Task SaveAsync();
    }
}
