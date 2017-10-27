using System;
using System.Threading.Tasks;

namespace NSubstituteDojo
{
    public class ChangeBadgerNameService
    {
        private readonly IFindBadgerByIdQuery _findBadgerQuery;
        private readonly IUpdateBadgerNameCommand _updateNameCommand;

        public ChangeBadgerNameService(IFindBadgerByIdQuery findBadgerQuery, IUpdateBadgerNameCommand updateNameCommand)
        {
            _findBadgerQuery = findBadgerQuery;
            _updateNameCommand = updateNameCommand;
        }

        public async Task<ChangeNameResult> ChangeName(Guid badgerId, string newName)
        {
            if (!newName.StartsWith("B"))
				return new ChangeNameResult(ChangeNameStatus.InvalidName);

            var badger = await _findBadgerQuery.FindById(badgerId);
            if (badger == null)
				return new ChangeNameResult(ChangeNameStatus.BadgerNotFound);

            if (badger.Name == newName)
				return new ChangeNameResult(ChangeNameStatus.Ok, badger);

            await _updateNameCommand.Update(badger.Id, newName);

            return new ChangeNameResult(ChangeNameStatus.Ok, new Badger(badgerId, newName));
        }
    }
}
