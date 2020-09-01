using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

static class ConcurrentDictionaryExtensions
{
    public static bool TryRemoveConditionally<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue previousValueToCompare)
    {
        var collection = (ICollection<KeyValuePair<TKey, TValue>>)dictionary;
        var toRemove = new KeyValuePair<TKey, TValue>(key, previousValueToCompare);
        return collection.Remove(toRemove);
    }
}
