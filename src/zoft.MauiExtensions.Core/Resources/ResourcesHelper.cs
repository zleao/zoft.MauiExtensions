namespace zoft.MauiExtensions.Core.Resources
{
    /// <summary>
    /// Helper class to access application resources
    /// </summary>
    public static class ResourcesHelper
    {
        /// <summary>
        /// Get a typed resource from the application resources. </br>
        /// If not found or type is wrong, it reverts to the fallback value
        /// </summary>
        /// <typeparam name="T">Type fo the resource to find</typeparam>
        /// <param name="key">Key of the resource to find</param>
        /// <param name="fallbackValue">Fallback value, if resource is not found or is of wrong type</param>
        public static T GetApplicationResource<T>(string key, T fallbackValue = default)
        {
            if(Application.Current.Resources.TryGetValue(key, out var resource) && resource is T typedResource)
            {
                return typedResource;
            }

            return fallbackValue;
        }
    }
}
