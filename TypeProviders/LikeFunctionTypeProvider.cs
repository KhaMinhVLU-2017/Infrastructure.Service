using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Infrastructure.Services.TypeProviders
{
    public class LikeFunctionTypeProvider : DefaultDynamicLinqCustomTypeProvider
    {
        public LikeFunctionTypeProvider() : base(ParsingConfig.Default, true) { }

        public override HashSet<Type> GetCustomTypes() => new[] { typeof(EF), typeof(DbFunctions), typeof(DbFunctionsExtensions) }.ToHashSet();
    }
}