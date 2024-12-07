using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sisusa.Common.ReturnTypes;

namespace Sisusa.Common.ReturnTypes.Tests
{
    [TestClass]
    public class FailureFactoryTests
    {
        [TestMethod]
        public void WithMessage_ValidMessage_ReturnsFailureInfo()
        {
            var message = "Test failure message";

            var result = FailureFactory.WithMessage(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(message, result.Message);
            Assert.IsInstanceOfType(result, typeof(FailureInfo));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WithMessage_NullOrWhiteSpace_ThrowsException()
        {
            FailureFactory.WithMessage("");
        }

        [TestMethod]
        public void WithCodeAndMessage_ValidInputs_ReturnsFailure()
        {
            string code = "ERR001";
            string message = "Detailed failure message";

            var result = FailureFactory.WithCodeAndMessage(code, message);

            Assert.IsNotNull(result);
            Assert.AreEqual($"{code}: {message}", result.Message);
            Assert.IsInstanceOfType(result, typeof(Failure));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithCodeAndMessage_NullCode_ThrowsException()
        {
            FailureFactory.WithCodeAndMessage(null!, "Valid message");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithCodeAndMessage_NullMessage_ThrowsException()
        {
            FailureFactory.WithCodeAndMessage("ERR001", null!);
        }

        [TestMethod]
        public void WithCodeMessageAndException_ValidInputs_ReturnsFailure()
        {
            string code = "ERR002";
            string description = "Extended failure description";
            var exception = new InvalidOperationException("Invalid operation");

            var result = FailureFactory.WithCodeMessageAndException(code, description, exception);

            Assert.IsNotNull(result);
            Assert.AreEqual($"{code}: {description}", result.Message);
            Assert.AreEqual(exception, result.InnerException.OrElse(null));
            Assert.IsInstanceOfType(result, typeof(Failure));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WithCodeMessageAndException_NullCode_ThrowsException()
        {
            FailureFactory.WithCodeMessageAndException(null!, "Valid description", new Exception());
        }

        [TestMethod]
        public void FromException_ValidException_ReturnsFailureInfo()
        {
            var exception = new Exception("Exception message");

            var result = FailureFactory.FromException(exception);

            Assert.IsNotNull(result);
            Assert.AreEqual(exception.Message, result.Message);
            Assert.AreEqual(exception, result.InnerException.OrElse(null));
            Assert.IsInstanceOfType(result, typeof(FailureInfo));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FromException_NullException_ThrowsException()
        {
            FailureFactory.FromException(null!);
        }
    }
}
