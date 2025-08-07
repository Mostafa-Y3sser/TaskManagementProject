using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Management.Domain.Exceptions
{
    public class CreateUserException : Exception
    {
        public CreateUserException(string Message) : base(Message) { }
    }
}
