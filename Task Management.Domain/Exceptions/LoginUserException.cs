
namespace Task_Management.Domain.Exceptions
{
    public class LoginUserException : Exception
    {
        public LoginUserException(string Message) : base(Message) { }
    }
}
