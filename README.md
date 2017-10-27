# Mocking Dojo

http://codemanship.co.uk/parlezuml/blog/?postid=1313

https://martinfowler.com/articles/mocksArentStubs.html

https://martinfowler.com/articles/mocksArentStubs.html#TheDifferenceBetweenMocksAndStubs

https://blog.pragmatists.com/test-doubles-fakes-mocks-and-stubs-1a7491dfa3da

## Intro

In this session we are going to write some tests around a service that allows Badgers to change their name. 

1. Open the NSubstitueDojo solution in visual studio 2015
2. Take a look at the ChangeBadgerNameService
3. Run the ChangeBadgerNameServiceTests (they should all fail)

[NSubstitute Docs](http://nsubstitute.github.io/help.html)

## Task 1.

- Open Task1.cs in the Tests project
- Replace the "null" values with an appropriate call to Substitute.For in all tests. By default NSubstitute will return null for the call to FindById on the query object which is what we want for this test.
- Run the tests
- Note that the "UsingMocks" variant is verifying the iteractions with the query object and the "UsingStubs" variant is just inspecting the result. Is there 

## Task 2.





Task 2.

Change the signature of IUpdateBadgerNameCommand to take a Badger object
