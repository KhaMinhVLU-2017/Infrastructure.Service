using System;
using System.Collections.Generic;

namespace Infrastructure.Service.Model
{
    public class SearchRestrictionDictionary : Dictionary<string, string>
    {
        public SearchRestrictionDictionary() : base(StringComparer.InvariantCultureIgnoreCase)
        { }
    }
}