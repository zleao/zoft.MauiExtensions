﻿namespace zoft.MauiExtensions.Core.Extensions;

/// <summary>
/// Extensions for DateTime type
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Formats the specified source.
    /// Deals with null DateTime
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="format">The format.</param>
    /// <returns></returns>
    public static string Format(this DateTime? source, string format)
    {
        return source == null || format.IsNullOrEmpty() ? string.Empty : source.Value.ToString(format);
    }

    /// <summary>
    /// To the short date string.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    public static string ToShortDateString(this DateTime source)
    {
        return source.ToString("d");
    }

    /// <summary>
    /// Retunrs the date part of the DateTime structure
    /// </summary>
    /// <param name="source">The t.</param>
    /// <returns></returns>
    public static DateTime ToDate(this DateTime source)
    {
        return new DateTime(source.Year, source.Month, source.Day);
    }

    /// <summary>
    /// Gets the UTC adjusted time.
    /// </summary>
    /// <param name="source">The date.</param>
    /// <returns></returns>
    public static DateTime GetUtcAdjustedTime(this DateTime source)
    {
        return new DateTime(source.Ticks, DateTimeKind.Utc);
    }


    /// <summary>
    /// Compares the value of this instance to a specified System.DateTime value
    /// and returns an integer that indicates whether this instance is earlier than,
    /// the same as, or later than the specified System.DateTime value.
    /// </summary>
    /// <param name="source">The date.</param>
    /// <param name="dateToCompare">The date to compare.</param>
    /// <returns></returns>
    public static int CompareTo(this DateTime source, DateTime? dateToCompare)
    {
        var safeToCompare = DateTime.MinValue;
        if (dateToCompare != null)
            safeToCompare = dateToCompare.Value;

        return source.CompareTo(safeToCompare);
    }

    /// <summary>
    /// Converts the specified string representation of a date and time to its System.DateTime
    /// equivalent using the specified format and culture-specific format information.
    /// The format of the string representation must match the specified format exactly.
    /// </summary>
    /// <param name="source">The string to convert</param>
    /// <param name="format">The format to apply</param>
    /// <param name="provider">The culture-specific format</param>
    /// <param name="fallbackValue">The fallback value.</param>
    /// <returns></returns>
    public static DateTime TryParseExact(this string source, string format, IFormatProvider provider, DateTime fallbackValue = default)
    {
        try
        {
            return DateTime.ParseExact(source, format, provider);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DateTime.TryParseExact - {ex.GetFullDescription()}");
            return fallbackValue;
        }
    }
}
