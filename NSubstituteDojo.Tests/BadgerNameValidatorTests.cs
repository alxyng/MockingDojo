using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
	public class BadgerNameValidatorTests
	{
		[TestCase(null, Result = false)]
		[TestCase("", Result = false)]
		public bool ABadgersNameCannotBeNullOrEmpty(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}
		
		[TestCase("Adam", Result = false)]
		[TestCase("Boris", Result = true)]
		public bool ABadgersNameMustBeginWithTheLetterB(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}

		[TestCase("Big Sean", Result = false)]
		[TestCase("BRONSON", Result = false)]
		[TestCase("Bronson", Result = true)]
		public bool AllButTheFirstLetterOfABadgersNameMustBeLowercaseLetters(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}

		[TestCase("مرحبا", Result = false)]
		[TestCase("Bπ", Result = false)]
		[TestCase("Billy", Result = true)]
		public bool ABadgersNameMustContainOnlyLettersOfTheLatinAlphabet(string name)
		{
			var validator = new BadgerNameValidator();
			return validator.IsValid(name);
		}
	}
}
