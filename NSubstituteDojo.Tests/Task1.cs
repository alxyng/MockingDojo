using System;
using NSubstitute;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
    public class Task1
    {
        private Badger _badger;

        [SetUp]
        public void Setup()
        {
            _badger = new Badger(Guid.NewGuid(), "Boris");
        }

        [Test] // STUBS
        public async void UpdatingABadgerWithAnInvalidNameUsingStubs()
        {
            var service = new ChangeBadgerNameService(Substitute.For<IFindBadgerByIdQuery>(), Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Ash");

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.InvalidName));
        }

        [Test] // MOCKS
        public async void UpdatingABadgerWithAnInvalidNameUsingMocks()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Ash");

            await findBadgerByIdQuery.Received(0).FindById(Arg.Any<Guid>());

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.InvalidName));

        }

	    [Test] // STUBS
	    public async void UpdatingABadgerThatDoesNotExistUsingStubs()
	    {
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

		    var result = await service.ChangeName(_badger.Id, "Brock");

		    Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.BadgerNotFound));
	    }

	    [Test] // MOCKS
        public async void UpdatingABadgerThatDoesNotExistUsingMocks()
        {
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Brock");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.BadgerNotFound));
        }
    }
}