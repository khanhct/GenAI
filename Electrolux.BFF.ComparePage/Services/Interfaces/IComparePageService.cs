using System.Threading.Tasks;
using Electrolux.BFF.ComparePage.Dtos;

namespace Electrolux.BFF.ComparePage.Services.Interfaces
{
    public interface IComparePageService
    {
        Task<ComparePageResponse> GetComparePageDataAsync(ComparePageRequest request, string correlationId);
    }
} 