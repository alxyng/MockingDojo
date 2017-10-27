using System;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
    public class ChangeBadgerNameServiceUsingStubsTests
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
        }

        [Test]
        public async void UpdatingTwiceTheSameNameTests()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = null;
            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            await service.ChangeName(_badger.Id, "Brock");

            var result = await service.ChangeName(_badger.Id, "Brock");

            Assert.That(result, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
        }
    }
}

    
