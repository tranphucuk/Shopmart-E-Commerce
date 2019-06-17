using System;
using System.Collections.Generic;
using System.Text;

namespace NetcoreOnlineShop.Utilities.Dtos
{
    public class GenericResult
    {
        public GenericResult()
        {

        }

        public GenericResult(bool success)
        {
            Success = success;
        }

        public GenericResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public GenericResult(bool success, object data)
        {
            Success = success;
            Data = data;
        }

        public GenericResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public GenericResult(object data)
        {
            Data = data;
        }

        public GenericResult(string message, object data)
        {
            Message = message;
            Data = data;
        }

        public GenericResult(bool success, string message, object data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
