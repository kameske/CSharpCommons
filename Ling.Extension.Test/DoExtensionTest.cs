using System;
using System.Linq;
using Commons;
using Commons.Linq.Extension;
using NUnit.Framework;

namespace Ling.Extension.Test
{
    [TestFixture]
    public class DoExtensionTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        [Test]
        public static void DoTest1_Struct()
        {
            // 構造体配列のDoテスト

            var intList = MakeIntList();

            // 受け取った値が順次出力されること
            var executedEnumerable = intList.Do(i =>
            {
                logger.Info($"Value: {i}");
                // ReSharper disable once RedundantAssignment
                i *= 20;
            });

            // 実行後の内容が変化していないこと
            Assert.IsTrue(executedEnumerable.SequenceEqual(intList));
        }

        [Test]
        public static void DoTest1_Class()
        {
            // クラス配列のDoテスト

            var exList = MakeNullExceptionArray();

            // 受け取った値が順次出力されること
            var executedEnumerable = exList.Do(ex =>
            {
                logger.Info($"Value: {ex}");
                // ReSharper disable once RedundantAssignment
                ex = new Exception();
            });

            // 実行後の内容が変化しないこと
            Assert.IsTrue(executedEnumerable.All(x => x == null));
        }

        [Test]
        public static void DoTest2_Struct()
        {
            // 構造体配列のDoテスト

            var intList = MakeIntList();

            // 受け取った値が順次出力されること
            var executedEnumerable = intList.Do((i, idx) =>
            {
                logger.Info($"Value[{idx}]: {i}");
                // ReSharper disable once RedundantAssignment
                i *= 20;
            });

            // 実行後の内容が変化していないこと
            Assert.IsTrue(executedEnumerable.SequenceEqual(intList));
        }

        [Test]
        public static void DoTest2_Class()
        {
            // クラス配列のDoテスト

            var exList = MakeNullExceptionArray();

            // 受け取った値が順次出力されること
            var executedEnumerable = exList.Do((ex, idx) =>
            {
                logger.Info($"Value[{idx}]: {ex}");
                // ReSharper disable once RedundantAssignment
                ex = new Exception($"Index: {idx}");
            });

            // 実行後の内容が変化しないこと
            Assert.IsTrue(executedEnumerable.All(x => x == null));
        }

        private static int[] MakeIntList()
            => Enumerable.Range(1, 10).ToArray();

        private static Exception[] MakeNullExceptionArray()
            => Enumerable.Range(1, 10)
                .Select(_ => (Exception) null).ToArray();
    }
}