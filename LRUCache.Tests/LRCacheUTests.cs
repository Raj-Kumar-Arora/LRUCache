namespace LRUCache.Tests
{
    public class LRCacheUTests
    {
        [Fact]
        public void Test_BasicGET()
        {
            var lruCache = new LRUCache<char, int>(3);
            lruCache.Put('A', 1);
            var expectedValue = 1;
            var returnValue = lruCache.TryGet('A', out int actualValue);
            Assert.True(returnValue);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Test_CacheMiss()
        {
            var lruCache = new LRUCache<char, int>(3);
            var returnValue = lruCache.TryGet('A', out int actualValue);
            Assert.False(returnValue);
        }

        [Fact]
        public void Test_LruEviction()
        {
            var lruCache = new LRUCache<char, int>(2);
            lruCache.Put('A', 1);
            lruCache.Put('B', 2);
            lruCache.Put('C', 3);

            var returnValue = lruCache.TryGet('A', out int actualValue);
            Assert.False(returnValue);
        }

        [Fact]
        public void Test_AccessChangesLruOrder()
        {
            var lruCache = new LRUCache<char, int>(2);
            lruCache.Put('A', 1);
            lruCache.Put('B', 2);
            lruCache.Put('A', 4);
            lruCache.Put('C', 3);

            var returnValue = lruCache.TryGet('B', out int actualValueB);
            Assert.False(returnValue);

            returnValue = lruCache.TryGet('A', out int actualValueA);
            Assert.True(returnValue);
        }

        [Fact]
        public void Test_UpdateExistingKey()
        {
            var lruCache = new LRUCache<char, int>(2);
            lruCache.Put('A', 1);
            lruCache.Put('B', 2);
            lruCache.Put('A', 3);

            var expectedValue = 3; 
            lruCache.TryGet('A', out int actualValue);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Test_UpdatingChangesRecency()
        {
            var lruCache = new LRUCache<char, int>(2);
            lruCache.Put('A', 1);
            lruCache.Put('B', 2);
            lruCache.Put('A', 4);
            lruCache.Put('C', 3);

            var returnValue = lruCache.TryGet('B', out int actualValueB);
            Assert.False(returnValue);

            var expectedValue = 4;
            lruCache.TryGet('A', out int actualValueA);
            Assert.Equal(expectedValue, actualValueA);
        }
    }
}
