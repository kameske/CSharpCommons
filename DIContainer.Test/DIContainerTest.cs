using System;
using Commons;
using NUnit.Framework;

namespace DIContainer.Test
{
    [TestFixture]
    public class DIContainerTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();

            Commons.DIContainer.Register(MakeTestClass, Commons.DIContainer.Lifetime.Container);
            Commons.DIContainer.Register(MakeTestClass, Commons.DIContainer.Lifetime.Container,
                nameof(DIContainerTest));
        }

        [Test]
        public static void ContainerConfigTest()
        {
            Assert.NotNull(Commons.DIContainer.ContainerConfig);
        }

        private static readonly object[] RegisterTestCaseSource =
        {
            new object[] {null, null, null, true},
            new object[] {(Func<ContainerTestClass>) MakeTestClass, null, "key", true},
            new object[] {null, Commons.DIContainer.Lifetime.Transient, null, true},
            new object[] {(Func<ContainerTestClass>) MakeTestClass, Commons.DIContainer.Lifetime.Transient, null, false},
            new object[] {(Func<ContainerTestClass>) MakeTestClass, Commons.DIContainer.Lifetime.Transient, "key", false},
        };

        [TestCaseSource(nameof(RegisterTestCaseSource))]
        public static void RegisterTest(
            Func<ContainerTestClass> createMethod, Commons.DIContainer.Lifetime lifetime,
            string key,
            bool isError)
        {
            var errorOccured = false;
            try
            {
                Commons.DIContainer.Register(createMethod, lifetime, key);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static readonly object[] ResolveTestCaseSource =
        {
            // key名でインスタンス作成関数登録済み
            new object[] {null, false},
            new object[] {nameof(DIContainerTest), false},
            // インスタンス作成関数未登録
            new object[] {$"{nameof(DIContainerTest)}-2", true},
        };

        [TestCaseSource(nameof(ResolveTestCaseSource))]
        public static void ResolveTest(string key, bool isError)
        {
            var errorOccured = false;
            try
            {
                var _ = Commons.DIContainer.Resolve<ContainerTestClass>(key);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);
        }

        private static object[] HasCreateMethodTestCaseSource =
        {
            // key名でインスタンス作成関数登録済み
            new object[] {null, false, true},
            new object[] {nameof(DIContainerTest), false, true},
            // インスタンス作成関数未登録
            new object[] {$"{nameof(DIContainerTest)}-2", false, false},
        };

        [TestCaseSource(nameof(HasCreateMethodTestCaseSource))]
        public static void HasCreateMethodTest(string key, bool isError, bool answer)
        {
            var errorOccured = false;
            var result = false;
            try
            {
                result = Commons.DIContainer.HasCreateMethod<ContainerTestClass>(key);
            }
            catch (Exception ex)
            {
                logger.Exception(ex);
                errorOccured = true;
            }

            // エラーフラグが一致すること
            Assert.AreEqual(errorOccured, isError);

            if (errorOccured) return;

            // 結果が意図した値であること
            Assert.AreEqual(result, answer);
        }

        [Test]
        public static void LifetimeTest()
        {
            {
                /*
                 * ライフタイム = Container で登録した場合、
                 * インスタンス取得するたびに同じインスタンスが取得できること。
                 */
                const string containerName = "Lifetime.Container Test Container";
                Commons.DIContainer.Register(MakeTestClass, Commons.DIContainer.Lifetime.Container, containerName);

                var instance1 = Commons.DIContainer.Resolve<ContainerTestClass>(containerName);
                var instance2 = Commons.DIContainer.Resolve<ContainerTestClass>(containerName);

                Assert.IsTrue(ReferenceEquals(instance1, instance2));
            }
            {
                /*
                 * ライフタイム = Transient で登録した場合、
                 * インスタンス取得するたびに異なるインスタンスが取得できること。
                 */
                const string containerName = "Lifetime.Transient Test Container";
                Commons.DIContainer.Register(MakeTestClass, Commons.DIContainer.Lifetime.Transient, containerName);

                var instance1 = Commons.DIContainer.Resolve<ContainerTestClass>(containerName);
                var instance2 = Commons.DIContainer.Resolve<ContainerTestClass>(containerName);

                Assert.IsFalse(ReferenceEquals(instance1, instance2));
            }
        }

        private static ContainerTestClass MakeTestClass()
            => new ContainerTestClass();

        public class ContainerTestClass : Commons.DIContainer.IInjectable<ContainerTestClass>
        {
        }
    }
}