using ModernHttpClient;
using Newtonsoft.Json;
using Polly;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Yol.Punla.AttributeBase;
using Yol.Punla.Barrack;
using Yol.Punla.Mapper;

namespace Yol.Punla.GatewayAccess
{
    [ModuleIgnore]
    public abstract class GatewayServiceBase
    {
        private string _companyName = "HopePH";
        public string CompanyName { get => _companyName; }

        private readonly string _baseAPI = AppSettingsProvider.Instance.GetValue("RestBaseApi");
        public string BaseAPI => _baseAPI;

        private readonly IServiceMapper _serviceMapper;
        public IServiceMapper ServiceMapper { get => _serviceMapper; }

        private readonly HttpClient _httpClient;
        public HttpClient HttpClient { get => _httpClient; }        

        protected GatewayServiceBase(IServiceMapper serviceMapper)
        {
            _serviceMapper = serviceMapper;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    _httpClient = new HttpClient();
                    break;
                case Device.Android:
                case Device.UWP:
                default:
                    _httpClient = new HttpClient(new NativeMessageHandler());
                    break;
            }
        }

        protected async Task<T> GetRemoteAsync<T>(string endPoint)
        {
            HttpResponseMessage jsonResponse = new HttpResponseMessage();

            try
            {
                jsonResponse = await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync
                        (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3)
                        )
                       .ExecuteAsync(async () =>
                       {
                           var result = await HttpClient.GetAsync(endPoint).ConfigureAwait(false);
                           return result;
                       });

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var jsonResult = await jsonResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(jsonResult))
                        return JsonConvert.DeserializeObject<T>(jsonResult);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("HOPEPH An error was in GatewayServiceBase.GetRemoteAsync with message {0}", exception.Message));

                if(exception.InnerException != null)
                    Debug.WriteLine(string.Format("HOPEPH An error was in GatewayServiceBase.GetRemoteAsync with message {0}", exception.InnerException.Message));

                throw;
            }

            throw new ArgumentException(string.Format("Not successful in getting results from this api {0}.", endPoint));
        }

        protected async Task<T> PostRemoteAsync<T>(string endPoint, HttpContent httpContent)
        {
            HttpResponseMessage jsonResponse = new HttpResponseMessage();

            try
            {
                jsonResponse = await Policy
                        .Handle<Exception>()
                        .WaitAndRetryAsync
                        (
                          retryCount: 3,
                          sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3)
                        )
                       .ExecuteAsync(async () =>
                       {
                           var result = await _httpClient.PostAsync(endPoint, httpContent).ConfigureAwait(false);
                           return result;
                       });

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var jsonResult = await jsonResponse.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (!string.IsNullOrWhiteSpace(jsonResult))
                    {
                        var deserializedObject = JsonConvert.DeserializeObject<T>(jsonResult);
                        return deserializedObject;
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(string.Format("HOPEPH An error was in GatewayServiceBase.GetRemoteAsync with message {0}", exception.Message));

                if (exception.InnerException != null)
                    Debug.WriteLine(string.Format("HOPEPH An error was in GatewayServiceBase.GetRemoteAsync with message {0}", exception.InnerException.Message));

                throw;
            }

            throw new ArgumentException(string.Format("Not successful in getting results from this api {0}.", endPoint));
        }

        protected StringContent CastToStringContent<T>(object item)
        {
            var jsonString = JsonConvert.SerializeObject((T)item);
            var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            return jsonContent;
        }
    }
}
