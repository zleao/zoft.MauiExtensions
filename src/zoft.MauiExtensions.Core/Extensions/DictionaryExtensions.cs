﻿namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extensions for IDictionary type
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Removes the pair (Key, Value) if key exists in dictionary.
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="key">The key.</param>
    public static void RemoveIfExists<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
    {
        if (source?.ContainsKey(key) == true)
        {
            source.Remove(key);
        }
    }

    /// <summary>
    /// Adds or updates a pair (Key, Value).
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source == null)
        {
            return;
        }

        if (source.ContainsKey(key))
        {
            source[key] = value;
        }
        else
        {
            source.Add(key, value);
        }
    }

    /// <summary>
    /// Adds or updates a list of pair (Key, Value).
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="items">The items.</param>
    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        var keyValuePairs = items.ToList();
        if (source == null || keyValuePairs.Count(0) == 0)
        {
            return;
        }

        foreach (var item in keyValuePairs)
        {
            if (source.ContainsKey(item.Key))
            {
                source[item.Key] = item.Value;
            }
            else
            {
                source.Add(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Adds or ignore the present keys of a list of pair (Key, Value).
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="items">The items.</param>
    public static void AddOrIgnore<TKey, TValue>(this IDictionary<TKey, TValue> source, IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        var keyValuePairs = items.ToList();
        if (source == null || keyValuePairs.Count(0) == 0)
        {
            return;
        }

        foreach (var item in keyValuePairs)
        {
            if (!source.ContainsKey(item.Key))
            {
                source.Add(item.Key, item.Value);
            }
        }
    }

    /// <summary>
    /// Applies the action to every item in the list
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="action">The action.</param>
    /// <returns></returns>
    public static IDictionary<T1, T2> ForEach<T1, T2>(this IDictionary<T1, T2> source, Action<T1, T2> action)
    {
        if (source?.Count > 0)
        {
            foreach (var item in source)
            {
                action(item.Key, item.Value);
            }
        }

        return source;
    }

    /// <summary>
    /// Converts the dictionary to a tuple list.
    /// Deals with null dictionaries
    /// </summary>
    /// <typeparam name="T1">The type of the 1.</typeparam>
    /// <typeparam name="T2">The type of the 2.</typeparam>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static IList<(T1 Key, T2 Value)> ToTupleList<T1, T2>(this IDictionary<T1, T2> source)
    {
        var result = new List<(T1, T2)>();

        if (source.Count(0) > 0)
        {
            result.AddRange(source.Select(o => (o.Key, o.Value)));
        }

        return result;
    }
}
