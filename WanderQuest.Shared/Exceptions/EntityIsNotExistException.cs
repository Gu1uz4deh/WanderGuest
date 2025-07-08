using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderQuest.Shared.Exceptions
{
    public class EntityIsNotExistException : Exception
    {
        private const string defaultMessage = "This entity does not exist";
        public EntityIsNotExistException() :this(defaultMessage)
        {
            
        }
        public EntityIsNotExistException(string message) : base(message)
        {

        }
    }
}
