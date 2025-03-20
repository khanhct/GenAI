using System.Collections.Generic;

namespace Electrolux.BFF.ComparePage.Dtos
{
    public class ComparePageRequest
    {
        public List<string> ProductIds { get; set; }
        public string Market { get; set; }
        public string Language { get; set; }
    }
} 