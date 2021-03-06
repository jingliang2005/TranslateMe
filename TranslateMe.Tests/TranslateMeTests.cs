﻿using Should;
using NUnit.Framework;

namespace TranslateMe.Tests
{
    [TestFixture]
    public class TranslateMeTests
    {
        TMLanguagesLoader loader;

        [OneTimeSetUp]
        public void LoadTranslations()
        {
            loader = new TMLanguagesLoader(TM.Instance);

            loader.AddTranslation("SayHello", "en", "Hello");
            loader.AddTranslation("SayHello", "fr", "Bonjour");
        }

        /// <summary>
        /// All static translations tests are in this method because
        /// of the concurent way of executing tests. 
        /// This to avoid conflicts between tests
        /// </summary>
        [Test]
        public void StaticBasicTranslations()
        {
            TM.Tr("TestNoTextId", "Test").ShouldEqual("Test");
            TM.Tr("SayHello", "SH").ShouldEqual("Hello");
            TM.Instance.CurrentLanguage = "fr";
            TM.Tr("SayHello", "SH").ShouldEqual("Bonjour");
            TM.Tr("SayHello", "SH", "en").ShouldEqual("Hello");
            TM.Tr("SayHello", "SH", "es").ShouldEqual("SH");
        }

        [OneTimeTearDown]
        public void ClearDicts()
        {
            loader.ClearAllTranslations();
        }
    }
}
