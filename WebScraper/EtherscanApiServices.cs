using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace WebScraper
{
    public class EtherscanApiServices : IEtherscanApiServices
    {
        private readonly Uri _baseUri;

        const int _secondToMs = 1000;
        private int _apiCallsLeft;
        private readonly Stopwatch _msSinceApiLimitStarted = new Stopwatch();
        private readonly ApiOptions _config;
        private readonly ILogger _logger;

        public EtherscanApiServices(IOptions<ApiOptions> config, ILogger<EtherscanApiServices> logger)
        {
            _logger = logger;
            _config = config.Value;
            _baseUri = new Uri(_config.Path);

            _apiCallsLeft = _config.CallsPerSecond;
            _msSinceApiLimitStarted.Start();
        }

        public async Task<TransactionDetails> GetTransactionDetailsAsync(string txnHash)
        {
            WaitIfNeeded();
            _apiCallsLeft--;

            var client = CreateClientForRequest();
            return await MakeRequestsUntilGet(client, txnHash);
        }

        private HttpClient CreateClientForRequest()
        {
            var client = new HttpClient
            {
                BaseAddress = _baseUri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private async Task<string> GetJsonResponse(HttpClient client, string txnHash)
        {
            var parameters = $"?module=proxy&action=eth_getTransactionByHash&txhash={txnHash}&apikey={_config.ApiKey}";
            var response = await client.GetAsync(parameters);
            if (!response.IsSuccessStatusCode)
            {
                var tryAgainDelay = _config.TryAgainDelayInMs.ForResponseApiUnavailable;
                _logger.LogWarning($"From Etherscan API couldn't get transaction {txnHash}\nStatusCode {response.StatusCode}\nStatusCode {response.ReasonPhrase}\nTrying again in {tryAgainDelay/1000}");
                Task.Delay(tryAgainDelay).Wait();
                return await GetJsonResponse(client, txnHash);
            }
            return response.Content.ReadAsStringAsync().Result;
        }

        private async Task<TransactionDetails> MakeRequestsUntilGet(HttpClient client, string txnHash)
        {
            var jsonResponse = await GetJsonResponse(client, txnHash);

            TransactionDetails result = null;
            try
            {
                result = JsonConvert.DeserializeObject<EtherscanTransactionByHashSuccessfulResponse>(jsonResponse).Result;
            }
            catch (JsonSerializationException)
            {
                var failReason = JsonConvert.DeserializeObject<EtherscanTransactionByHashFailedResponse>(jsonResponse).Result;
                switch (failReason)
                {
                    case "Invalid API Key":
                        _logger.LogCritical($"API response: \"{failReason}\"");
                        State.ExitAndLog(new StackTrace(), _logger);
                        break;
                    case "Max rate limit reached":
                        result = HandleRequestLimitReached(client, txnHash);
                        break;
                    default:
                        _logger.LogCritical($"Unexpected API fail response: \"{failReason}\"");
                        State.ExitAndLog(new StackTrace(), _logger);
                        break;
                }
            }

            return result;
        }

        private TransactionDetails HandleRequestLimitReached(HttpClient client, string txnHash)
        {
            Task.Delay(_config.TryAgainDelayInMs.ForCallsPerSecondLimit).Wait();
            var result = MakeRequestsUntilGet(client, txnHash).Result;
            _apiCallsLeft = _config.CallsPerSecond - 2; // just for safety since we don't know how long it took for api to respond
            _msSinceApiLimitStarted.Restart();
            return result;
        }

        private void WaitIfNeeded()
        {
            if (_apiCallsLeft <= 0)
            {
                var msToWait = _secondToMs - (int)_msSinceApiLimitStarted.ElapsedMilliseconds;
                msToWait = msToWait < 0 ? 0 : msToWait;
                Task.Delay(msToWait).Wait();

                _apiCallsLeft = _config.CallsPerSecond;
                _msSinceApiLimitStarted.Restart();
            }
        }
    }
}
