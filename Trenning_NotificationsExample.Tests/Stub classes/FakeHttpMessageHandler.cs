using System.Net;

namespace Trenning_NotificationsExample.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<string, byte[]> _responses;

        public FakeHttpMessageHandler(Dictionary<string, byte[]> responses)
        {
            _responses = responses;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_responses.TryGetValue(request.RequestUri.ToString(), out var content))
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(content)
                });
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
