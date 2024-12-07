using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sisusa.Common.ReturnTypes;
using System;

namespace Sisusa.Common.Tests.ReturnTypes
{
    [TestClass]
    public class ValidationErrorTests
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Arrange
            var propertyName = "TestProperty";
            var reasonForFail = "TestReason";

            // Act
            var error = new ValidationError(propertyName, reasonForFail);

            // Assert
            Assert.AreEqual(propertyName, error.PropertyName);
            Assert.AreEqual(reasonForFail, error.ReasonForError);
        }

        [TestMethod]
        public void Property_ShouldReturnValidationErrorWithDefaultMessage()
        {
            // Arrange
            var propertyName = "TestProperty";

            // Act
            var error = ValidationError.Property(propertyName);

            // Assert
            Assert.AreEqual(propertyName, error.PropertyName);
            Assert.AreEqual($"A validation error on {propertyName} occurred.", error.ReasonForError);
        }

        [TestMethod]
        public void ShouldBeGreaterThan_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";
            var threshold = 10;

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldBeGreaterThan(threshold);

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} should be greater than {threshold}", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldBeAtLeast_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";
            var threshold = 5;

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldBeAtLeast(threshold);

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} should be at least {threshold}.", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldBeAtMost_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";
            var threshold = 100;

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldBeAtMost(threshold);

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} should be at most {threshold}.", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldBeLessThan_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";
            var threshold = 50;

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldBeLessThan(threshold);

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} should be less than {threshold}", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldNotBeEmpty_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldNotBeEmpty();

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} should not be empty or null", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldNotBeNull_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldNotBeNull();

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} is null and should have a valid value.", result.ReasonForError);
        }

        [TestMethod]
        public void ShouldHaveFutureDate_ShouldReturnExpectedValidationError()
        {
            // Arrange
            var propertyName = "TestProperty";

            var error = new ValidationError(propertyName, "InitialReason");

            // Act
            var result = error.ShouldHaveFutureDate();

            // Assert
            Assert.AreEqual(propertyName, result.PropertyName);
            Assert.AreEqual($"{propertyName} cannot be a date in the past.", result.ReasonForError);
        }
    }

    [TestClass]
    public class ValidationErrorExtensionsTests
    {
        [TestMethod]
        public void ToString_ShouldReturnReasonForError()
        {
            // Arrange
            var error = new ValidationError("TestProperty", "TestReason");

            // Act
            var result = error.ToString();

            // Assert
            Assert.AreEqual("TestReason", result);
        }

        [TestMethod]
        public void Flatten_ShouldConcatenateErrorMessages()
        {
            // Arrange
            var errors = new[]
            {
                new ValidationError("Prop1", "Error1"),
                new ValidationError("Prop2", "Error2"),
                new ValidationError("Prop3", "Error3")
            };

            // Act
            var result = errors.Flatten();

            // Assert
            var expected = "Error1" + Environment.NewLine + "Error2" + Environment.NewLine + "Error3";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AsException_ShouldReturnValidationException()
        {
            // Arrange
            var error = new ValidationError("TestProperty", "TestReason");

            // Act
            var ex = error.AsException();

            // Assert
            Assert.IsInstanceOfType(ex, typeof(ValidationException));
            Assert.AreEqual("TestReason", ex.Message);
        }
    }
}
