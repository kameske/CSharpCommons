using System;
using Commons;
using NUnit.Framework;

namespace DIContainer.Test
{
    [TestFixture]
    public class ConfigTest
    {
        private static Logger logger;

        [SetUp]
        public static void Setup()
        {
            LoggerInitializer.Init();
            logger = Logger.GetInstance();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase("abc", false)]
        [TestCase("あいうえお", false)]
        [TestCase("abc\r\n123", false)]
        public static void DefaultKeyTest(string key, bool isError)
        {
            var errorOccured = false;
            try
            {
                Commons.DIContainer.ContainerConfig.DefaultKey = key;
            }
            catch (Exception ex)
            {
                errorOccured = true;
                logger.Exception(ex);
            }

            Assert.AreEqual(isError, errorOccured);

            if (errorOccured) return;

            Assert.IsTrue(Commons.DIContainer.ContainerConfig.DefaultKey.Equals(key));
        }
    }
}