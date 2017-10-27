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

* Feel comfortable with the commonly used features of NSubstitute
* Understand the difference between Mocks and Stubs
* Be able to identify when / when not to use a Mock or Stub
* Have practiced writing basic unit tests using Mocking and Stubbing
* Be keen to write some more tests in your day job!

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
