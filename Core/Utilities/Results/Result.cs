using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message):this(success) // 1 parametre gelirse zaten success, 2 parametre gelirse hem success hem de message tetiklendi.
        {
            Message = message;
            // Read only message ı burada set ettim 
        }

        public Result(bool success)
        {
            Success = success;
        }

        public bool Success { get; }
        public string Message { get; }
    }
    
}
