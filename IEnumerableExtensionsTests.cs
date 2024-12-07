using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sisusa.Common.ReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable CS8604

namespace Sisusa.Common.ReturnTypes.Tests
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod]
        public void ToOptional_ShouldReturnEmpty_WhenValueIsNull()
        {
            // Arrange
            string? value = null;

            // Act
            var result = value.ToOptional();

            result.Match(
                some: val=> Assert.Fail($"Should not have a value but has {val}"),
                none: ()=> Assert.IsTrue(true)
                );
        }

        [TestMethod]
        public void ToOptional_ShouldReturnEmpty_WhenValueIsDefault()
        {
            // Arrange
            int value = default;

            // Act
            var result = value.ToOptional();
            result.Match(
                some: val => Assert.Fail("Has valid value when empty expected."),
                none: () => Assert.IsTrue(true));
        }

        [TestMethod]
        public void ToOptional_ShouldReturnSome_WhenValueIsNotNullOrDefault()
        {
            // Arrange
            int value = 42;

            // Act
            var result = value.ToOptional();

            // Assert
            Assert.AreEqual(42, result.OrElse(50));
        }

        [TestMethod]
        public void FirstOrNone_ShouldReturnEmpty_WhenSequenceIsNull()
        {
            // Arrange
            IEnumerable<int>? sequence = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sequence.FirstOrNone());
        }

        [TestMethod]
        public void FirstOrNone_ShouldReturnEmpty_WhenSequenceIsEmpty()
        {
            // Arrange
            var sequence = Enumerable.Empty<int>();

            // Act
            var result = sequence.FirstOrNone();

            // Assert
            Assert.AreEqual(0, result.OrElse(0));
        }

        [TestMethod]
        public void FirstOrNone_ShouldReturnSome_WhenSequenceHasElements()
        {
            // Arrange
            var sequence = new List<int> { 42, 99 };

            // Act
            var result = sequence.FirstOrNone();

            // Assert
            Assert.AreEqual(42, result.OrElse(50));
        }

        [TestMethod]
        public void FirstOrNone_ShouldReturnEmpty_WhenFirstElementIsDefault()
        {
            // Arrange
            var sequence = new List<int> { default, 42 };

            // Act
            var result = sequence.FirstOrNone();

            // Assert
            Assert.AreEqual(0, result.OrElse(0));
        }

        [TestMethod]
        public void FailureOrSingle_ShouldReturnSuccess_WhenSequenceHasSingleElement()
        {
            // Arrange
            var sequence = new List<int> { 42 };

            // Act
            var result = sequence.FailureOrSingle();

            // Assert
            Assert.AreEqual(42, result
                                   .MatchReturn(success: val=> val, failure: err=> -1)
                                   );
        }

        [TestMethod]
        public void FailureOrSingle_ShouldReturnFailure_WhenSequenceHasMultipleElements()
        {
            // Arrange
            var sequence = new List<int> { 42, 99 };

            // Act
            var result = sequence.FailureOrSingle();

            // Assert
            Assert.AreNotEqual("Success", result.MatchReturn(
                success: val => "Success", 
                failure: err => err.Message)
                );
        }

        [TestMethod]
        public void FailureOrSingle_ShouldThrowException_WhenSequenceIsNull()
        {
            // Arrange
            IEnumerable<int>? sequence = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sequence.FailureOrSingle());
        }

        [TestMethod]
        public void FailureOrSingle_ShouldReturnFailure_WhenSequenceIsEmpty()
        {
            // Arrange
            var sequence = Enumerable.Empty<int>();

            // Act
            var result = sequence.FailureOrSingle();

            // Assert
            var msg = result.MatchReturn(success=> "Succeeded", err=> err.Message);
            Assert.AreNotEqual("Succeeded", msg);
        }

        [TestMethod]
        public void IsEmpty_ShouldReturnTrue_WhenSequenceIsEmpty()
        {
            // Arrange
            var sequence = Enumerable.Empty<int>();

            // Act
            var result = sequence.IsEmpty();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsEmpty_ShouldReturnFalse_WhenSequenceHasElements()
        {
            // Arrange
            var sequence = new List<int> { 42 };

            // Act
            var result = sequence.IsEmpty();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsEmpty_ShouldThrowException_WhenSequenceIsNull()
        {
            // Arrange
            IEnumerable<int>? sequence = null;

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => sequence.IsEmpty());
        }
    }
}

#pragma warning restore CS8604
