using MediatR;
using Microsoft.AspNetCore.Mvc;
using Task_Management.Responses;
using Task_Management.Application.TaskOperations.CreateTask;
using Task_Management.Application.Dtos;
using Task_Management.Application.TaskOperations.GetTask;
using Task_Management.Application.TaskOperations.UpdateTask;
using Task_Management.Application.TaskOperations.DeleteTask;

namespace Task_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult<ApiResponse<int>>> CreateTask(CreateTaskCommand command)
        {
            int TaskID = await _mediator.Send(command);

            return Ok(ApiResponse<int>.Success(TaskID));
        }

        [HttpGet("GetTask/{ID:int}")]
        public async Task<ActionResult<ApiResponse<TaskDto>>> GetTask(int ID)
        {
            TaskDto taskDto = await _mediator.Send(new GetTaskQuery { TaskID = ID });

            return Ok(ApiResponse<TaskDto>.Success(taskDto));
        }

        [HttpPut("UpdateTask/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateTask([FromRoute] int ID, [FromBody] UpdateTaskCommand command)
        {
            command.TaskID = ID;

            bool Result = await _mediator.Send(command);

            return Ok(ApiResponse<bool>.Success(Result, "Task updated successfully."));
        }

        [HttpDelete("DeleteTask/{ID:int}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteTask([FromRoute] int ID)
        {
            bool Result = await _mediator.Send(new DeleteTaskCommand { TaskID = ID });
            return Ok(ApiResponse<bool>.Success(Result, "Task deleted successfully."));
        }
    }
}