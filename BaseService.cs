using System;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Infrastructure.Service.Model;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Service.Extension;
using Infrastructure.Service.Abstraction;
using Infrastructure.Repository.Model.Generic;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repository.Implement.Abstraction;

namespace Infrastructure.Service
{
    public abstract class BaseService<TCriteria, TModel, TEntity, TKey> : ISearchService<TCriteria, TModel, TEntity, TKey> where TCriteria : BaseCriteria
                                                                                                                  where TEntity : class, IEntity<TKey>

    {
        private IServiceProvider _serviceProvider;
        private IRepository<TEntity, TKey> _repository;
        private Expression<Func<TEntity, TModel>> _projection { get; set; }

        public BaseService(IRepository<TEntity, TKey> repository, IServiceProvider serviceProvider, Expression<Func<TEntity, TModel>> projection)
        {
            _repository = repository;
            _projection = projection;
            _serviceProvider = serviceProvider;
        }

        public async Task<PagedList<TModel>> FindByCriteriaAsync(TCriteria criteria)
        {
            DateTime start = DateTime.UtcNow;

            Page page = new Page();
            IEnumerable<TModel> data = new List<TModel>();

            // Duration miliseconds
            DateTime startDuration = DateTime.UtcNow;
            var validation = _serviceProvider.GetRequiredService<IValidation>();
            validation.Validate(criteria);
            var dataTask = BuildQueryAsync(criteria);
            var pageTask = BuildDurationAsync(criteria);
            await Task.WhenAll(pageTask, dataTask);

            page = await pageTask;
            data = await dataTask;

            double durationTime = DateTime.UtcNow.Subtract(startDuration).Milliseconds;
            // End duration

            double totalTime = DateTime.UtcNow.Subtract(start).Milliseconds;
            page.TotalTime = totalTime;
            page.Duration = durationTime;
            return new PagedList<TModel>(page, data);
        }

        private async Task<Page> BuildDurationAsync(TCriteria criteria)
        {
            DateTime start = DateTime.UtcNow;
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbType = GetDbType();
                var context = scope.ServiceProvider.GetService(dbType);
                var compiler = scope.ServiceProvider.GetRequiredService<ICompiler>();
                var query = AsQueryable(context);
                query = BuildQuery(query, compiler, criteria);

                var (pageIndex, pageSize) = HandlerPaging(criteria);
                var totalItemCount = await query.CountAsync();
                var queryResult = query.Skip(pageIndex * pageSize).Take(pageSize);
                var itemCount = await queryResult.CountAsync();
                int queryDuration = DateTime.UtcNow.Subtract(start).Milliseconds;

                return Page.Create(criteria.PageIndex, pageSize, totalItemCount, itemCount, queryDuration);
            }
        }

        private async Task<IEnumerable<TModel>> BuildQueryAsync(TCriteria criteria)
        {
            var scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var dbType = GetDbType();
                var context = scope.ServiceProvider.GetService(dbType);
                var compiler = scope.ServiceProvider.GetRequiredService<ICompiler>();
                var query = AsQueryable(context);
                query = BuildQuery(query, compiler, criteria);

                var (pageIndex, pageSize) = HandlerPaging(criteria);
                query = query.Skip(pageIndex * pageSize).Take(pageSize);
                var result = await query.Select(_projection).ToListAsync();
                return result;
            }
        }

        private IQueryable<TEntity> BuildQuery(IQueryable<TEntity> entities, ICompiler compiler, TCriteria criteria)
        {
            compiler.SetEntityType(typeof(TEntity));
            var (dynamicFilter, paramsQuery) = compiler.BuildQueryString(criteria);
            if (!string.IsNullOrEmpty(dynamicFilter))
                entities = entities.Where(dynamicFilter, paramsQuery);

            if (criteria.Sorts != null && !string.IsNullOrEmpty(criteria.Sorts))
            {
                var sortCriteria = compiler.DeserializeModel<Sort>(criteria.Sorts);
                entities = ExpressionHelper.OrderBy<TEntity>(entities, sortCriteria);
            }

            return entities;
        }

        private IQueryable<TEntity> AsQueryable(object context)
        {
            return (context as IRepository<TEntity, TKey>).AsQueryable();
        }

        protected abstract Type GetDbType();

        private (int, int) HandlerPaging(TCriteria criteria)
        {
            var pageIndex = criteria.PageIndex <= 0 ? -1 : criteria.PageIndex - 1;
            var pageSize = criteria.PageSize <= 0 ? 0 : criteria.PageSize;

            if (pageIndex == -1)
                pageSize = 0;
            return (pageIndex, pageSize);
        }
    }
}