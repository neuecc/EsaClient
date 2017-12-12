using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EsaClient
{
    public class BearerAuthenticationMessageHandler : DelegatingHandler
    {
        readonly System.Net.Http.Headers.AuthenticationHeaderValue authHeader;

        public BearerAuthenticationMessageHandler(string token)
            : this(token, null)
        {
        }

        public BearerAuthenticationMessageHandler(string token, HttpMessageHandler handler)
            : base(handler ?? new HttpClientHandler())
        {
            authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = authHeader;
            return base.SendAsync(request, cancellationToken);
        }
    }
}
