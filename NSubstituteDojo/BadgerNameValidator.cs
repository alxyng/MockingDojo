using System.Text.RegularExpressions;

namespace NSubstituteDojo
{
	public class BadgerNameValidator
	{
		public bool IsValid(string name)
		{
			if (name == null)
				return false;

			if (!name.StartsWith("B"))
				return false;

			if (!Regex.IsMatch(name.Substring(1), @"^[a-z]+$"))
				return false;

			return true;
		}
	}
}