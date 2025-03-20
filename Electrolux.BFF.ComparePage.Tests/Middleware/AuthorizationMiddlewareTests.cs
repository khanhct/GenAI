using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Electrolux.BFF.ComparePage.Middleware;
using T1.ReArch.BFF.Core.Services.Interfaces;
using T1.ReArch.BFF.Core.Models;
using Microsoft.Azure.Functions.Worker.Http;

namespace Electrolux.BFF.ComparePage.Tests.Middleware;

public class AuthorizationMiddlewareTests
{
    private readonly Mock<ILogger<AuthorizationMiddleware>> _loggerMock;
    private readonly Mock<IAuthorizationService> _authServiceMock;
    private readonly Mock<IUserContextService> _userContextServiceMock;
    private readonly Mock<FunctionContext> _functionContextMock;
    private readonly Mock<HttpRequestData> _httpRequestMock;
    private readonly AuthorizationMiddleware _middleware;

    public AuthorizationMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<AuthorizationMiddleware>>();
        _authServiceMock = new Mock<IAuthorizationService>();
        _userContextServiceMock = new Mock<IUserContextService>();
        _functionContextMock = new Mock<FunctionContext>();
        _httpRequestMock = new Mock<HttpRequestData>();

        _middleware = new AuthorizationMiddleware(
            _loggerMock.Object,
            _authServiceMock.Object,
            _userContextServiceMock.Object);
    }

    [Fact]
    public async Task Invoke_WhenUserIsAuthorized_ShouldContinueExecution()
    {
        // Arrange
        var url = new Uri("https://localhost/api/page/en-us/content-123");
        _httpRequestMock.Setup(r => r.Url).Returns(url);

        _functionContextMock.Setup(c => c.GetHttpRequestDataAsync())
            .ReturnsAsync(_httpRequestMock.Object);

        _authServiceMock.Setup(s => s.CheckAccessRightAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new AccessRightResult { IsAuthorized = true });

        var executed = false;
        FunctionExecutionDelegate next = (context) =>
        {
            executed = true;
            return Task.CompletedTask;
        };

        // Act
        await _middleware.Invoke(_functionContextMock.Object, next);

        // Assert
        Assert.True(executed);
    }

    [Fact]
    public async Task Invoke_WhenUnauthorizedB2BUser_ShouldReturnForbiddenWithRedirect()
    {
        // Arrange
        var url = new Uri("https://localhost/api/page/en-us/content-123");
        var mockResponse = new Mock<HttpResponseData>(_httpRequestMock.Object);
        
        _httpRequestMock.Setup(r => r.Url).Returns(url);
        _httpRequestMock.Setup(r => r.CreateResponse()).Returns(mockResponse.Object);

        _functionContextMock.Setup(c => c.GetHttpRequestDataAsync())
            .ReturnsAsync(_httpRequestMock.Object);

        _userContextServiceMock.Setup(s => s.GetUserContextAsync())
            .ReturnsAsync(new UserContext { IsAuthenticated = true, IsB2BUser = true });

        _authServiceMock.Setup(s => s.CheckAccessRightAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new AccessRightResult 
            { 
                IsAuthorized = false,
                IsB2BPage = false,
                UnauthenticatedB2BPageUrl = "/b2b-error"
            });

        var executed = false;
        FunctionExecutionDelegate next = (context) =>
        {
            executed = true;
            return Task.CompletedTask;
        };

        // Act
        await _middleware.Invoke(_functionContextMock.Object, next);

        // Assert
        Assert.False(executed);
        Assert.Equal(HttpStatusCode.Forbidden, mockResponse.Object.StatusCode);
    }

    [Fact]
    public async Task Invoke_WhenAnonymousUserAccessingB2BPage_ShouldReturnUnauthorizedWithLoginRedirect()
    {
        // Arrange
        var url = new Uri("https://localhost/api/page/en-us/content-123");
        var mockResponse = new Mock<HttpResponseData>(_httpRequestMock.Object);
        
        _httpRequestMock.Setup(r => r.Url).Returns(url);
        _httpRequestMock.Setup(r => r.CreateResponse()).Returns(mockResponse.Object);

        _functionContextMock.Setup(c => c.GetHttpRequestDataAsync())
            .ReturnsAsync(_httpRequestMock.Object);

        _userContextServiceMock.Setup(s => s.GetUserContextAsync())
            .ReturnsAsync(new UserContext { IsAuthenticated = false });

        _authServiceMock.Setup(s => s.CheckAccessRightAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new AccessRightResult 
            { 
                IsAuthorized = false,
                IsB2BPage = true,
                B2BLoginPageUrl = "/b2b-login"
            });

        var executed = false;
        FunctionExecutionDelegate next = (context) =>
        {
            executed = true;
            return Task.CompletedTask;
        };

        // Act
        await _middleware.Invoke(_functionContextMock.Object, next);

        // Assert
        Assert.False(executed);
        Assert.Equal(HttpStatusCode.Unauthorized, mockResponse.Object.StatusCode);
    }
} 