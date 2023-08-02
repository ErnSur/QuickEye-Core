using System.IO;
using NUnit.Framework;
using OneAsset;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(ScriptableObjectFactory))]
    public class ScriptableObjectFactoryTests
    {
        private SoWithAsset _singletonAsset;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _singletonAsset = Resources.Load<SoWithAsset>(SoWithAsset.ResourcesPath);
            Assert.NotNull(_singletonAsset);
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AssetDatabase.Refresh();
        }

        [SetUp]
        public void Setup()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [TearDown]
        public void Teardown()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [Test]
        public void Should_LoadAssetInstance_When_AssetExists()
        {
            var result = ScriptableObjectFactory.LoadOrCreateInstance<SoWithAsset>();

            Assert.NotNull(result);
            Assert.AreEqual(_singletonAsset, result);
        }

        [Test]
        public void Should_Throw_When_TypeHasMandatorySingletonAttributeButAssetIsMissing()
        {
            Assert.Throws<AssetIsMissingException>(() =>
            {
                ScriptableObjectFactory.LoadOrCreateInstance<SoWithMissingAsset>();
            });
        }

        [Test]
        public void Should_CreateNewInstance_When_TypeHasNonMandatorySingletonAttributeAndAssetIsMissing()
        {
            var result = ScriptableObjectFactory.LoadOrCreateInstance<SoWithNonMandatoryMissingAsset>();

            Assert.NotNull(result);
        }

        [Test]
        public void Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            ScriptableObjectFactory.LoadOrCreateInstance<SoWithCreateAutomatically>();

            FileAssert.Exists(SoWithCreateAutomatically.AbsoluteAssetPath);
        }

        private static void DeleteTestOnlyAssetsIfTheyExist()
        {
            if (Directory.Exists(SoWithCreateAutomatically.RootTestAssetsDirectory))
            {
                AssetDatabase.DeleteAsset(SoWithCreateAutomatically.RootTestAssetsDirectory);
            }
        }
    }
}