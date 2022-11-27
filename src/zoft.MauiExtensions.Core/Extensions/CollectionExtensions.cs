namespace zoft.MauiExtensions.Core.Extensions
{
    /// <summary>
    /// Extensions for ICollection type
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines if the collection is null or empty.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the collection is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source.Count(0) == 0;
        }

        /// <summary>
        /// Adds the range of items.
        /// Deals with null collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="items">The items.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items, bool throwException = false)
        {
            if (source == null || items == null)
                return;

            try
            {
                var enumeratedItems = items.ToList();
                if (enumeratedItems.Count > 0)
                {
                    foreach (var item in enumeratedItems)
                    {
                        source.Add(item);
                    }
                }
            }
            catch (Exception) when (!throwException)
            {
            }
        }

        /// <summary>
        /// Adds the missing items to the collection.
        /// Deals with null collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetSollection">The source.</param>
        /// <param name="itemsToAddIfMissing">The items.</param>
        public static void AddMissing<T>(this ICollection<T> targetSollection, IEnumerable<T> itemsToAddIfMissing) => AddMissing(targetSollection, itemsToAddIfMissing, ShouldAddItem);

        /// <summary>
        /// Adds the missing items to the collection.
        /// Deals with null collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetCollection">The source.</param>
        /// <param name="itemsToAddIfMissing">The items.</param>
        /// <param name="validationFunc">The validation function.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <remarks>
        /// The missing item is only added if the validation func returns true
        /// </remarks>
        public static void AddMissing<T>(this ICollection<T> targetCollection,
                                         IEnumerable<T> itemsToAddIfMissing,
                                         Func<T[], T, bool> validationFunc,
                                         bool throwException = false)
        {
            if (targetCollection == null || itemsToAddIfMissing == null || validationFunc == null)
                return;

            try
            {
                var enumeratedItems = itemsToAddIfMissing.ToList();
                if (enumeratedItems.Count > 0)
                {
                    foreach (T item in enumeratedItems)
                    {
                        if (validationFunc.Invoke(targetCollection.ToArray(), item))
                        {
                            targetCollection.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex) when (!throwException)
            {
                Console.WriteLine($"ICollection.AddMissing - {ex.GetFullDescription()}");
            }
        }

        /// <summary>
        /// Default validation Func used in the AddMissing methods
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetCollection">The target collection.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private static bool ShouldAddItem<T>(T[] targetCollection, T item)
        {
            return !targetCollection.Contains(item);
        }

        /// <summary>
        /// Returns the element count of the collection.
        /// If collection is null or empty, returns zero
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="fallbackValue">The fallback value.</param>
        /// <returns></returns>
        public static int Count<T>(this ICollection<T> source, int fallbackValue)
        {
            return source?.Count ?? fallbackValue;
        }
    }
}
