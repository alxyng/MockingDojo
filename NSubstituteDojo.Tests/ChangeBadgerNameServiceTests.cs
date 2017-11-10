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

		[Test] // STUBS
		public async void UpdatingABadgerWithTheSameNameUsingStubs()
		{
			var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
			findBadgerByIdQuery
				.FindById(_badger.Id)
				.Returns(_badger);

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>(), validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

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

			var validator = Substitute.For<IBadgerNameValidator>();
			validator
				.IsValid(Arg.Any<string>())
				.Returns(BadgerNameValidatorResult.Success);

			var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand, validator);

			await service.ChangeName(_badger.Id, "Brock");

			updateBadgerNameCommand.ClearReceivedCalls();

			var result = await service.ChangeName(_badger.Id, "Brock");

			await updateBadgerNameCommand.Received(0).Update(Arg.Any<Badger>());

			Assert.That(result.Status, Is.EqualTo(ChangeNameStatus.Ok));
			Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
		}
	}
}