using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Electrolux.BFF.ComparePage.Services.Interfaces;
using Electrolux.BFF.ComparePage.Dtos;
using Electrolux.BFF.ComparePage.Constants;
using System.Net;

namespace Electrolux.BFF.ComparePage.Functions
{
    public class ComparePageFunction
    {
        private readonly IComparePageService _comparePageService;
        private readonly ILogger<ComparePageFunction> _logger;

        public ComparePageFunction(
            IComparePageService comparePageService,
            ILogger<ComparePageFunction> logger)
        {
            _comparePageService = comparePageService ?? throw new ArgumentNullException(nameof(comparePageService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [FunctionName("GetComparePageData")]
        public async Task<IActionResult> GetComparePageData(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = Constants.Routes.ComparePageEndpoint)] ComparePageRequest request,
            HttpRequest httpRequest)
        {
            try
            {
                _logger.LogInformation("Processing compare page request");

                var correlationId = httpRequest.Headers[Constants.Headers.CorrelationId].ToString();
                var market = httpRequest.Headers[Constants.Headers.Market].ToString();
                var language = httpRequest.Headers[Constants.Headers.Language].ToString();

                var response = await _comparePageService.GetComparePageDataAsync(request, correlationId);
                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing compare page request");
                return new ObjectResult(new ComparePageResponse 
                { 
                    Success = false, 
                    ErrorMessage = "An error occurred processing your request" 
                })
                { 
                    StatusCode = (int)HttpStatusCode.InternalServerError 
                };
            }
        }
    }
} 