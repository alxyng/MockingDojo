namespace NSubstituteDojo
{
	public interface IBadgerNameValidator
	{
		BadgerNameValidatorResult IsValid(string name);
	}
}