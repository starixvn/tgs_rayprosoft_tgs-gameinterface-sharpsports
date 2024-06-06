using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.SharpSports.Core.Extensions;

namespace Tgs.GameInterface.SharpSports.Core;

internal sealed partial class GameApiClient(
	ILogger<GameApiClient> logger,
	HttpClient httpClient,
	IParameterEncryptor encryptor)
	: IGameApiClient
{
	private readonly DateTimeConverter _dateTimeConverter = new();

	private async ValueTask<JsonNode> PostAsync(
		Uri apiBaseUrl,
		string merchantId,
		string clientId,
		string clientSecret,
		string relativeUrl,
		JsonObject parameters,
		string logPrefix,
		CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var encoded = encryptor.Encrypt(parameters, clientSecret);
		if (string.IsNullOrWhiteSpace(encoded))
			throw new Exception("encrypted parameter is empty");
		var requestContent = JsonContent.Create(new JsonObject
		{
			["data"] = encoded,
			["merchantId"] = merchantId,
		});

		var apiUrl = new Uri(apiBaseUrl, relativeUrl);
		var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
		{
			Content = requestContent,
		};
		request.Headers.Add("client_id", clientId);

		var sw = Stopwatch.StartNew();

		var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		var apiLogEvent = new
		{
			Path = request?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = request?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = request?.Headers + request?.Content?.Headers?.ToString(),
			Requestoarameters = parameters.ToJsonString(),
			RequestContent = (await requestContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = response.Headers.ToString(),
			ResponseContent = responseContent.NoLongerThan(4096),
			ResponseCode = (short)response.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		response.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseContent)!;
	}
}
