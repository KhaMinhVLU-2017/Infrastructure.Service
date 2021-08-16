using System.Threading.Tasks;
using Infrastructure.Service.Model;
using Infrastructure.Repository.Model.Generic;

namespace Infrastructure.Service.Abstraction
{
    public interface ISearchService<TCriteria, TModel, TEntity, TKey> where TEntity : IEntity<TKey>
                                                                      where TCriteria : BaseCriteria
    {
        Task<PagedList<TModel>> FindByCriteriaAsync(TCriteria criteria);
    }
}