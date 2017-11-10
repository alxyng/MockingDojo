using System;
using System.Threading.Tasks;

namespace NSubstituteDojo
{
    public class ChangeBadgerNameService
    {
        private readonly IFindBadgerByIdQuery _findBadgerQuery;
        private readonly IUpdateBadgerNameCommand _updateNameCommand;
	    private readonly IBadgerNameValidator _badgerNameValidator;

	    public ChangeBadgerNameService(IFindBadgerByIdQuery findBadgerQuery, IUpdateBadgerNameCommand updateNameCommand, IBadgerNameValidator badgerNameValidator)
        {
            _findBadgerQuery = findBadgerQuery;
            _updateNameCommand = updateNameCommand;
	        _badgerNameValidator = badgerNameValidator;
        }

        public async Task<ChangeNameResult> ChangeName(Guid badgerId, string newName)
        {
	        switch (_badgerNameValidator.IsValid(newName))
	        {
		        case BadgerNameValidatorResult.Success:
			        break;
		        case BadgerNameValidatorResult.InvalidName:
					return new ChangeNameResult(ChangeNameStatus.InvalidName);
				case BadgerNameValidatorResult.NameTooLong:
					return new ChangeNameResult(ChangeNameStatus.NameTooLong);
				default:
			        throw new ArgumentOutOfRangeException();
	        }

			var badger = await _findBadgerQuery.FindById(badgerId);
            if (badger == null)
				return new ChangeNameResult(ChangeNameStatus.BadgerNotFound);

            if (badger.Name == newName)
				return new ChangeNameResult(ChangeNameStatus.Ok, badger);

            await _updateNameCommand.Update(new Badger(badger.Id, newName));

            return new ChangeNameResult(ChangeNameStatus.Ok, new Badger(badgerId, newName));
        }
    }
}
