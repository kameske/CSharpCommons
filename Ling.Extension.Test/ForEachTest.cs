using System;
using System.Linq;
using Commons;
using Commons.Linq.Extension;
using NUnit.Framework;

namespace Ling.Extension.Test
{
    [TestFixture]
    public class ForEachTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }


        [Test]
        public static void ForEachTest1_Struct()
        {
            // 構造体配列のForEachテスト

            var intList = MakeIntList();
            var intCloneList = MakeIntList();

            // 受け取った値が順次出力されること
            intList.ForEach(i =>
            {
                logger.Info($"Value: {i}");
                // ReSharper disable once RedundantAssignment
                i *= 20;
            });

            // 実行後の内容が変化していないこと
            Assert.IsTrue(intList.SequenceEqual(intCloneList));
        }

        [Test]
        public static void ForEachTest1_Class()
        {
            // クラス配列のForEachテスト

            var exList = MakeNullExceptionArray();

            // 受け取った値が順次出力されること
            exList.ForEach(ex =>
            {
                logger.Info($"Value: {ex}");
                // ReSharper disable once RedundantAssignment
                ex = new Exception();
            });

            // 実行後の内容が変化しないこと
            Assert.IsTrue(exList.All(x => x == null));
        }

        [Test]
        public static void ForEachTest2_Struct()
        {
            // 構造体配列のForEachテスト

            var intList = MakeIntList();
            var intCloneList = MakeIntList();

            // 受け取った値が順次出力されること
            intList.ForEach((i, idx) =>
            {
                logger.Info($"Value[{idx}]: {i}");
                // ReSharper disable once RedundantAssignment
                i *= 20;
            });

            // 実行後の内容が変化していないこと
            Assert.IsTrue(intList.SequenceEqual(intCloneList));
        }

        [Test]
        public static void ForEachTest2_Class()
        {
            // クラス配列のForEachテスト

            var exList = MakeNullExceptionArray();

            // 受け取った値が順次出力されること
            exList.ForEach((ex, idx) =>
            {
                logger.Info($"Value[{idx}]: {ex}");
                // ReSharper disable once RedundantAssignment
                ex = new Exception($"Index: {idx}");
            });

            // 実行後の内容が変化しないこと
            Assert.IsTrue(exList.All(x => x == null));
        }

        private static int[] MakeIntList()
            => Enumerable.Range(1, 10).ToArray();

        private static Exception[] MakeNullExceptionArray()
            => Enumerable.Range(1, 10)
                .Select(_ => (Exception)null).ToArray();
    }
}