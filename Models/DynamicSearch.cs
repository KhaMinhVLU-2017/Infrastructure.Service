using System;
using System.Collections.Generic;

namespace Infrastructure.Service.Model
{
    public class DynamicSearch
    {
        public IDictionary<string, string> LimitOperates { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public string[] HiddenKeys { get; set; }
    }
}