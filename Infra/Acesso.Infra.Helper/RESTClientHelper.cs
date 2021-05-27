using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Acesso.Infra.Helper {

    public class RESTClientHelper {

        private readonly Uri BaseUrlUri;
        private HttpClient client = new HttpClient();

        public RESTClientHelper(string baseUrl) {
            BaseUrlUri = new Uri(baseUrl);
            client.BaseAddress = BaseUrlUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient getClient() {
            return client;
        }

        public HttpClient getClientWithBearer(string token) {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }



}
