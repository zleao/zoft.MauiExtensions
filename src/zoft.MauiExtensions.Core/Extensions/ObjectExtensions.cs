﻿using System.Reflection;

namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extensions for object type
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Gets the property as value as a <see cref="string"/>
    /// </summary>
    /// <param name="source">The source instance.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="fallbackValue">Fallback value in case of null</param>
    /// <returns></returns>
    public static string GetPropertyValueAsString(this object source, string propertyName, string fallbackValue = "")
    {
        return source?.GetPropertyValue(propertyName)?.ToString() ?? fallbackValue;
    }

    /// <summary>
    /// Gets the property value.
    /// Deals with null object
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    public static object GetPropertyValue(this object source, string propertyName)
    {
        if (source != null)
        {
            var prop = GetPropertyInfoRecursively(source.GetType(), propertyName);
            if (prop != null)
                return prop.GetValue(source);
        }

        return null;
    }

    /// <summary>
    /// Sets the property value.
    /// Deals with null object
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="value">The value.</param>
    public static void SetPropertyValue(this object source, string propertyName, object value)
    {
        if (source != null)
        {
            var prop = GetPropertyInfoRecursively(source.GetType(), propertyName);

            prop?.SetValue(source, value);
        }
    }

    private static PropertyInfo GetPropertyInfoRecursively(Type sourceType, string propertyName)
    {
        if (sourceType != null)
        {
            var typeInfo = sourceType.GetTypeInfo();
            var prop = typeInfo.GetDeclaredProperty(propertyName);

            return prop ?? GetPropertyInfoRecursively(typeInfo.BaseType, propertyName);
        }

        return null;
    }

    /// <summary>
    /// Checks if the object is null and throws <see cref="ArgumentNullException"/>
    /// </summary>
    /// <typeparam name="T">Type of the argument</typeparam>
    /// <param name="argument">The argument.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public static T ThrowIfNull<T>(this T argument, string propertyName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException($"{propertyName} is null");
        }

        return argument;
    }
}
