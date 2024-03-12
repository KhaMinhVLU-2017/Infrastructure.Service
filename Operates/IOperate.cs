using Infrastructure.Service.Model;

namespace Infrastructure.Service.Operate
{
    public interface IOperate
    {
        CriteriaValue Compile();
    }
}