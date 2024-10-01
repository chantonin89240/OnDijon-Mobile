using NUnit.Framework;
using OnDijon.Common.Utils.Services;
using OnDijon.Common.Utils.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OnDijon.UnitTest.Common.Services.Tests
{
    class CacheServiceTests
    {
        private const string TestKey = "Test";

        private ICacheService _cacheService;

        [SetUp]
        public void Setup()
        {
            _cacheService = new CacheService();
        }

        [Test]
        public async Task CacheTest()
        {
            //put
            var putObject = new TestObject { StringProperty = "Test", IntProperty = 42, DateTimeProperty = DateTime.UtcNow };
            await _cacheService.Put(TestKey, putObject);


            //get
            var getObject = await _cacheService.Get<TestObject>(TestKey);
            Assert.AreEqual(putObject, getObject);

            //delete
            await _cacheService.Delete<TestObject>(TestKey);
            var nullObject = await _cacheService.Get<TestObject>(TestKey);
            Assert.IsNull(nullObject);
        }

        class TestObject
        {
            public string StringProperty { get; set; }

            public int IntProperty { get; set; }

            public DateTime DateTimeProperty { get; set; }

            public override bool Equals(object obj)
            {
                return obj is TestObject @object &&
                       StringProperty == @object.StringProperty &&
                       IntProperty == @object.IntProperty &&
                       DateTimeProperty == @object.DateTimeProperty;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(StringProperty, IntProperty, DateTimeProperty);
            }
        }
    }
}
