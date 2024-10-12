using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace Mss.App.Logger.Utils.RequestsUtils;

public static class RequestInfoExtractor
{
    public static string GetFullUrl(HttpRequest request)
    {
        var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        return fullUrl;
    }

    public static string GetHeadersByLine(HttpRequest request)
    {
        var headers = new List<string>();

        var headersCollection = request.Headers;

        foreach (var header in headersCollection)
        {
            headers.Add($"Header: {header.Key} - Value: {header.Value}");
        }

        return string.Join("; ", headers);

    }
}
