using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Proof.Core.DataStructures;

namespace Proof.NUnitTests.Core.DataStructures
{
    [TestFixture]
    internal class FixedListTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(20)]
        public void ShouldHaveCorrectCapacity(int capacity)
        {
            var fixedList = new FixedList<int>(capacity);

            Assert.AreEqual(capacity, fixedList.Capacity());
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(1, 10)]
        [TestCase(1, 100)]
        public void ShouldAddSingleValueCorrectly(int capacity, int value)
        {
            var fixedList = new FixedList<int>(capacity);
            fixedList.Add(value);

            Assert.AreEqual(1, fixedList.Index);
            Assert.AreEqual(value, fixedList.Get(0));
            Assert.AreEqual(value, fixedList.First());
        }

        [TestCase(2, 0, 1)]
        [TestCase(3, 0, 1)]
        [TestCase(2, 1, 2)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 1, 2)]
        [TestCase(3, 5, 10, 20)]
        [TestCase(4, 5, 10, 20)]
        [TestCase(3, 20, 5, 10)]
        public void ShouldPerformMultipleSingleAddsCorrectly(int capacity, params int[] values)
        {
            var fixedList = new FixedList<int>(capacity);
            foreach(int current in values)
            {
                fixedList.Add(current);
            }

            Assert.AreEqual(fixedList.Index, values.Length);

            for(int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], fixedList.Get(i));
            }
            Assert.AreEqual(values.First(), fixedList.First());
        }

        [TestCase(2, 0, 1)]
        [TestCase(3, 0, 1)]
        [TestCase(2, 5, 10)]
        [TestCase(3, 5, 10)]
        [TestCase(4, 5, 10)]
        [TestCase(3, 5, 10, 20)]
        [TestCase(4, 5, 10, 20)]
        [TestCase(3, 20, 5, 10)]
        public void ShouldAddMultipleValuesCorrectly(int capacity, params int[] values)
        {
            var fixedList = new FixedList<int>(capacity);
            fixedList.Add(values);

            Assert.AreEqual(values.Length, fixedList.Index);

            for(int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], fixedList.Get(i));
            }
            Assert.AreEqual(values.First(), fixedList.First());
        }

        [TestCase(4, 0, 1, 2, 3)]
        [TestCase(4, 30, 20, 10, 0)]
        [TestCase(5, 0, 1, 2, 3)]
        [TestCase(6, 0, 1, 2, 3, 4, 5)]
        [TestCase(7, 0, 1, 2, 3, 4, 5)]
        [TestCase(6, 0, 10, 20, 30, 40, 50)]
        public void ShouldPerformMultipleMultipleValueAddsCorrectly(int capacity, params int[] values)
        {
            var fixedList = new FixedList<int>(capacity);
            for(int i = 0; i < values.Length; i += 2)
            {
                int[] arr = { values[i], values[i + 1] };
                fixedList.Add(arr);
            }

            Assert.AreEqual(fixedList.Index, values.Length);

            for (int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], fixedList.Get(i));
            }

            Assert.AreEqual(values.First(), fixedList.First());
        }

        [TestCase(0)]
        [TestCase(1, 10)]
        [TestCase(2, 10)]
        [TestCase(2, 10, 20)]
        [TestCase(5, 10, 20, 30, 40, 50)]
        public void ShouldClearCorrectly(int capacity, params int[] values)
        {
            var fixedList = new FixedList<int>(capacity);
            foreach(int value in values)
            {
                fixedList.Add(value);
            }

            fixedList.Clear();
            Assert.AreEqual(0, fixedList.Index);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldErrorOnInsertMoreItemsThanCapacity(int capacity)
        {
            var fixedList = new FixedList<int>(capacity);

            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                for (int i = 0; i <= capacity; i++)
                {
                    fixedList.Add(i);
                }
            });
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void ShouldNotErrorOnFillAfterClear(int capacity)
        {
            Assert.DoesNotThrow(() =>
            {
                var fixedList = new FixedList<int>(capacity);

                for (int i = 0; i < capacity; i++)
                {
                    fixedList.Add(i);
                }

                fixedList.Clear();

                for (int i = 0; i < capacity; i++)
                {
                    fixedList.Add(i);
                }
            }, "FixedList should not error on fill after clear.");
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ShouldErrorOnGetGreaterThanOrEqualToIndex(int capacity)
        {
            var fixedList = new FixedList<int>(capacity);
            if (capacity >= 1)
            {
                fixedList.Add(0);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => fixedList.Get(1));
        }
    }
}
