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
				return new ChangeNameResult(UpdateStatus.InvalidName);

            var badger = await _findBadgerQuery.FindById(badgerId);
            if (badger == null)
				return new ChangeNameResult(UpdateStatus.BadgerNotFound);

            if (badger.Name == newName)
				return new ChangeNameResult(UpdateStatus.Ok, badger);

            await _updateNameCommand.Update(badger.Id, newName);

            return new ChangeNameResult(UpdateStatus.Ok, new Badger(badgerId, newName));
        }

        public enum UpdateStatus
        {
            Ok,
            BadgerNotFound,
            InvalidName,
        }

        public class ChangeNameResult
        {
            public ChangeNameResult(UpdateStatus updateStatus, Badger badger = null)
            {
                if (updateStatus == UpdateStatus.Ok && badger == null)
                    throw new ArgumentException("UpdateStatus can't be OK if badger is null", nameof(badger));
                if (badger != null && updateStatus != UpdateStatus.Ok)
                    throw new ArgumentException("UpdateStatus must be OK when badger is not null", nameof(updateStatus));

                Status = updateStatus;
                UpdatedBadger = badger;
            }

            public Badger UpdatedBadger { get; }

            public UpdateStatus Status { get;  }
        }
    }
}
