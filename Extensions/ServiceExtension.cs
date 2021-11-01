using System;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Infrastructure.Service.Common;
using Infrastructure.Service.Operate;
using Infrastructure.Service.Validation;
using Infrastructure.Service.TypeParser;
using Infrastructure.Service.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                operate.Add(Constant.IN_OPERATE, new InOperate());
                operate.Add(Constant.NOT_IN_OPERATE, new NotInOperate());
                operate.Add(Constant.EQUAL_OPERATE, new EqualOperate());
                operate.Add(Constant.NOT_EQUAL_OPERATE, new NotEqualOperate());
                operate.Add(Constant.LESS_THAN_OPERATE, new LessThanOperate());
                operate.Add(Constant.LESS_THAN_EQUAL_OPERATE, new LessThanEqualOperate());
                operate.Add(Constant.GREATER_THAN_OPERATE, new GreaterThanOperate());
                operate.Add(Constant.GREATER_THAN_EQUAL_OPERATE, new GreaterThanEqualOperate());
                return operate;
            });

            serviceCollection.AddScoped<IParser, BaseParser>();

            serviceCollection.AddSingleton<DynamicConfig>(services =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var dynamic = new DynamicConfig();
                configuration.GetSection("DynamicSearch").Bind(dynamic);
                return dynamic;
            });

            serviceCollection.AddScoped<ICompiler, Compiler>();
            serviceCollection.AddScoped<IValidation, SearchValidation>();
        }
    }
}