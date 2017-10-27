using System;
using System.Threading.Tasks;

namespace NSubstituteDojo
{
    public interface IFindBadgerByIdQuery
    {
        Task<Badger> FindById(Guid id);
    }
}