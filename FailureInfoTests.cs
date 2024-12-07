using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sisusa.Common.ReturnTypes;
using System;

namespace Sisusa.Common.Tests.ReturnTypes
{
    [TestClass]
    public class FailureInfoTests
    {
        [TestMethod]
        public void Constructor_WithMessageOnly_ShouldInitializeProperties()
        {
            // Arrange
            string message = "Test Message";

            var fallbackExc = new Exception("Fallback exception");

            // Act
            var failureInfo = new FailureInfo(message);

            // Assert
            Assert.AreEqual(message, failureInfo.Message);
            
            Assert.AreEqual(message, failureInfo.InnerException.OrElse(fallbackExc)?.Message);
        }

        [TestMethod]
        public void Constructor_WithMessageAndException_ShouldInitializeProperties()
        {
            // Arrange
            string message = "Test Message";
            var exception = new Exception("Inner Exception");

            // Act
            var failureInfo = new FailureInfo(message, exception);

            // Assert
            Assert.AreEqual(message, failureInfo.Message);
            
            failureInfo.InnerException.Match(
                some: ex=> Assert.AreEqual(exception.Message, ex?.Message),
                none: () => Assert.Fail("No inner exception"));
           
        }
        

        [TestMethod]
        public void Constructor_WithMessageException_ShouldInitializeProperties()
        {
            // Arrange
            string message = "Test Message";
            var exception = new Exception("Inner Exception");
            
            // Act
            var failureInfo = new FailureInfo(message, exception);

            // Assert
            Assert.AreEqual(message, failureInfo.Message);
            
            failureInfo.InnerException.Match(
                some: ex=> Assert.AreEqual(exception, ex),
                none: () =>Assert.Fail("Exception and wrapped exception not the same.")
                );
        }

        [TestMethod]
        public void FromException_ShouldCreateFailureInfoWithExceptionAndMessage()
        {
            // Arrange
            var exception = new Exception("Inner Exception");
            string message = "Test Message";

            // Act
            var failureInfo = FailureInfo.FromException(exception, message);

            // Assert
            Assert.AreEqual(message, failureInfo.Message);
            failureInfo.InnerException.Match(
                some: ex=> Assert.AreEqual(exception.Message, ex?.Message), 
                none: () => Assert.Fail("No inner exception")
                );
        }

        [TestMethod]
        public void WithMessage_ShouldCreateFailureInfoWithMessage()
        {
            // Arrange
            string message = "Test Message";

            // Act
            var failureInfo = FailureInfo.WithMessage(message);

            // Assert
            Assert.AreEqual(message, failureInfo.Message);
            failureInfo.InnerException.Match(
                some: ex=>Assert.AreEqual(message, ex?.Message), 
                none: () => Assert.Fail("No inner exception")
                );
        }

        [TestMethod]
        public void WithException_ShouldUpdateInnerException()
        {
            // Arrange
            string message = "Test Message";
            var originalException = new Exception("Original Exception");
            var newException = new Exception("New Exception");
            var failureInfo = new FailureInfo(message, originalException);

            // Act
            var updatedFailureInfo = failureInfo.WithException(newException);

            // Assert
            Assert.AreSame(failureInfo, updatedFailureInfo);
            Assert.AreEqual(newException, failureInfo.InnerException.OrElse(originalException));
        }
        

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithNullMessage_ShouldThrowArgumentNullException()
        {
            // Act
            var failureInfo = new FailureInfo(null!);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithException_NullException_ShouldNotAllowNullAssignment()
        {
            // Arrange
            string message = "Test Message";
            var failureInfo = new FailureInfo(message);

            // Act
            
            var updatedFailureInfo = failureInfo.WithException(null!);

            // Assert
            Assert.AreSame(failureInfo, updatedFailureInfo);
            
        }
    }
}
