using Infrastructure.Service.Model;
using Infrastructure.Service.Validation;
using Infrastructure.Service.Abstraction;
using Microsoft.Extensions.Configuration;
using Infrastructure.Service.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service.Extension
{
    public static class ServiceExtension
    {
        public static void AddSearchService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SearchRestrictionDictionary>(services =>
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var restrict = new SearchRestrictionDictionary();
                configuration.GetSection("SearchRestrictions").Bind(restrict);
                return restrict;
            });

            serviceCollection.AddScoped<ISortConverter, SortConverter>();
            serviceCollection.AddScoped<IValidation, SearchValidation>();
            serviceCollection.AddScoped<IFilterConverter, FilterConverter>();
        }
    }
}