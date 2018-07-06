using System.Net;
using Yol.Punla.AttributeBase;

namespace Yol.Punla.Messages
{
    [ModuleIgnore]
    public class HttpResponseMessage<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public T Result { get; set; }
    }
}
