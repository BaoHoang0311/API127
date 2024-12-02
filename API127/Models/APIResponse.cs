using System.Net;

namespace API127.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get;set;}
        public bool IsSuccess { get;set;}
        public List<string > ErrorMessages { get;set;} = new List<string>();
        public object Result { get;set; }
    }
}
