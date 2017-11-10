using System;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
    public class Task2
    {
        private Badger _badger;

        [SetUp]
        public void Setup()
        {
            _badger = new Badger(Guid.NewGuid(), "Boris");
        }

        [Test] // STUBS
        public async void UpdatingABadgerWithTheSameNameUsingStubs()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery
				.FindById(_badger.Id)
				.Returns(_badger);

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Boris");

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test] // MOCKS
        public async void UpdatingABadgerWithTheSameNameUsingMocks()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery
				.FindById(Arg.Any<Guid>())
				.Returns(_badger);

	        var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Boris");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(0).Update(Arg.Any<Badger>());

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test] // STUBS
        public async void UpdatingABadgerWithAValidNameUsingStubs()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
	        findBadgerByIdQuery
		        .FindById(Arg.Any<Guid>())
		        .Returns(_badger);

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));

        }

        [Test] // MOCKS
        public async void UpdatingABadgerWithAValidNameUsingMocks()
        {
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
	        findBadgerByIdQuery
		        .FindById(Arg.Any<Guid>())
		        .Returns(_badger);

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(1).Update(Arg.Is<Badger>(b => b.Id == _badger.Id && b.Name == "Brock"));

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
		}

	    [Test] // STUBS
	    public void QueryDatabaseErrorUsingStubs()
	    {
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
		    findBadgerByIdQuery
			    .FindById(Arg.Any<Guid>())
				.Throws(new Exception("Badgers broke the database"));

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

		    Assert.That(async () => await service.ChangeName(_badger.Id, "Brock"),
			    Throws.Exception.With.Message.EqualTo("Badgers broke the database"));
		}

		[Test] // MOCKS
	    public void QueryDatabaseErrorUsingMocks()
	    {
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
		    findBadgerByIdQuery
			    .FindById(Arg.Any<Guid>())
			    .Throws(new Exception("Badgers broke the database"));

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

		    Assert.That(async () => await service.ChangeName(_badger.Id, "Brock"),
			    Throws.Exception.With.Message.EqualTo("Badgers broke the database"));

		    findBadgerByIdQuery.Received(1).FindById(_badger.Id);
		    updateBadgerNameCommand.Received(0).Update(Arg.Any<Badger>());
		}

	    [Test] // STUBS
	    public async void UpdatingTwiceTheSameNameUsingStubs()
	    {
		    var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
		    findBadgerByIdQuery
			    .FindById(Arg.Any<Guid>())
			    .Returns(_badger);

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

		    await service.ChangeName(_badger.Id, "Brock");

		    var result = await service.ChangeName(_badger.Id, "Brock");

		    Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
		    Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
	    }

		[Test] // MOCKS
	    public async void UpdatingTwiceTheSameNameUsingMocks()
	    {
		    var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
		    findBadgerByIdQuery
			    .FindById(Arg.Any<Guid>())
			    .Returns(_badger, new Badger(_badger.Id, "Brock"));

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

		    await service.ChangeName(_badger.Id, "Brock");

			updateBadgerNameCommand.ClearReceivedCalls();

		    var result = await service.ChangeName(_badger.Id, "Brock");

		    await updateBadgerNameCommand.Received(0).Update(Arg.Any<Badger>());

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
		    Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
	    }
	}
}