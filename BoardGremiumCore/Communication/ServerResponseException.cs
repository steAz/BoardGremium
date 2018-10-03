using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGremiumCore.Communication
{
    public class ServerResponseException : Exception
    {
        public ServerResponseException(string message) :base("Exception while communicating with server: " + message)
        {
            
        }
    }
}
