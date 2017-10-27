using System;

namespace NSubstituteDojo
{
	public class ChangeNameResult
	{
		public ChangeNameResult(ChangeNameStatus changeNameStatus, Badger badger = null)
		{
			if (changeNameStatus == ChangeNameStatus.Ok && badger == null)
				throw new ArgumentException("ChangeNameStatus can't be OK if badger is null", nameof(badger));
			if (badger != null && changeNameStatus != ChangeNameStatus.Ok)
				throw new ArgumentException("ChangeNameStatus must be OK when badger is not null", nameof(changeNameStatus));

			Status = changeNameStatus;
			UpdatedBadger = badger;
		}

		public Badger UpdatedBadger { get; }

		public ChangeNameStatus Status { get;  }
	}
}