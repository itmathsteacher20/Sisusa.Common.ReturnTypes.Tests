using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sisusa.Common.ReturnTypes.Tests
{
    [TestClass]
    public class OptionalTests
    {
        // Test creation of Some and None
        [TestMethod]
        public void TestSome()
        {
            var someValue = Optional<int>.Some(5);
           
            Assert.AreEqual(5, someValue.OrElse(0));
        }

        [TestMethod]
        public void TestEmpty()
        {
            var emptyValue = Optional<int>.Empty();
            
            Assert.AreEqual(0, emptyValue.OrElse(0));
        }

        [TestMethod]
        public void TestOrElse()
        {
            var someValue = Optional<int>.Some(10);
            Assert.AreEqual(10, someValue.OrElse(5));

            var emptyValue = Optional<int>.Empty();
            Assert.AreEqual(5, emptyValue.OrElse(5));
        }

        [TestMethod]
        public void TestIfHasValue()
        {
            var someValue = Optional<int>.Some(10);
            bool wasExecuted = false;
            someValue.IfHasValue(value => wasExecuted = true);
            Assert.IsTrue(wasExecuted);

            var emptyValue = Optional<int>.Empty();
            wasExecuted = false;
            emptyValue.IfHasValue(value => wasExecuted = true);
            Assert.IsFalse(wasExecuted);
        }

        [TestMethod]
        public void TestOrElseGet()
        {
            var someValue = Optional<int>.Some(5);
            Assert.AreEqual(5, someValue.OrElseGet(() => 10));

            var emptyValue = Optional<int>.Empty();
            Assert.AreEqual(10, emptyValue.OrElseGet(() => 10));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestOrThrow()
        {
            var someValue = Optional<int>.Some(5);
            Assert.AreEqual(5, someValue.OrThrow(new Exception("No value")));

            var emptyValue = Optional<int>.Empty();
            emptyValue.OrThrow(new Exception("No value"));
        }

        [TestMethod]
        public void TestMap()
        {
            var someValue = Optional<int>.Some(5);
            var mappedValue = someValue.Map(value => value * 2);
            Assert.AreEqual(10, mappedValue.OrElse(0));

            var emptyValue = Optional<int>.Empty();
            var mappedEmptyValue = emptyValue.Map(value => value * 2);
            Assert.AreEqual(0, mappedEmptyValue.OrElse(0));
        }

        [TestMethod]
        public async Task TestMapAsync()
        {
            var someValue = Optional<int>.Some(5);
            var mappedValue = await someValue.MapAsync(value => Task.FromResult(value * 2));
            Assert.AreEqual(10, mappedValue.OrElse(0));

            var emptyValue = Optional<int>.Empty();
            var mappedEmptyValue = await emptyValue.MapAsync(value => Task.FromResult(value * 2));
            Assert.AreEqual(0, mappedEmptyValue.OrElse(0));
        }

        [TestMethod]
        public void TestFlatMap()
        {
            var someValue = Optional<int>.Some(5);
            var mappedValue = someValue.FlatMap(value => Optional<int>.Some(value * 2));
            Assert.AreEqual(10, mappedValue.OrElse(0));

            var emptyValue = Optional<int>.Empty();
            var mappedEmptyValue = emptyValue.FlatMap(value => Optional<int>.Some(value * 2));
            Assert.AreEqual(0, mappedEmptyValue.OrElse(0));
        }

        [TestMethod]
        public void TestThen()
        {
            var someValue = Optional<int>.Some(5);
            var result = someValue.Then(value => Optional<int>.Some(value * 2));
            Assert.AreEqual(10, result.OrElse(0));

            var emptyValue = Optional<int>.Empty();
            var emptyResult = emptyValue.Then(value => Optional<int>.Some(value * 2));
            Assert.AreEqual(0, emptyResult.OrElse(0));
        }

        [TestMethod]
        public void TestMatch()
        {
            var someValue = Optional<int>.Some(5);
            bool wasSomeCalled = false;
            bool wasNoneCalled = false;
            someValue.Match(value => wasSomeCalled = true, () => wasNoneCalled = true);
            Assert.IsTrue(wasSomeCalled);
            Assert.IsFalse(wasNoneCalled);

            var emptyValue = Optional<int>.Empty();
            wasSomeCalled = false;
            wasNoneCalled = false;
            emptyValue.Match(value => wasSomeCalled = true, () => wasNoneCalled = true);
            Assert.IsFalse(wasSomeCalled);
            Assert.IsTrue(wasNoneCalled);
        }

        [TestMethod]
        public async Task TestMatchAsync()
        {
            var someValue = Optional<int>.Some(5);
            bool wasSomeCalled = false;
            bool wasNoneCalled = false;
            await someValue.MatchAsync(
                async value => { wasSomeCalled = true; await Task.FromResult(true); },
                async () => { wasNoneCalled = true; await Task.FromResult(true); });
            Assert.IsTrue(wasSomeCalled);
            Assert.IsFalse(wasNoneCalled);

            var emptyValue = Optional<int>.Empty();
            wasSomeCalled = false;
            wasNoneCalled = false;
            await emptyValue.MatchAsync(
                async value => { wasSomeCalled = true; await Task.FromResult(true); },
                async () => { wasNoneCalled = true; await Task.FromResult(true); });
            Assert.IsFalse(wasSomeCalled);
            Assert.IsTrue(wasNoneCalled);
        }
    }
}
