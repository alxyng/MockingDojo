using System;
using NSubstitute;
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

        [Test]
        public async void UpdatingABadgerWithTheSameNameUsingStubs()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery.FindById(_badger.Id).Returns(_badger);

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Boris");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test]
        public async void UpdatingABadgerWithTheSameNameUsingMocks()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery.FindById(Arg.Any<Guid>()).Returns(_badger);

            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Boris");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(0).Update(Arg.Any<Guid>(), "Boris");

            Assert.That(result, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test]
        public async void UpdatingABadgerWithAValidNameUsingStubs()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = null;
            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            Assert.That(result, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));

        }

        [Test]
        public async void UpdatingABadgerWithAValidNameUsingMocks()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = null;
            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(1).Update(_badger.Id, "Brock");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
        }
    }

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