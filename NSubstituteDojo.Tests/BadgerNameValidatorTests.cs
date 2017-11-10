using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
	public class BadgerNameValidatorTests
	{
		[TestCase(null, Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("", Result = BadgerNameValidatorResult.InvalidName)]
		public BadgerNameValidatorResult ABadgersNameCannotBeNullOrEmpty(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}
		
		[TestCase("Adam", Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("Boris", Result = BadgerNameValidatorResult.Success)]
		public BadgerNameValidatorResult ABadgersNameMustBeginWithTheLetterB(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}

		[TestCase("Big Sean", Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("BRONSON", Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("Bronson", Result = BadgerNameValidatorResult.Success)]
		public BadgerNameValidatorResult AllButTheFirstLetterOfABadgersNameMustBeLowercaseLetters(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}

		[TestCase("مرحبا", Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("Bπ", Result = BadgerNameValidatorResult.InvalidName)]
		[TestCase("Billy", Result = BadgerNameValidatorResult.Success)]
		public BadgerNameValidatorResult ABadgersNameMustContainOnlyLettersOfTheLatinAlphabet(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}

		[TestCase("Bbcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwx", Result = BadgerNameValidatorResult.Success)]
		[TestCase("Bbcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxy", Result = BadgerNameValidatorResult.NameTooLong)]
		public BadgerNameValidatorResult NameMustNotExceedFiftyCharacters(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}
	}
}
