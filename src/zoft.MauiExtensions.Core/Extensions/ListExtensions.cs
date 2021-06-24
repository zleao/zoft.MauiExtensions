using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace zoft.MauiExtensions.Core.Extensions
{
    /// <summary>
    /// Extensions for IList type
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Finds the zero based index of an item, using the search predicate.
        /// If item not found, returns -1
        /// Deals with null IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="searchPredicate">The search predicate.</param>
        /// <returns></returns>
        public static int FindIndex<T>(this IList<T> source, Func<T, bool> searchPredicate)
        {
            if (source != null)
            {
                var item = source.FirstOrDefault(searchPredicate);
                if (item != null)
                {
                    return source.IndexOf(item);
                }
            }

            return -1;
        }

        /// <summary>
        /// Applies the action to every item in the list
        /// Deals with null IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="action">The action.</param>
        public static IList<T> ForEach<T>(this IList<T> source, Action<T> action)
        {
            if (source?.Count > 0)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }

            return source;
        }

        /// <summary>
        /// Removes all the items filtered by a predicate.
        /// Deals with null IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="filterPredicate">The filter predicate.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static bool RemoveAll<T>(this IList<T> source, Func<T, bool> filterPredicate, bool throwException = false)
        {
            try
            {
                var toRemove = source.Where(filterPredicate).ToArray();
                toRemove.ForEach(i => source.Remove(i));

                return true;
            }
            catch (Exception) when (!throwException)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the last item of a IList.
        /// Deals with null IList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static bool RemoveLast<T>(this IList<T> source, bool throwException = false)
        {
            try
            {
                if (source?.Count > 0)
                {
                    source.RemoveAt(source.Count - 1);
                }

                return true;
            }
            catch (Exception ex) when (!throwException)
            {
                Console.WriteLine($"IList.RemoveLast - {ex.GetFullDescription()}");
                return false;
            }
        }

        /// <summary>
        /// Adds an item to list if it is not allready in the list.
        /// Deals with null IList
        /// </summary>
        /// <param name="targetList">The source.</param>
        /// <param name="itemToAdd">The item to add.</param>
        public static void AddMissing(this IList targetList, object itemToAdd)
        {
            if (targetList == null)
            {
                return;
            }

            if (!targetList.Contains(itemToAdd))
            {
                targetList.Add(itemToAdd);
            }
        }
    }
}
