using System.Text.RegularExpressions;

namespace NSubstituteDojo
{
	public enum BadgerNameValidatorResult
	{
		Success,
		InvalidName,
		NameTooLong
	}

	public class BadgerNameValidator
	{
		public BadgerNameValidatorResult IsValid(string name)
		{
			if (name == null)
				return BadgerNameValidatorResult.InvalidName;

			if (!name.StartsWith("B"))
				return BadgerNameValidatorResult.InvalidName;

			if (!ContainsOnlyLowercaseLatinAlphabetLetters(name.Substring(1)))
				return BadgerNameValidatorResult.InvalidName;

			if (name.Length > 50)
				return BadgerNameValidatorResult.NameTooLong;

			return BadgerNameValidatorResult.Success;
		}

		private static bool ContainsOnlyLowercaseLatinAlphabetLetters(string str)
		{
			return Regex.IsMatch(str, @"^[a-z]+$");
		}
	}
}