using System.Net;
using System.Net.Http;

namespace Yol.Punla.UnitTest.WebAPI.Barrack
{
    public class APIHttpResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
    }
}
