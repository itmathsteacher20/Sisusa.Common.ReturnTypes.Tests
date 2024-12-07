using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sisusa.Common.ReturnTypes;

namespace Sisusa.Common.Tests.ReturnTypes
{
    [TestClass]
    public class FailureOrTests
    {
        private record TestValue(string Content);
        //private record TestFailureInfo(string Reason) : FailureInfo(Reason);

        [TestMethod]
        public void Success_CreatesSuccessInstance()
        {
            // Arrange
            var value = new TestValue("Test");

            // Act
            var result = FailureOr<TestValue>.Succeed(value);

            // Assert
            Assert.IsNotNull(result);
            
            Assert.AreEqual(value, result.GetOr(new TestValue("Fallback")));
        }

        [TestMethod]
        public void Failure_CreatesFailureInstance_WithMessage()
        {
            // Arrange
            var message = "Test failure";

            // Act
            var result = FailureOr<TestValue>.Fail(message);

            // Assert
            Assert.IsNotNull(result);
           
            Assert.AreEqual(new TestValue("Fallback"), result.GetOr(new TestValue("Fallback")));
        }

        [TestMethod]
        public void Failure_CreatesFailureInstance_WithFailureInfo()
        {
            // Arrange
            var failureInfo = new FailureInfo("Test failure");

            // Act
            var result = FailureOr<TestValue>.Fail(failureInfo);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.MatchReturn(
                success: value=> false,
                failure: fInfo=> true
                ));
            //Assert.IsTrue((bool)result.GetType().GetProperty("IsError")?.GetValue(result));
        }

        [TestMethod]
        public void From_ReturnsSuccessOnActionSuccess()
        {
            // Arrange
            var value = new TestValue("Test");

            // Act
            var result = FailureOr<TestValue>.From(() => value);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(value, result.GetOr(new TestValue("Fallback")));
        }

        [TestMethod]
        public void From_ReturnsFailureOnActionException()
        {
            // Arrange
            Func<TestValue> action = () => throw new InvalidOperationException("Test error");

            // Act
            var result = FailureOr<TestValue>.From(action);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.MatchReturn(value => true, err => false));
            
        }

        [TestMethod]
        public void WrapException_CreatesFailureWithException()
        {
            // Arrange
            var exception = new InvalidOperationException("Test exception");

            // Act
            var result = FailureOr<TestValue>.Fail(exception);

            var fallback = new Exception("Fallback exception");

            // Assert
            Assert.IsNotNull(result);
            result.Match(
                success: val => Assert.Fail("Did not fail, no exception thrown!"),
                failure: err => Assert.
                                   IsInstanceOfType<InvalidOperationException>(
                                       err.InnerException.OrElse(fallback))
                        );

        }

        

        [TestMethod]
        public void GetOr_ReturnsFallbackOnError()
        {
            // Arrange
            var result = FailureOr<TestValue>.Fail("Error");

            // Act
            var value = result.GetOr(new TestValue("Fallback"));

            // Assert
            Assert.AreEqual(new TestValue("Fallback"), value);
        }

        [TestMethod]
        public void Handle_InvokesCorrectAction()
        {
            // Arrange
            var successValue = new TestValue("Success");
            var errorInfo = new FailureInfo("Error");

            var successCalled = false;
            var errorCalled = false;

            var success = FailureOr<TestValue>.Succeed(successValue);
            var error = FailureOr<TestValue>.Fail(errorInfo);

            // Act
            success.Match(
                value => successCalled = value == successValue,
                failure => errorCalled = true
            );

            error.Match(
                value => successCalled = true,
                failure => errorCalled = failure == errorInfo
            );

            // Assert
            Assert.IsTrue(successCalled);
            Assert.IsTrue(errorCalled);
        }

        [TestMethod]
        public void Match_ReturnsCorrectResult()
        {
            // Arrange
            var successValue = new TestValue("Success");
            var errorInfo = new FailureInfo("Error");

            var success = FailureOr<TestValue>.Succeed(successValue);
            var error = FailureOr<TestValue>.Fail(errorInfo);

            // Act
            var successResult = success.MatchReturn(
                success: value => value.Content,
                failure: failure => failure.Message
            );

            var errorResult = error.MatchReturn(
                success: value => value.Content,
                failure: failure => failure.Message
            );

            // Assert
            Assert.AreEqual("Success", successResult);
            Assert.AreEqual("Error", errorResult);
        }

        [TestMethod]
        public void Extensions_Map_TransformsOnSuccess()
        {
            // Arrange
            var value = FailureOr<int>.Succeed(10);

            // Act
            var mapped = value.Map<int>(x => x * 2);

            // Assert
            Assert.AreEqual(20, mapped.GetOr(50));
        }

        [TestMethod]
        public void Extensions_Then_ExecutesNextOnSuccess()
        {
            // Arrange
            var value = FailureOr<int>.Succeed(10);

            // Act
            var next = value.Then(x => FailureOr<int>.Succeed(x + 5));

            // Assert
            Assert.AreEqual(15, next.GetOr(0));
        }

        [TestMethod]
        public void Extensions_Catch_HandlesError()
        {
            // Arrange
            var failure = FailureOr<TestValue>.Fail("Test failure");
            var errorHandled = false;

            // Act
            failure.Catch(error => errorHandled = error.Message == "Test failure");

            // Assert
            Assert.IsTrue(errorHandled);
        }
    }
}
