using FluentValidation;

namespace Task_Management.Application.TaskOperations.GetTask
{
    class GetTaskValidator : AbstractValidator<GetTaskQuery>
    {
        public GetTaskValidator()
        {
            RuleFor(x => x.TaskID)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Task ID is required.")
                .GreaterThan(0).WithMessage("Task ID must be greater than 0.");
        }
    }
}