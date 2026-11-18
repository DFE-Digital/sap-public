using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SAPPub.Infrastructure.EES.Config;
using SAPPub.Infrastructure.EES.Entities;
using SAPPub.Infrastructure.Repositories;
using SAPPub.Web.Models.Config;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace SAPPub.Infrastructure.EES.Helpers
{
    public interface IEESClient
    {
        Task<EESResponseModel> GetDataQuery(Guid dataSetId, string dataSetVersion, EESRequestModel requestModel, CancellationToken cancellationToken = default);
        Task<EESMetadataModel> GetMetadata(Guid dataSetId, string dataSetVersion, CancellationToken cancellationToken = default);
    }

    public class EESClient : IEESClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EESClient> _logger;

        private static readonly TimeSpan[] RetryDelays =
        [
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)
        ];

        private static readonly HttpStatusCode[] retryStatuses =
        [
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError
        ];

        public EESClient(HttpClient httpClient, ILogger<EESClient> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(EESConstants.BaseAPIUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _logger = logger;
        }

        public async Task<EESResponseModel> GetDataQuery(
            Guid dataSetId,
            string dataSetVersion,
            EESRequestModel requestModel,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("EES: Queried data with {0} and version {1}", dataSetId, dataSetVersion);

            var response = await ExecuteWithRetryAsync(async () =>
            {
                using var requestMessage = new HttpRequestMessage(
                    HttpMethod.Post, $"/statistics/v1/data-sets/{dataSetId}/query?dataSetVersion={dataSetVersion}");
                requestMessage.Headers.Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Accept", "application/json");
                requestMessage.Content = JsonContent.Create(requestModel);
                return await _httpClient.SendAsync(requestMessage, cancellationToken);
            }, cancellationToken);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    _logger.LogError("EES: Query content null with {dataSetId} and version {dataSetVersion}", dataSetId, dataSetVersion);
                    return new EESResponseModel();
                }

                var result = JsonConvert.DeserializeObject<EESResponseModel>(responseContent);
                return result ?? new EESResponseModel();
            }

            _logger.LogError("EES: Query response null with {dataSetId} and version {dataSetVersion}", dataSetId, dataSetVersion);
            return new EESResponseModel();
        }

        public async Task<EESMetadataModel> GetMetadata(
            Guid dataSetId,
            string dataSetVersion,
            CancellationToken cancellationToken = default)
        {

            //Logging

            var response = await ExecuteWithRetryAsync(async () =>
            {
                using var requestMessage = new HttpRequestMessage(
                    HttpMethod.Get, $"/statistics/v1/data-sets/{dataSetId}/meta?dataSetVersion={dataSetVersion}");
                requestMessage.Headers.Add("Content-Type", "application/json");
                requestMessage.Headers.Add("Accept", "application/json");
                return await _httpClient.SendAsync(requestMessage, cancellationToken);
            }, cancellationToken);

            if (response != null && response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    _logger.LogError("EES: Query content null with {dataSetId} and version {dataSetVersion}", dataSetId, dataSetVersion);

                    return new EESMetadataModel();
                }
                var result = JsonConvert.DeserializeObject<EESMetadataModel>(responseContent);
                return result ?? new EESMetadataModel();
            }

            _logger.LogError("EES: Metadata response null with {dataSetId} and version {dataSetVersion}", dataSetId, dataSetVersion);
            return new EESMetadataModel();
        }



        private static bool ShouldRetry(HttpResponseMessage? response)
        {
            if (response == null)
            {
                return true;
            }

            if (retryStatuses.Contains(response.StatusCode))
            {
                return true;
            }

            var statusCode = (int)response.StatusCode;
            return statusCode == 429 || statusCode >= 500;
        }

        private static async Task<HttpResponseMessage?> ExecuteWithRetryAsync(
                Func<Task<HttpResponseMessage>> operation,
                CancellationToken cancellationToken)
        {
            for (var attempt = 0; attempt <= RetryDelays.Length; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    var response = await operation();
                    if (!ShouldRetry(response) || attempt == RetryDelays.Length)
                    {
                        return response;
                    }
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    if (attempt == RetryDelays.Length)
                    {
                        return null;
                    }
                }

                await Task.Delay(RetryDelays[attempt], cancellationToken);
            }

            return null;
        }
    }
}
