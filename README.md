# Mocking Dojo

## Intro

In this session we are going to write some tests around a service that allows Badgers to change their name. We'll be focusing on how we can use NSubstitute to write tests and consider the differences between Mocking and Stubbing and the scenarios where we might choose to use one method over the other.

Before beginning the Dojo, it would be useful to have a read of Martin Fowler's blog post [Mocks Aren't Stubs](https://martinfowler.com/articles/mocksArentStubs.html#TheDifferenceBetweenMocksAndStubs). The blog post is quite long so for this session just focus on the _'The Difference Between Mocks and Stubs'_ section. The whole post is still worth a read in your own time however.

1. Open the `NSubstitueDojo` solution in Visual Studio 2015
2. Take a look at the `ChangeBadgerNameService`
3. Run the `ChangeBadgerNameServiceTests` (most of them should fail)

[NSubstitute Docs](http://nsubstitute.github.io/help.html)

## Aims

By the end of the session we hope you'll...

- Feel comfortable with the commonly used features of NSubstitute
- Understand the difference between Mocks and Stubs
- Be able to identify when / when not to use a Mock or Stub
- Have practiced writing basic unit tests using Mocking and Stubbing
- Be keen to write some more tests in your day job!

## Task 1

- Open `Task1.cs` in the Tests project.
- Replace the `null` values with an appropriate call to `Substitute.For` in all tests. By default NSubstitute will return `null` for the call to `FindById` on the query object which is what we want for this test.
- Run the tests.
- Note that the _'UsingMocks'_ variant is verifying the iteractions with the query object and the _'UsingStubs'_ variant is just inspecting the result. 
- Is there a downside to either of these two methods in this scenario? Which method do you think is most appropriate here?

## Task 2

- Open `Task2.cs` in the Tests project.
- Replace the `null` values with appropriate substituted values and perform the necessary setups to make the tests pass.
- Run the tests.
- Is there a downside to either of these two methods in this scenario? Which method do you think is most appropriate here?

## Task 3

- Now change the signature of the `Update` method on `IUpdateBadgerNameCommand` to take a `Badger` object rather than a `Guid` and a `string`.
- Make all your tests compile and pass again.
- Which method of testing was more brittle and required most changes after this modification?

## Task 4

Your Badger product owner wants some additional validation when Badger's change their name. Based on the acceptance criteria provided, test drive some changes to the `ChangeBadgerNameService`.
_Hint: You may find it useful to extract the validation logic into a separate validation service._

- Make the changes to the `ChangeBadgerNameService`.
- The tests should pass between each acceptance criteria.
- What affect did your changes have on the tests for `ChangeBadgerNameService`?
- Did you find any of the tests unhelpful when making these changes? Why?

**Acceptance Criteria**
- A badger's name must still begin with a capital letter.
- A badger's name must still begin with the letter 'B'.
- All but the first letter in a badger's name must be lower case.
- A badger's name must only contain letters of the latin alphabet.
- All above criteria should return an appropriate _'Invalid Name'_ response.
- The badger's name must not exceed 50 characters.
- The name length criteria should return an appropriate _'Name too long'_ response.
