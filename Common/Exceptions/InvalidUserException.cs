using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public class InvalidUserException
    {
        [DataMember]
        public string exceptionMessage;

        public InvalidUserException(string exception) { exceptionMessage = exception; }
    }
}
