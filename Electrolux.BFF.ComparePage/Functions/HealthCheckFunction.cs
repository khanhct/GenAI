using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Electrolux.BFF.ComparePage.Functions;

public class HealthCheckFunction
{
    [Function("HealthCheck")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req)
    {
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "application/json; charset=utf-8");

        response.WriteString("""{"status": "healthy"}""");
        return response;
    }
} 