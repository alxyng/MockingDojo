using System.Threading.Tasks;

namespace NSubstituteDojo
{
    public interface IUpdateBadgerNameCommand
    {
        Task Update(Badger badger);
    }
}