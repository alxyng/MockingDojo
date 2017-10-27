using System;
using NSubstitute;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
	public class ChangeBadgerNameServiceUsingMocksTests
	{
		private Badger _badger;

		[SetUp]
		public void Setup()
		{
			_badger = new Badger(Guid.NewGuid(), "Boris");
		}









		[Test]
		public void QueryDatabaseErrorTest()
		{
			IFindBadgerByIdQuery findBadgerByIdQuery = null;
			IUpdateBadgerNameCommand updateBadgerNameCommand = null;

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

			Assert.That(() => service.ChangeName(_badger.Id, "Brock").Wait(),
				Throws.Exception.With.Message.EqualTo("Badgers broke the database"));

			findBadgerByIdQuery.Received(1).FindById(_badger.Id);
			updateBadgerNameCommand.Received(0).Update(Arg.Any<Guid>(), Arg.Any<string>());
		}

		[Test]
		public async void UpdatingTwiceTheSameNameTests()
		{
			IFindBadgerByIdQuery findBadgerByIdQuery = null;
			IUpdateBadgerNameCommand updateBadgerNameCommand = null;

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

			await service.ChangeName(_badger.Id, "Brock");

			findBadgerByIdQuery.ClearReceivedCalls();

			var result = await service.ChangeName(_badger.Id, "Brock");

			await updateBadgerNameCommand.Received(0).Update(Arg.Any<Guid>(), "Boris");

			Assert.That(result, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
			Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
		}
	}
}