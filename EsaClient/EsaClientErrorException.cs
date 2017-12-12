using System;
using System.Collections.Generic;
using System.Text;

namespace EsaClient
{
    public class EsaClientErrorException : Exception
    {
        readonly string error;
        readonly string message;

        public EsaClientErrorException(string error, string message)
        {
            this.error = error;
            this.message = message;
        }

        public override string ToString()
        {
            return error + ": " + message;
        }
    }
}
