using System.Net;
using Microsoft.Azure.Functions.Worker.Http;
using Moq;
using Xunit;
using Electrolux.BFF.ComparePage.Functions;

namespace Electrolux.BFF.ComparePage.Tests.Functions;

public class HealthCheckFunctionTests
{
    [Fact]
    public void HealthCheck_ReturnsOkResponse()
    {
        // Arrange
        var mockRequest = new Mock<HttpRequestData>();
        var mockResponse = new Mock<HttpResponseData>(mockRequest.Object);
        
        mockRequest.Setup(r => r.CreateResponse()).Returns(mockResponse.Object);
        
        var function = new HealthCheckFunction();

        // Act
        var response = function.Run(mockRequest.Object);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
} 