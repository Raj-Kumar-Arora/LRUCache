namespace LRUCache
{
    public class CacheEntry<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; set; }

        public CacheEntry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
    public class LRUCache<TKey, TValue> (int capacity)
    {
        private readonly Dictionary<TKey, LinkedListNode<CacheEntry<TKey, TValue>>> _cache = new ();
        private readonly LinkedList<CacheEntry<TKey, TValue>> _list = new ();
        private readonly int _capacity = capacity;

        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _list.Remove(node);
                _list.AddFirst(node);
                value = node.Value.Value;

                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        public void Put(TKey key, TValue value)
        {
            var found = _cache.TryGetValue(key, out var node);
            if (found)
            {
                node.Value.Value = value;
                _list.Remove(node);
                _list.AddFirst(node);
            }
            else
            {
                var kvpair = new CacheEntry<TKey, TValue>(key, value);
                var newNode = new LinkedListNode<CacheEntry<TKey, TValue>>(kvpair);
                _list.AddFirst(newNode);
                _cache.Add(key, newNode);

                if (_list.Count > _capacity)
                {
                    var lastNode = _list.Last;
                    _list.RemoveLast();
                    _ = _cache.Remove(lastNode.Value.Key);
                }
            }
        }
    }
}
