using System;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
    public class ChangeBadgerNameServiceTests
    {
        private Badger _badger;

        [SetUp]
        public void Setup()
        {
            _badger = new Badger(Guid.NewGuid(), "Boris");
        }

        [Test]
        public async void UpdatingABadgerWithAnInvalidNameReturnsStatusInvalidName()
        {
	        var validator = Substitute.For<IBadgerNameValidator>();
	        validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.InvalidName);

            var service = new ChangeBadgerNameService(null, null, validator);

            var result = await service.ChangeName(_badger.Id, "Ash");

            Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.InvalidName));
        }

	    [Test]
	    public async void UpdatingABadgerWithANameThatIsTooLongReturnsStatusNameTooLong()
	    {
		    var validator = Substitute.For<IBadgerNameValidator>();
		    validator
			    .IsValid(Arg.Any<string>())
			    .Returns(BadgerNameValidatorResult.NameTooLong);

		    var service = new ChangeBadgerNameService(null, null, validator);

		    var result = await service.ChangeName(_badger.Id, "Ash");

		    Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.NameTooLong));
	    }

	    [Test]
	    public async void UpdatingABadgerThatDoesNotExistReturnsStatusBadgerNotFound()
	    {
		    var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
		    findBadgerByIdQuery
			    .FindById(Arg.Any<Guid>())
			    .Returns(Task.FromResult<Badger>(null));

			var validator = Substitute.For<IBadgerNameValidator>();
		    validator
			    .IsValid(Arg.Any<string>())
			    .Returns(BadgerNameValidatorResult.Success);

		    var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>(), validator);

			var result = await service.ChangeName(_badger.Id, "Brock");

		    Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.BadgerNotFound));
	    }

		[Test]
		public async void UpdatingABadgerWithTheSameNameDoesNotCallUpdate()
		{
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
			findBadgerByIdQuery
				.FindById(_badger.Id)
				.Returns(_badger);

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

			var result = await service.ChangeName(_badger.Id, "Boris");

			await updateBadgerNameCommand.Received(0).Update(Arg.Any<Badger>());

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
			Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
		}

		[Test]
		public async void UpdatingABadgerWithAValidNameCallsUpdate()
		{
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
			findBadgerByIdQuery
				.FindById(Arg.Any<Guid>())
				.Returns(_badger);

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

			var result = await service.ChangeName(_badger.Id, "Brock");

			await updateBadgerNameCommand.Received(1).Update(Arg.Is<Badger>(b => b.Id == _badger.Id && b.Name == "Brock"));

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
			Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));

		}

		[Test]
		public void QueryDatabaseError()
		{
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
			findBadgerByIdQuery
				.FindById(Arg.Any<Guid>())
				.Throws(new Exception("Badgers broke the database"));

			var updateBadgerNameCommand = Substitute.For<IUpdateBadgerNameCommand>();

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

			Assert.That(async () => await service.ChangeName(_badger.Id, "Brock"),
				Throws.Exception.With.Message.EqualTo("Badgers broke the database"));
		}
	}
}