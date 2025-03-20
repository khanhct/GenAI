using System.Collections.Generic;

namespace Electrolux.BFF.ComparePage.Dtos
{
    public class ComparePageResponse
    {
        public List<ProductCompareData> Products { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ProductCompareData
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Features { get; set; }
        public Dictionary<string, string> Specifications { get; set; }
    }
} 