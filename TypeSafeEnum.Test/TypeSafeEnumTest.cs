using System.Collections.Generic;
using Commons;
using Commons.Linq.Extension;
using NUnit.Framework;

namespace TypeSafeEnum.Test
{
    [TestFixture]
    public class TypeSafeEnumTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        public class TestEnum : TypeSafeEnum<TestEnum>
        {
            public static TestEnum Item1 { get; }
            public static TestEnum Item2 { get; }
            public static TestEnum Item3 { get; }
            public static TestEnum Item4 { get; }
            public static TestEnum Item5 { get; } = new TestEnum(nameof(Item5));
            public static TestEnum ItemNull { get; }

            static TestEnum()
            {
                Item1 = new TestEnum(nameof(Item1));
                Item2 = new TestEnum(nameof(Item2));
                Item3 = new TestEnum(nameof(Item3));
                Item4 = new TestEnum(nameof(Item4));

                ItemNull = new TestEnum("null");
                // TODO: ↓コメントアウト解除してテスト実行した場合例外が発生すること
                // ItemNull = new TestEnum(null);
            }

            public TestEnum(string id) : base(id)
            {
            }

            public static IEnumerable<TestEnum> All()
                => AllItems;
        }

        public class TestEnum_DupItemId : TypeSafeEnum<TestEnum_DupItemId>
        {
            public static TestEnum_DupItemId Item1 { get; }
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public static TestEnum_DupItemId Item2 { get; }

            static TestEnum_DupItemId()
            {
                Item1 = new TestEnum_DupItemId(nameof(Item1));
                // TODO: ↓コメントアウト解除してテスト実行した場合 IdDuplicateTest が失敗すること
                // Item2 = new TestEnum_DupItemId(nameof(Item1));
            }

            public TestEnum_DupItemId(string id) : base(id)
            {
            }
        }

        [Test]
        public static void IdTest()
        {
            Assert.AreEqual(TestEnum.Item1.Id, nameof(TestEnum.Item1));
            Assert.AreEqual(TestEnum.Item4.Id, nameof(TestEnum.Item4));
        }

        private static readonly object[] OperatorEqualTestCaseSource =
        {
            new object[] {null, null, true},
            new object[] {null, TestEnum.Item1, false},
            new object[] {TestEnum.Item2, null, false},
            new object[] {TestEnum.Item1, TestEnum.Item1, true},
            new object[] {TestEnum.Item1, TestEnum.Item2, false},
        };

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorEqualTest(TestEnum left, TestEnum right, bool answer)
        {
            var result = left == right;
            Assert.AreEqual(result, answer);
        }

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void OperatorNotEqualTest(TestEnum left, TestEnum right, bool answerIsEqual)
        {
            var result = left != right;
            Assert.AreEqual(result, !answerIsEqual);
        }

        [TestCaseSource(nameof(OperatorEqualTestCaseSource))]
        public static void EqualsTest(TestEnum left, TestEnum right, bool answer)
        {
            if (left == null) return;

            var result = left.Equals(right);
            Assert.AreEqual(result, answer);
        }

        [Test]
        public static void RegisteredItemTest()
        {
            // TODO: ログに "TestEnum Item Id : Item5" が出力されないこと
            TestEnum.All().ForEach(
                item => logger.Info($"TestEnum Item Id : {item.Id}"));
        }

        [Test]
        public static void IdDuplicateTest()
        {
            var _ = TestEnum_DupItemId.Item1;
        }
    }
}