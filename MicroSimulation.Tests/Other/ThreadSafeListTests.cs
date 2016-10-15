using NUnit.Framework;
using MicroSimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroSimulation.Tests
{
    [TestFixture()]
    public class ThreadSafeListTests
    {
        private const int TestItemCount = 5;

        private static readonly int[] TestData = new[] { 10, 7, 3, 8, 1 };

        private ThreadSafeList<int> _list;

        [OneTimeSetUp]
        public void Init()
        {
            _list = new ThreadSafeList<int>();
        }

        [SetUp]
        public void Reset()
        {
            _list.Clear();
            _list.AddRange(TestData);
        }

        [Test()]
        public void AddTest()
        {
            _list.Add(13);

            Assert.AreEqual(TestItemCount + 1, _list.Count);
        }

        [Test()]
        public void ClearTest()
        {
            _list.Clear();

            Assert.AreEqual(0, _list.Count);
        }

        [Test()]
        public void ContainsTest()
        {
            Assert.IsTrue(_list.Contains(1));
            Assert.IsFalse(_list.Contains(11));
        }

        [Test()]
        public void CopyToTest()
        {
            var target = new int[TestItemCount];

            _list.CopyTo(target, 0);

            CollectionAssert.AreEqual(target, _list.ToArray());
        }

        public static IEnumerable<TestCaseData> GetGetEnumeratorTestCases()
        {
            yield return new TestCaseData(new int[] { 10, 7, 3, 8, 1 })
                .SetName("GetEnumerator");

            yield return new TestCaseData(new int[] { })
                .SetName("GetEnumerator - Empty List");
        }

        [
            Test(),
            TestCaseSource("GetGetEnumeratorTestCases")
        ]
        public void GetEnumeratorTest(int[] expected)
        {
            var actual = new List<int>();
            var list = new ThreadSafeList<int>(expected);

            foreach (var i in list)
                actual.Add(i);

            CollectionAssert.AreEqual(
                expected,
                actual.ToArray()
                );
        }

        [Test()]
        public void IndexOfTest()
        {
            Assert.AreEqual(2, _list.IndexOf(3));
            Assert.AreEqual(-1, _list.IndexOf(22));
        }

        [Test()]
        public void InsertTest()
        {
            _list.Insert(2, 6);

            Assert.AreEqual(2, _list.IndexOf(6));
            Assert.AreEqual(3, _list.IndexOf(3));
        }

        [Test()]
        public void RemoveTest()
        {
            _list.Remove(7);

            Assert.AreEqual(-1, _list.IndexOf(7));
            Assert.AreEqual(1, _list.IndexOf(3));
        }

        [Test()]
        public void RemoveAtTest()
        {
            _list.RemoveAt(1);

            Assert.AreEqual(-1, _list.IndexOf(7));
            Assert.AreEqual(1, _list.IndexOf(3));
        }
    }
}