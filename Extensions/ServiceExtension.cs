using System;
using System.Collections.Generic;
using Infrastructure.Service.Common;
using Infrastructure.Service.Parser;
using Infrastructure.Service.Operate;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Service.Abstraction;

namespace Infrastructure.Service.Extension
{
    public static class ServiceExtension
    {
        public static void AddSearchService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDictionary<string, IOperate>>(services =>
            {
                var comparer = StringComparer.OrdinalIgnoreCase;
                var operate = new Dictionary<string, IOperate>(comparer);
                operate.Add(Constant.EQUAL_OPERATE, new EqualOperate());
                operate.Add(Constant.NOT_EQUAL_OPERATE, new NotEqualOperate());
                operate.Add(Constant.LESS_THAN_OPERATE, new LessThanOperate());
                operate.Add(Constant.LESS_THAN_EQUAL_OPERATE, new LessThanEqualOperate());
                operate.Add(Constant.GREATER_THAN_OPERATE, new GreaterThanOperate());
                operate.Add(Constant.GREATER_THAN_EQUAL_OPERATE, new GreaterThanEqualOperate());
                return operate;
            });

            serviceCollection.AddSingleton<IDictionary<string, IParser>>(Services =>
            {
                var comparer = StringComparer.OrdinalIgnoreCase;
                var parses = new Dictionary<string, IParser>(comparer);
                parses.Add(Constant.NUMBER_TYPE, new NumberParser());
                parses.Add(Constant.TEXT_TYPE, new TextParser());
                parses.Add(Constant.GUID_TYPE, new GuidParser());
                return parses;
            });

            serviceCollection.AddScoped<ICompiler, Compiler>();
        }
    }
}