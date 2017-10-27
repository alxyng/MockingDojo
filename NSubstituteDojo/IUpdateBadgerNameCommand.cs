using System;
using System.Threading.Tasks;

namespace NSubstituteDojo
{
    public interface IUpdateBadgerNameCommand
    {
        Task Update(Guid badgerId, string newName);
    }
}