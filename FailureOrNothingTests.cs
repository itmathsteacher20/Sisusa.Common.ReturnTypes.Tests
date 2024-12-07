using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sisusa.Common.ReturnTypes;

namespace Sisusa.Common.ReturnTypes.Tests;

[TestClass]
public class FailureOrNothingTests
{
    [TestMethod]
    public void Succeed_ShouldReturnNonErrorInstance()
    {
        var result = FailureOrNothing.Succeed();

        result.Match(
            success: () => Assert.IsTrue(true),
            failure: _ => Assert.Fail("Expected success but got failure.")
        );
    }

    [TestMethod]
    public void Fail_WithMessage_ShouldReturnErrorInstance()
    {
        var result = FailureOrNothing.Fail("Test failure");

        result.Match(
            success: () => Assert.Fail("Expected failure but got success."),
            failure: error =>
            {
                Assert.IsNotNull(error);
                Assert.AreEqual("Test failure", error.Message);
            }
        );
    }

    [TestMethod]
    public void Fail_WithException_ShouldReturnErrorInstance()
    {
        var exception = new InvalidOperationException("Test exception");
        var result = FailureOrNothing.Fail(exception);

        result.Match(
            success: () => Assert.Fail("Expected failure but got success."),
            failure: error =>
            {
                error.InnerException.Match(
                    some: ex =>
                    {
                        Assert.AreSame(exception, ex);
                        Assert.AreEqual(exception.Message, ex!.Message);
                    },
                    none: () =>
                    {
                        Assert.Fail("No exception wrapped.");
                    });
            }
        );
    }

    [TestMethod]
    public void Fail_WithExceptionAndMessage_ShouldReturnErrorInstance()
    {
        var failMsg = "Test Failure";
        var exception = new InvalidOperationException("Test exception");
        var result = FailureOrNothing.Fail(exception, failMsg);

        result.Match(
            success: () => Assert.Fail("Expected failure but got success."),
            failure: error =>
            {
                Assert.AreEqual(failMsg, error.Message);
                error.InnerException.Match(
                    some: ex =>
                    {
                        Assert.AreSame(exception, ex);
                    },
                    none: () =>
                    {
                        Assert.Fail("No exception here when IOE was expected.");
                    });
            }
        );
    }

    [TestMethod]
    public void Then_WithSuccess_ShouldExecuteAction()
    {
        var actionExecuted = false;

        var result = FailureOrNothing.Succeed().Then(() =>
        {
            actionExecuted = true;
        });

        Assert.IsTrue(actionExecuted);
        result.Match(
            success: () => Assert.IsTrue(true),
            failure: _ => Assert.Fail("Expected success but got failure.")
        );
    }

    [TestMethod]
    public void Then_WithFailure_ShouldNotExecuteAction()
    {
        var actionExecuted = false;

        var result = FailureOrNothing.Fail("Test failure").Then(() =>
        {
            actionExecuted = true;
        });

        Assert.IsFalse(actionExecuted);
        result.Match(
            success: () => Assert.Fail("Expected failure but got success."),
            failure: error => Assert.AreEqual("Test failure", error.Message)
        );
    }

    [TestMethod]
    public async Task ThenAsync_WithSuccess_ShouldExecuteAction()
    {
        var actionExecuted = false;

        var result = await FailureOrNothing.Succeed().ThenAsync(async () =>
        {
            await Task.Delay(10);
            actionExecuted = true;
        });

        Assert.IsTrue(actionExecuted);
        result.Match(
            success: () => Assert.IsTrue(true),
            failure: _ => Assert.Fail("Expected success but got failure.")
        );
    }

    [TestMethod]
    public async Task ThenAsync_WithFailure_ShouldNotExecuteAction()
    {
        var actionExecuted = false;

        var result = await FailureOrNothing.Fail("Test failure").ThenAsync(async () =>
        {
            await Task.Delay(10);
            actionExecuted = true;
        });

        Assert.IsFalse(actionExecuted);
        result.Match(
            success: () => Assert.Fail("Expected failure but got success."),
            failure: error => Assert.AreEqual("Test failure", error.Message)
        );
    }

    [TestMethod]
    public void MatchReturn_ShouldExecuteCorrectFunction()
    {
        var successResult = FailureOrNothing.Succeed().MatchReturn(
            success: () => "Success",
            failure: _ => "Failure"
        );

        var failureResult = FailureOrNothing.Fail("Test failure").MatchReturn(
            success: () => "Success",
            failure: _ => "Failure"
        );

        Assert.AreEqual("Success", successResult);
        Assert.AreEqual("Failure", failureResult);
    }

    [TestMethod]
    public async Task MatchReturnAsync_ShouldExecuteCorrectFunction()
    {
        var successResult = await FailureOrNothing.Succeed().MatchReturnAsync(
            success: async () =>
            {
                await Task.Delay(10);
                return "Success";
            },
            failure: async _ =>
            {
                await Task.Delay(10);
                return "Failure";
            }
        );

        var failureResult = await FailureOrNothing.Fail("Test failure").MatchReturnAsync(
            success: async () =>
            {
                await Task.Delay(10);
                return "Success";
            },
            failure: async _ =>
            {
                await Task.Delay(10);
                return "Failure";
            }
        );

        Assert.AreEqual("Success", successResult);
        Assert.AreEqual("Failure", failureResult);
    }

    [TestMethod]
    public void Catch_WithFailure_ShouldExecuteAction()
    {
        var actionExecuted = false;

        FailureOrNothing.Fail("Test failure").Catch(error =>
        {
            actionExecuted = true;
            Assert.AreEqual("Test failure", error.Message);
        });

        Assert.IsTrue(actionExecuted);
    }

    [TestMethod]
    public void Catch_WithSuccess_ShouldNotExecuteAction()
    {
        var actionExecuted = false;

        FailureOrNothing.Succeed().Catch(error =>
        {
            actionExecuted = true;
        });

        Assert.IsFalse(actionExecuted);
    }

    [TestMethod]
    public void ThrowAsException_WithFailure_ShouldThrowException()
    {
        var result = FailureOrNothing.Fail("Test failure");

        Assert.ThrowsException<Exception>(() => result.ThrowAsException());
    }

    [TestMethod]
    public void ThrowAsException_WithSuccess_ShouldNotThrowException()
    {
        var result = FailureOrNothing.Succeed();

        try
        {
            result.ThrowAsException();
            Assert.IsTrue(true);
        }
        catch
        {
            Assert.Fail("Expected no exception to be thrown.");
        }
    }
}