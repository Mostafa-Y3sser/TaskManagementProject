using System;

namespace Task_Management.Application.Dtos
{
    public class ProjectDto
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<BoardDto> BoardDtos { get; set; } = new();
    }
}