using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PSWeb_Server.Models
{
    public class APIResponse<T>
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set;}
        public T Data { get; set;}
        public  object Error { get; set;}
        //Request Success
        public APIResponse(T data, string message = "", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Success = true; 
            StatusCode = statusCode;
            Message =  message;
            Data = data;
            Error = null;
        }
        // Request Failed
        public APIResponse(HttpStatusCode statusCode, string message, object error = null)
        {
            Success = false;
            StatusCode = statusCode;
            Message = message;
            Data = default(T);
            Error = error;
        }
    }
}